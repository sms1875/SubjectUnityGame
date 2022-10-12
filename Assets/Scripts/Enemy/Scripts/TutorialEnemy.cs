using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class TutorialEnemy : MonoBehaviour
{
    private GameObject targetObj;
    private Transform target;
    private LayerMask targetMask;
    private LayerMask exclusionMask;
    private Transform viewTf;

    [Header("DamageText")]
    [SerializeField]
    private GameObject damageTextPrefab;
    [SerializeField]
    private Transform damageTextSpawPoint;

    [SerializeField]
    private AI_Type ai_Type;

    private Vector3 destination = Vector3.zero;
    private float lastAttackTime;

    private NavMeshAgent navMeshAgent;
    private Rigidbody rigidbody;

    private EnemyStatus enemyStatus;
    private EnemyAnimatorController enemyAnimatorController;

    private Image image;
    private Text text;

    public Transform Target => target;

    private bool isLook;
    private bool isDie;

    private void Awake()
    {
        targetObj = GameObject.FindGameObjectWithTag("Player");
        target = targetObj.transform;
        targetMask = LayerMask.GetMask("Player");
        exclusionMask = LayerMask.GetMask("Enemy", "Ignore");
        enemyStatus = GetComponent<EnemyStatus>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        enemyAnimatorController = GetComponentInChildren<EnemyAnimatorController>();

        image = GetComponentInChildren<Image>();
        text = GetComponentInChildren<Text>();

        navMeshAgent.updateRotation = false;
    }

    private void FieldOfView()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, 50, targetMask);
        int index = 0;

        while (index < colls.Length)
        {
            Transform targetTf = colls[index].transform;

            if (targetTf.CompareTag("Player"))
            {
                Vector3 direction = (targetTf.position - transform.position).normalized;
                float angle = Vector3.Angle(direction, transform.forward);

                if (angle <= enemyStatus.ViewAngle * 0.5f)
                {
                    Ray ray = new Ray(transform.position + transform.up, direction);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 50, ~exclusionMask))
                    {
                        viewTf = hit.transform;
                        Debug.Log("보고 있는 트랜스폼: " + hit.transform.name);
                        if (hit.transform.CompareTag("Player") && !enemyAnimatorController.IsBattle)
                        {
                            StartCoroutine("Pursuit");
                            isLook = true;
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

    private void Update()
    {
        if (!isDie)
        {
            if (!isLook)
            {
                FieldOfView();
            }
            FreezeVelocity();
        }
    }

    private bool LockTargetRotation()
    {
        destination = new Vector3(target.position.x, 0, target.position.z);

        Vector3 to = new Vector3(destination.x, 0f, destination.z);
        Vector3 from = new Vector3(transform.position.x, 0f, transform.position.z);

        Vector3 direction = (to - from).normalized;
        float angle = Vector3.Angle(direction, transform.forward);

        if (angle <= 0.1f) return false;
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(to - from), enemyStatus.SpinSpeed);
            return true;
        }
    }

    private IEnumerator Pursuit()
    {
        navMeshAgent.speed = enemyStatus.RunSpeed;
        enemyAnimatorController.IsBattle = false;

        while (true)
        {
            if (enemyAnimatorController.MoveSpeed < 1f) enemyAnimatorController.MoveSpeed += 0.1f;
            else enemyAnimatorController.MoveSpeed = 1f;
            navMeshAgent.SetDestination(target.position);

            LockTargetRotation();

            if (target == null)
            {
                yield return null;
                continue;
            }
            else
            {
                float distance = Vector3.Distance(target.position, transform.position);
                if (distance <= enemyStatus.AttackRange && viewTf != null && viewTf.CompareTag("Player"))
                {
                    StartCoroutine("Attack");
                    yield break;
                }
            }

            yield return null;
        }
    }


    private IEnumerator Attack()
    {
        navMeshAgent.ResetPath();
        navMeshAgent.velocity = Vector3.zero;
        enemyAnimatorController.IsBattle = true;
        navMeshAgent.speed = 0f;
        switch (ai_Type)
        {
            case AI_Type.Melee:
                while (true)
                {
                    if (enemyAnimatorController.MoveSpeed > 0f) enemyAnimatorController.MoveSpeed -= 0.1f;
                    else enemyAnimatorController.MoveSpeed = 0f;

                    if (Time.time - lastAttackTime > enemyStatus.AttackRate)
                    {
                        lastAttackTime = Time.time;

                        enemyAnimatorController.OnAttack();
                    }

                    LockTargetRotation();

                    if (target == null)
                    {
                        yield return null;
                        continue;
                    }
                    else
                    {
                        float distance = Vector3.Distance(target.position, transform.position);
                        if (distance > enemyStatus.AttackRange)
                        {
                            StartCoroutine("Pursuit");
                            yield break;
                        }
                    }

                    yield return null;

                }
            case AI_Type.Projectile:
                while (true)
                {
                    if (enemyAnimatorController.MoveSpeed > 0f) enemyAnimatorController.MoveSpeed -= 0.1f;
                    else enemyAnimatorController.MoveSpeed = 0f;

                    if (Time.time - lastAttackTime > enemyStatus.AttackRate)
                    {
                        lastAttackTime = Time.time;

                        enemyAnimatorController.OnAttack();
                    }

                    LockTargetRotation();

                    yield return null;

                }
        }
    }

    public void TakeDamage(float damage, Transform hitTf = null) // 플레이어 공격 trigger에 들어갈 시, 발동
    {
        if (enemyStatus.HealthPoint <= 0) return;

        StopAllCoroutines();

        if (damageTextPrefab)
        {
            GameObject cloneDmgText = Instantiate(damageTextPrefab, damageTextSpawPoint.position, target.rotation);
            cloneDmgText.GetComponent<DamageText>().SetUp(damage);
        }

        enemyStatus.HealthPoint -= damage;

        if (image && text)
        {
            image.fillAmount = enemyStatus.HealthPoint / enemyStatus.MaxHealthPoint;
            text.text = $"{enemyStatus.HealthPoint}/{enemyStatus.MaxHealthPoint}";
        }


        if (enemyStatus.HealthPoint <= 0)
        {
            enemyStatus.HealthPoint = 0;
            StartCoroutine("Die");
        }
        else
        {
            StopCoroutine("Hit");
            StartCoroutine("Hit");
        }
    }

    private IEnumerator Die()
    {
        isDie = true;
        navMeshAgent.isStopped = true;

        GameManager.enemyKillNum++;
        GameManager.enemyCurrentNum--;

        enemyAnimatorController.OnDie();

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }

    private IEnumerator Hit()
    {
        yield return null;

        navMeshAgent.ResetPath();

        float random = Random.Range(0, 2) / 1;
        enemyAnimatorController.HitState = random;
        enemyAnimatorController.OnHit();

        StopCoroutine("Attack");
        StopCoroutine("Pursuit");
        StartCoroutine("Pursuit");
    }
}
