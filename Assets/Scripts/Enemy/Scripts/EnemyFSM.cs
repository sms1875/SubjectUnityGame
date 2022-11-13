using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public enum AI_Type { Melee = 0, Projectile, Ray, Jump, Rush, Hide, Fly, Obstacle}
public enum EnemyState { None = -1, Idle = 0, Wander, Pursuit, Attack, Hit, Die }

public class EnemyFSM : MonoBehaviour
{
    public bool isDefence;

    private GameObject targetObj;
    private Transform target;
    private LayerMask targetMask;
    private LayerMask exclusionMask;
    private Transform viewTf;

    [SerializeField]
    private float wanderDistance = 10;
    private Vector3 wanderCenter;

    //[Header("DamageText")]
    //[SerializeField]
    //private GameObject damageTextPrefab;
    //[SerializeField]
    //private Transform damageTextSpawPoint;

    public AI_Type ai_Type;

    private Animator anim;
    private Transform upperBody;
    private Vector3 lookPosition = Vector3.zero;

    [SerializeField]
    private bool isDestroy;

    private Vector3 destination = Vector3.zero;
    private float lastAttackTime;
    public EnemyState enemyState = EnemyState.None;
    public EnemyState preEnemyStatae = EnemyState.None;

    private NavMeshAgent navMeshAgent;
    private Rigidbody rigidbody;
    private AudioSource audioSource;

    private EnemyStatus enemyStatus;
    private EnemyAnimatorController enemyAnimatorController;

    private Image image;
    private Text text;

    private bool isBaseEnemy;
    private bool isAttack;

    private bool isChanging;
    public bool isDead;

    private int flyID;

    private Material[] materials;

    public Transform Target => target;

    private void Awake()
    {
        if (!transform.parent) isDestroy = true;
        if (isDefence)
        {
            targetObj = GameObject.FindGameObjectWithTag("SpaceShip");
            target = targetObj.transform;
            targetMask = LayerMask.GetMask("SpaceShip");
            exclusionMask = LayerMask.GetMask("Enemy", "Default");
        }
        else
        {
            targetObj = GameObject.FindGameObjectWithTag("Player");
            target = targetObj.transform;
            targetMask = LayerMask.GetMask("Player");
            exclusionMask = LayerMask.GetMask("Enemy", "Default");
        }
        enemyStatus = GetComponent<EnemyStatus>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        enemyAnimatorController = GetComponentInChildren<EnemyAnimatorController>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        wanderCenter = transform.position;
        enemyStatus.Defence += 1;
        if (ai_Type == AI_Type.Obstacle)
        {
            materials = GetComponent<MeshRenderer>().materials;
        }
        if (ai_Type == AI_Type.Ray || ai_Type == AI_Type.Hide)
        {
            upperBody = anim.GetBoneTransform(HumanBodyBones.UpperChest);
        }

        image = GetComponentInChildren<Image>();
        text = GetComponentInChildren<Text>();

        if(ai_Type == AI_Type.Fly)
        {
            flyID = navMeshAgent.agentTypeID;
            navMeshAgent.agentTypeID = 0;
        }

        if(transform.parent && transform.parent.parent && transform.parent.parent.CompareTag("SpawnerGroup"))
        {
            isBaseEnemy = true;
        }
    }

    private void OnEnable()
    {
        if (ai_Type == AI_Type.Obstacle) return;
        GameManager.enemyCurrentNum++;
        MeleeEvent.cnt++;
        if (image && text)
        {
            image.fillAmount = enemyStatus.HealthPoint / enemyStatus.MaxHealthPoint;
            text.text = $"{enemyStatus.HealthPoint}/{enemyStatus.MaxHealthPoint}";
        }
        if (ai_Type != AI_Type.Hide)
        {
            ChangeState(EnemyState.Idle);
        }
    }

    private void OnDisable()
    {
        if (ai_Type == AI_Type.Obstacle) return;
        StopCoroutine(enemyState.ToString());

        enemyState = EnemyState.None;
    }

    private void LateUpdate()
    {
        if (ai_Type == AI_Type.Obstacle) return;
        if (enemyState == EnemyState.Die)
        {
            return;
        }
        FieldOfView();
        FreezeVelocity();

        if ((ai_Type == AI_Type.Ray || ai_Type == AI_Type.Hide) && enemyAnimatorController.IsBattle)
        {
            RayAndHideLook();
        }
    }

    private void RayAndHideLook()
    {
        if (!isAttack && viewTf != null && viewTf.CompareTag("Player"))
        {
            lookPosition = new Vector3(target.position.x, target.position.y, target.position.z);
        }
        upperBody.LookAt(lookPosition);
        upperBody.rotation = upperBody.rotation * Quaternion.Euler(new Vector3(0,136,-70));
    }

    private void FieldOfView()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, 60, targetMask);
        int index = 0;
        
        while (index < colls.Length)
        {
            Transform targetTf = colls[index].transform;

            if (targetTf.tag == target.tag)
            {
                Vector3 direction = (targetTf.position - transform.position).normalized;
                float angle = Vector3.Angle(direction, transform.forward);

                if (angle <= enemyStatus.ViewAngle * 0.5f)
                {
                    Ray ray = new Ray(transform.position + new Vector3(0, 0.2f, 0), direction);
                    RaycastHit hit;
                    
                    if(Physics.Raycast(ray, out hit, 60, ~exclusionMask))
                    {
                        viewTf = hit.transform;
                        //Debug.Log("보고 있는 트랜스폼: " + hit.transform.name);
                        if(hit.transform.tag == target.tag && !enemyAnimatorController.IsBattle)
                        {
                            if(ai_Type!=AI_Type.Hide) ChangeState(EnemyState.Pursuit);
                            else    //AI_Type.Hide
                            {
                                StartCoroutine("Attack");
                            }
                        }
                        else if(ai_Type == AI_Type.Hide)
                        {
                            enemyAnimatorController.IsBattle = false;
                            StopCoroutine("Attack");
                        }
                    }
                }
            }
            index++;
        }
    }

    private void FreezeVelocity()
    {
        rigidbody.velocity = new Vector3(0, 0, 0);
        rigidbody.angularVelocity = new Vector3(0, 0, 0);
    }

    public void ChangeState(EnemyState newState)
    {
        if (ai_Type == AI_Type.Obstacle) return;
        if (enemyState == newState) return;
        audioSource.Stop();

        if (ai_Type == AI_Type.Fly && !isChanging)
        {
            StartCoroutine("FlyChangeState", newState);
        }
        else
        {
            StopCoroutine(enemyState.ToString());
            StopCoroutine("AutoChangeFromIdleToWander");

            navMeshAgent.acceleration = 10;
            preEnemyStatae = enemyState;
            enemyState = newState;

            StartCoroutine(enemyState.ToString());
        }
    }

    private IEnumerator FlyChangeState(EnemyState newState)
    {
        if(newState == EnemyState.Die)
        {
            StopCoroutine(enemyState.ToString());
            StartCoroutine("Die");
            yield break;
        }
        isChanging = true;

        StopCoroutine(enemyState.ToString());
        StopCoroutine("AutoChangeFromIdleToWander");
        navMeshAgent.acceleration = 10;

        if (newState == EnemyState.Pursuit || newState == EnemyState.Attack || newState == EnemyState.Hit)
        {
            enemyAnimatorController.HitState = 1;
            if (enemyState == EnemyState.Idle || enemyState == EnemyState.Wander)
            {
                enemyAnimatorController.OnTakeOff();
                yield return new WaitForSeconds(4.5f);
                enemyAnimatorController.MoveSpeed = 1f;
                navMeshAgent.agentTypeID = flyID;
            }
        }
        else if(newState == EnemyState.Idle || newState == EnemyState.Wander)
        {
            if(enemyState == EnemyState.Pursuit || enemyState == EnemyState.Attack || enemyState == EnemyState.Hit)
            {
                Debug.Log("Land");
                enemyAnimatorController.HitState = 1;
                enemyAnimatorController.OnLand();

                yield return new WaitForSeconds(3f);
                navMeshAgent.agentTypeID = 0;
            }
        }

        preEnemyStatae = enemyState;
        enemyState = newState;
        StartCoroutine(enemyState.ToString());

        isChanging = false;
    }

    private IEnumerator Idle()
    {
        navMeshAgent.ResetPath();
        enemyAnimatorController.IsBattle = false;

        StartCoroutine("AutoChangeFromIdleToWander");

        while (true)
        {
            if (enemyAnimatorController.MoveSpeed > 0f) enemyAnimatorController.MoveSpeed -= 0.01f;
            else enemyAnimatorController.MoveSpeed = 0f;

            CalculateDistanceToTragetAndSelectState();

            yield return null;
        }
    }

    private IEnumerator AutoChangeFromIdleToWander()
    {
        int changeTime = Random.Range(3, 10);

        destination = CalculateWanderPosition();

        yield return new WaitForSeconds(changeTime);

        ChangeState(EnemyState.Wander);
    }

    private bool LockWanderRotation()
    {
        Vector3 to = new Vector3(destination.x, 0f, destination.z);
        Vector3 from = new Vector3(transform.position.x, 0f, transform.position.z);

        Vector3 direction = (to - from).normalized;
        float angle = Vector3.Angle(direction, transform.forward);

        if (angle <= 0.1f) return true;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(to - from), 1.5f);
        return false;
    }

    private IEnumerator Wander()
    {
        float currentTime = 0;
        float maxTime = 5;

        Vector3 to = new Vector3(destination.x, transform.position.y, destination.z);

        navMeshAgent.speed = enemyStatus.WalkSpeed;
        navMeshAgent.SetDestination(to);

        while (true)
        {
            currentTime += Time.deltaTime;
            to = new Vector3(destination.x, transform.position.y, destination.z);

            if (enemyAnimatorController.MoveSpeed < 0.5f) enemyAnimatorController.MoveSpeed += 0.02f;
            else enemyAnimatorController.MoveSpeed = 0.5f;

            LockWanderRotation();

            if ((to-transform.position).sqrMagnitude < 0.5f || currentTime >= maxTime)
            {
                ChangeState(EnemyState.Idle);
            }

            CalculateDistanceToTragetAndSelectState();

            yield return null;
        }
    }
    
    private Vector3 CalculateWanderPosition()
    {
        float wanderRadius = 10f;
        int wanderJitter = 0;
        int wanderJitterMin = 0;
        int wanderJitterMax = 360;

        Vector3 rangePosition;
        Vector3 rangeScale;

        if (isBaseEnemy)
        {
            rangePosition = transform.parent.parent.position;
            rangeScale = Vector3.one * 10f;
        }

        else
        {
            rangePosition = wanderCenter;
            rangeScale = Vector3.one * 5;
        }

        wanderJitter = Random.Range(wanderJitterMin, wanderJitterMax);
        Vector3 targetPosition = transform.position + SetAngle(wanderRadius, wanderJitter);

        targetPosition.x = Mathf.Clamp(targetPosition.x, rangePosition.x - rangeScale.x * 0.5f, rangePosition.x + rangeScale.x * 0.5f);
        targetPosition.y = transform.position.y;
        targetPosition.z = Mathf.Clamp(targetPosition.z, rangePosition.z - rangeScale.z * 0.5f, rangePosition.z + rangeScale.z * 0.5f);

        return targetPosition;
    }

    private Vector3 SetAngle(float radius, int angle)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Cos(angle) * radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }

    private IEnumerator Pursuit()
    {
        if (enemyState == EnemyState.Attack)
        {
            navMeshAgent.ResetPath();
            yield return new WaitForSeconds(1.5f);
        }
        navMeshAgent.speed = enemyStatus.RunSpeed;
        enemyAnimatorController.IsBattle = false;
        float extraRotationSpeed = 5f;

        if (ai_Type == AI_Type.Ray && enemyAnimatorController.IsSpin)
        {
            enemyAnimatorController.IsSpin = false;
        }

        float currentTime = 0;
        float onlyPursuitTime = 2;
        bool isOnlyPursuit;
        if (ai_Type==AI_Type.Projectile||ai_Type == AI_Type.Ray)
        {
            isOnlyPursuit = true;
        }
        else
        {
            isOnlyPursuit = false;
        }

        while (true)
        {
            if (enemyAnimatorController.MoveSpeed < 1f) enemyAnimatorController.MoveSpeed += 0.1f;
            else enemyAnimatorController.MoveSpeed = 1f;
            navMeshAgent.SetDestination(target.position);

            if (Vector3.Magnitude(navMeshAgent.velocity) > enemyStatus.RunSpeed / 2)
            {
                navMeshAgent.acceleration = 50;
            }

            Vector3 lookrotation = navMeshAgent.steeringTarget - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);

            if (isOnlyPursuit)
            {
                currentTime += Time.deltaTime;
                if(currentTime >= onlyPursuitTime)
                {
                    isOnlyPursuit = false;
                }
            }
            else
            {
                CalculateDistanceToTragetAndSelectState();
            }

            yield return null;
        }
    }

    private bool LockTargetRotation()
    {
        if(isAttack)
        {
            return false;
        }
        destination = new Vector3(target.position.x, 0, target.position.z);
        
        Vector3 to = new Vector3(destination.x, 0f, destination.z);
        Vector3 from = new Vector3(transform.position.x, 0f, transform.position.z);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(to - from), enemyStatus.SpinSpeed);

        return true;
    }

    private void CalculateDistanceToTragetAndSelectState()
    {
        if (target == null) return;
        float distance = Vector3.Distance(target.position, transform.position);

        // 공격 범위 안보다 깊게 pursuit
        if (ai_Type == AI_Type.Ray || ai_Type == AI_Type.Projectile || ai_Type == AI_Type.Fly)
        {
            if (distance <= enemyStatus.AttackRange - 8f && viewTf != null && viewTf.tag == target.tag)
            {
                ChangeState(EnemyState.Attack);
                return;
            }
            else if (enemyState == EnemyState.Attack && distance <= enemyStatus.PursuitLimitRange)
            {
                ChangeState(EnemyState.Pursuit);
                return;
            }
            else if ((enemyState == EnemyState.Idle || enemyState == EnemyState.Wander || enemyState == EnemyState.Hit) && distance <= enemyStatus.TargetRecognitionRange)
            {
                ChangeState(EnemyState.Pursuit);
                return;
            }
            else if (distance > enemyStatus.PursuitLimitRange && (enemyState == EnemyState.Pursuit || enemyState == EnemyState.Attack))
            {
                navMeshAgent.ResetPath();
                ChangeState(EnemyState.Idle);
                return;
            }
        }
        // 공격 범위에 맞게 pursuit
        else
        {
            if (distance <= enemyStatus.AttackRange && viewTf != null && viewTf.tag == target.tag)
            {
                ChangeState(EnemyState.Attack);
                return;
            }
            else if (enemyState == EnemyState.Attack && distance <= enemyStatus.PursuitLimitRange)
            {
                ChangeState(EnemyState.Pursuit);
                return;
            }
            else if ((enemyState == EnemyState.Idle || enemyState == EnemyState.Wander) && distance <= enemyStatus.TargetRecognitionRange)
            {
                ChangeState(EnemyState.Pursuit);
                return;
            }
            else if (distance > enemyStatus.PursuitLimitRange && (enemyState == EnemyState.Pursuit || enemyState == EnemyState.Attack))
            {
                navMeshAgent.ResetPath();
                ChangeState(EnemyState.Idle);
                return;
            }
        }
    }

    private IEnumerator Attack()
    {
        navMeshAgent.ResetPath();
        navMeshAgent.velocity = Vector3.zero;
        enemyAnimatorController.IsBattle = true;
        //navMeshAgent.speed = 0f;

        switch (ai_Type)
        {
            case AI_Type.Melee:
                while (true)
                {
                    if (enemyAnimatorController.MoveSpeed > 0f) enemyAnimatorController.MoveSpeed -= 0.1f;
                    else enemyAnimatorController.MoveSpeed = 0f;

                    if (Time.time - lastAttackTime > enemyStatus.AttackRate)
                    {
                        isAttack = true;
                        lastAttackTime = Time.time;

                        enemyAnimatorController.OnAttack();

                        yield return new WaitForSeconds(enemyStatus.AttackHoldingTime);

                        isAttack = false;
                    }

                    LockTargetRotation();
                    
                    CalculateDistanceToTragetAndSelectState();

                    yield return null;

                }
            case AI_Type.Projectile:
                while (true)
                {
                    if (enemyAnimatorController.MoveSpeed > 0f) enemyAnimatorController.MoveSpeed -= 0.1f;
                    else enemyAnimatorController.MoveSpeed = 0f;

                    if (Time.time - lastAttackTime > enemyStatus.AttackRate)
                    {
                        LockTargetRotation();
                        isAttack = true;
                        lastAttackTime = Time.time;

                        enemyAnimatorController.OnAttack();
                        yield return new WaitForSeconds(enemyStatus.AttackHoldingTime);
                        isAttack = false;
                    }

                    LockTargetRotation();

                    CalculateDistanceToTragetAndSelectState();
                    yield return null;

                }
             case AI_Type.Ray:
                navMeshAgent.speed = 1f;
                float lastMoveTime = 0;
                lastAttackTime = Time.time - enemyStatus.AttackHoldingTime;
                while (true)
                {
                    if (enemyAnimatorController.MoveSpeed > 0f) enemyAnimatorController.MoveSpeed -= 0.1f;
                    else enemyAnimatorController.MoveSpeed = 0f;

                    if (Time.time - lastAttackTime > enemyStatus.AttackRate && !enemyAnimatorController.IsReload)
                    {
                        enemyAnimatorController.MoveSpeed = 0f;
                        isAttack = true;
                        transform.LookAt(new Vector3(target.position.x, 0f, target.position.z));
                        lastMoveTime = Time.time;
                        lastAttackTime = Time.time;
                        enemyAnimatorController.OnAttack();

                        yield return new WaitForSeconds(enemyStatus.AttackHoldingTime);

                        isAttack = false;
                    }
                    else if (Time.time - lastAttackTime > enemyStatus.AttackRate - 0.3f)
                    {
                        enemyAnimatorController.MoveSpeed = 0f;
                        enemyAnimatorController.IsSpin = false;
                        navMeshAgent.ResetPath();
                    }

                    float randomX = 0f;
                    float randomZ = 0f;
                    if (!enemyAnimatorController.IsStopAiming)
                    {
                        if (enemyAnimatorController.IsBuuletLine)
                        {
                            navMeshAgent.speed = 1f;
                            if (Time.time - lastMoveTime > enemyStatus.MoveTime)
                            {
                                enemyAnimatorController.IsSpin = true;
                                lastMoveTime = Time.time;
                                randomX = Random.Range(-5, 6);
                                randomZ = Random.Range(-5, 6);
                                navMeshAgent.SetDestination(new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ));
                            }
                        }
                    }
                    LockTargetRotation();
                    CalculateDistanceToTragetAndSelectState();

                    yield return null;

                }
            case AI_Type.Jump:
                break;
            case AI_Type.Rush:
                while (true)
                {
                    if (Time.time - lastAttackTime > enemyStatus.AttackRate)
                    {
                        isAttack = true;
                        lastAttackTime = Time.time;
                        Vector3 direction = (transform.position - target.position).normalized;
                        Vector3 rushPoint = target.position + direction * -10;

                        navMeshAgent.acceleration = 50f;
                        navMeshAgent.speed = 100f;
                        enemyAnimatorController.OnAttack();
                        enemyAnimatorController.OnAttackCollision();

                        navMeshAgent.SetDestination(rushPoint);

                        yield return new WaitForSeconds(enemyStatus.AttackHoldingTime);

                        isAttack = false;

                        navMeshAgent.speed = 0f;
                        navMeshAgent.velocity = Vector3.zero;
                        navMeshAgent.ResetPath();
                        navMeshAgent.acceleration = 8f;
                    }

                    LockTargetRotation();

                    CalculateDistanceToTragetAndSelectState();

                    yield return null;
                }
            case AI_Type.Hide:
                while (true)
                {
                    if (Time.time - lastAttackTime > enemyStatus.AttackRate)
                    {
                        lastAttackTime = Time.time;

                        enemyAnimatorController.OnAttack();

                        yield return new WaitForSeconds(enemyStatus.AttackHoldingTime);
                    }

                    if (!enemyAnimatorController.IsStopAiming)
                    {
                        LockTargetRotation();
                    }

                    yield return null;
                }
            case AI_Type.Fly:
                while (true)
                {
                    if (Time.time - lastAttackTime > enemyStatus.AttackRate)
                    {
                        lastAttackTime = Time.time;

                        enemyAnimatorController.OnAttack();

                        yield return new WaitForSeconds(enemyStatus.AttackHoldingTime);
                    }

                    LockTargetRotation();

                    CalculateDistanceToTragetAndSelectState();

                    yield return null;

                }
        }
    }

    public void TakeDamage(float damage, Transform hitTf = null) // 플레이어 공격 trigger에 들어갈 시, 발동
    {
        if (enemyStatus.HealthPoint <= 0) return;

        if (ai_Type == AI_Type.Obstacle)
        {
            enemyStatus.HealthPoint -= damage * (100 - enemyStatus.Defence) / 100;

            if (enemyStatus.HealthPoint <= 0)
            {
                StartCoroutine("OnDissolve");
            }
        }
        else
        {
            if (!isAttack && !enemyAnimatorController.IsBattle)
            {
                StopAllCoroutines();
            }

            //if (damageTextPrefab)
            //{
            //    GameObject cloneDmgText = Instantiate(damageTextPrefab, damageTextSpawPoint.position, target.rotation);
            //    cloneDmgText.GetComponent<DamageText>().SetUp(damage);
            //}

            enemyStatus.HealthPoint -= damage;

            //if (image && text)
            //{
            //    image.fillAmount = enemyStatus.HealthPoint / enemyStatus.MaxHealthPoint;
            //    text.text = $"{enemyStatus.HealthPoint}/{enemyStatus.MaxHealthPoint}";
            //}


            if (enemyStatus.HealthPoint <= 0)
            {
                enemyStatus.HealthPoint = 0;
                StopAllCoroutines();
                ChangeState(EnemyState.Die);
            }
            else if (isAttack || enemyAnimatorController.IsBattle)
            {
                return;
            }
            else
            {
                if (enemyState == EnemyState.Hit) ReHit();
                else ChangeState(EnemyState.Hit);
            }
        }
    }

    private IEnumerator OnDissolve()
    {
        float progress = 1f;
        while (true)
        {
            progress -= 0.3f * Time.deltaTime;
            if (progress <= 0)
            {
                gameObject.SetActive(false);
                yield break;
            }

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].SetFloat("_Progress", progress);
            }

            GetComponent<MeshRenderer>().materials = materials;

            yield return null;
        }
    }

    private IEnumerator Die()
    {
        enemyAnimatorController.IsBattle = false;

        isDead = true;

        navMeshAgent.isStopped = true;

        GameManager.enemyKillNum++;
        GameManager.enemyCurrentNum--;
        MeleeEvent.cnt--;

        enemyAnimatorController.OnDie();

        yield return new WaitForSeconds(2f);

        if (isDestroy) gameObject.SetActive(false);
        else GetComponentInParent<SpawnerController>().OnDeactivateItem(gameObject);
    }

    private IEnumerator Hit()
    {
        navMeshAgent.ResetPath();
        navMeshAgent.speed = 0;

        if (ai_Type != AI_Type.Fly)
        {
            float random = Random.Range(0, 2) / 1;
            enemyAnimatorController.HitState = random;
            enemyAnimatorController.MoveSpeed = 0f;
        }

        enemyAnimatorController.OnHit();


        if (target == null) ChangeState(EnemyState.Idle);
        if (enemyAnimatorController.IsBattle == true) ChangeState(EnemyState.Attack);
        if (ai_Type == AI_Type.Hide) yield break;

        float currentTime = 0;
        float maxTime = 10;

        Vector3 point = target.position;
        Vector3 to = new Vector3(point.x, 0f, point.z);
        Vector3 from;

        while (true)
        {
            from = new Vector3(transform.position.x, 0f, transform.position.z);
            if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(to - from)) <= 1f)
            {
                transform.rotation = Quaternion.LookRotation(to - from);
                break;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(to - from), enemyStatus.SpinSpeed);

            yield return null;
        }

        navMeshAgent.speed = enemyStatus.RunSpeed;
        navMeshAgent.SetDestination(point);
        while (true)
        {
            if (enemyAnimatorController.MoveSpeed < 1f) enemyAnimatorController.MoveSpeed += 0.01f;
            else enemyAnimatorController.MoveSpeed = 1f;

            currentTime += Time.deltaTime;
            if ((point - transform.position).sqrMagnitude <= 0.1f || currentTime >= maxTime)
            {
                navMeshAgent.ResetPath();
                ChangeState(EnemyState.Idle);
            }

            CalculateDistanceToTragetAndSelectState();

            yield return null;
        }
    }
    private void ReHit()
    {
        StopCoroutine("Hit");
        StartCoroutine("Hit");
    }

    //private void OnDrawGizmos()
    //{
    //    if (navMeshAgent == null) return;

    //    Gizmos.color = Color.black;
    //    Gizmos.DrawRay(transform.position, navMeshAgent.destination - transform.position);

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, enemyStatus.TargetRecognitionRange);

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, enemyStatus.PursuitLimitRange);

    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, enemyStatus.AttackRange);
    //}
}
