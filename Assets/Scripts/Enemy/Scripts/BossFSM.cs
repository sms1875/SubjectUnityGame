using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossFSM : CombatEvent
{
    public float maxHealth_Phase1 = 100;
    public float maxHealth_Phase2 = 100;
    public float currentHealth = 100;
    public float thinkTime = 0.2f;
    public float stunnedTime = 2f;
    public float meleeRange = 30f;
    public float rushRate = 30f;
    public float rushOverDistance = 10f;
    public float beamRate = 60f;
    public float veneerBreathRate = 20f;
    public float runSpeed;
    public float jumpSpeed;
    public float rushSpeed;
    public float pursuitLimitTime = 10f;
    public float acceleration = 1.5f;
    public float bigBreathHoldingTime = 10f;
    public float splitBreathHoldingTime = 2f;
    public float veneerBreathHoldingTime = 3f;

    public GameObject fireZone;
    public GameObject rushAttackTrigger;
    public GameObject bigBreath;
    public GameObject splitBreath;
    public GameObject veneerPattern;
    public GameObject veneerBreath;
    public Material[] phase2_Materials;

    public List<BossPattern> bossMeleePatterns;
    private float meleeTotalWeight = 0f;

    public RectTransform warningUI;
    public Text warningText;

    private float rushCharge = 0f;
    private float beamCharge = 0f;
    private float veneerBreathCharge = 0f;

    private bool isLook;
    private bool isDie;
    private bool isRun;
    private bool isPhase2;
    private bool isInvincibility;

    [Header("DamageText")]
    [SerializeField]
    private GameObject damageTextPrefab;
    [SerializeField]
    private Transform damageTextSpawPoint;

    private GameObject targetObj;
    private Transform target;
    private Transform viewTf;

    private NavMeshAgent navMeshAgent;
    private Rigidbody rigid;
    private Animator animator;

    private Transform child0;
    private Rigidbody childRigidbody;

    private void Awake()
    {
        targetObj = GameObject.FindGameObjectWithTag("Player");
        target = targetObj.transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        child0 = transform.GetChild(0);
        childRigidbody = child0.GetComponent<Rigidbody>();


        for (int i = 0; i < bossMeleePatterns.Count; i++)
        {
            meleeTotalWeight += bossMeleePatterns[i].weight;
        }
    }

    private void OnEnable()
    {
        StartCoroutine("Think");
    }

    private void Update()
    {
        if (isDie) return;

        Charge(); // 돌진과 즉사급 레이저 쿨타임 계산
        FieldOfView();

        if (isLook)
        {
            LookTarget();
        }
    }

    private void Charge()
    {
        rushCharge += Time.deltaTime;
        beamCharge += Time.deltaTime;
        veneerBreathCharge += Time.deltaTime;
    }

    private void LookTarget()
    {
        Vector3 to = new Vector3(target.position.x, 0f, target.position.z);
        Vector3 from = new Vector3(transform.position.x, 0f, transform.position.z);

        Vector3 direction = (to - from).normalized;
        float angle = Vector3.Angle(direction, transform.forward);

        if (angle <= 0.1f) return;
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(to - from), 3f);
            return;
        }
    }
    private void FieldOfView()
    {
        Ray ray = new Ray(transform.position + transform.up, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, ~LayerMask.GetMask("Stage1_Boss", "Enemy")))
        {
            viewTf = hit.transform;
        }
    }

    private void LateUpdate()
    {
        if (isDie) return;
        if (isLook)
        {
            FreezeVelocity();
        }
    }

    private void FreezeVelocity()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }


    private IEnumerator Think()
    {
        yield return new WaitForSeconds(thinkTime);

        while (true)
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if (beamRate <= beamCharge) // 즉사급 레이저 쿨타임 돌아옴
            {
                StartCoroutine("BigBreath");
                yield break;
            }
            if (distance > meleeRange) // 근접 범위를 벗어남 돌진과 점프 공격
            {
                if (rushRate <= rushCharge) // 돌진 쿨타임 돌아옴
                {
                    StartCoroutine("Rush");
                    yield break;
                }
                else // 돌진 쿨타임이 아직 돌지 않음
                {
                    StartCoroutine("Jump");
                    yield break;
                }
            }
            else if (distance <= meleeRange) // 근접 범위에 있음
            {
                List<BossPattern> bossMeleePatternWeightSortList = bossMeleePatterns.OrderBy(x => x.weight).ToList();
                string patternName = "";
                float weight = 0;
                float selectNum = 0;
                selectNum = meleeTotalWeight * Random.Range(0.0f, 1.0f); // 0.0 ~ 1.0

                for (int i = 0; i < bossMeleePatternWeightSortList.Count; i++)
                {
                    weight += bossMeleePatternWeightSortList[i].weight;
                    if (selectNum <= weight)
                    {
                        patternName = bossMeleePatternWeightSortList[i].patternName;
                        if (patternName == "veneerBreath" && veneerBreathRate > veneerBreathCharge)
                        {
                            patternName = bossMeleePatternWeightSortList[i + 1].patternName;
                        }

                        break;
                    }
                }
                Debug.Log(patternName);

                switch (patternName)
                {
                    case "SplitBreath":
                        StartCoroutine("SplitBreath");
                        break;
                    case "VeneerBreath":
                        StartCoroutine("VeneerBreath");
                        break;
                    case "KickORPunch":
                        int random = Random.Range(0, 2); // 0or1 50%
                        if (random == 0)
                        {
                            StartCoroutine("Kick");
                        }
                        else
                        {
                            StartCoroutine("Punch");
                        }
                        break;
                    default:
                        Debug.Log("Melee Attack Error");
                        break;
                }

                yield break;
            }


            yield return null;
        }

    }

    private IEnumerator BigBreath()
    {
        isLook = true;

        beamCharge = 0f;

        //StartCoroutine("WarningMessage");

        animator.SetTrigger("onBigBreath");

        yield return new WaitForSeconds(1.1f); // ready breath motion

        bigBreath.SetActive(true);

        yield return new WaitForSeconds(bigBreathHoldingTime);

        isLook = false;

        animator.SetTrigger("onBigBreathEnd");

        yield return new WaitForSeconds(1.3f);

        StartCoroutine("Think");
    }

    private IEnumerator WarningMessage()
    {
        warningText.text = "!고열발생!";

        for (int i = 0; i < 3; i++)
        {
            warningUI.localPosition = Vector3.zero;
            yield return new WaitForSeconds(0.3f);
            warningUI.localPosition = Vector3.down * 1000;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator Rush()
    {
        isLook = true;
        if (viewTf != target)
        {
            while (true)
            {
                if (viewTf == target)
                {
                    navMeshAgent.ResetPath();
                    navMeshAgent.speed = 0;
                    isRun = false;

                    break;
                }
                else if (!isRun)
                {
                    navMeshAgent.speed = runSpeed;
                    isRun = true;
                    animator.SetTrigger("onRun");
                }
                navMeshAgent.SetDestination(target.position);

                yield return null;
            }
        }


        rushCharge = 0f;
        navMeshAgent.enabled = false;


        Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 moveDirection = (target.position - transform.position).normalized;
        float distance = Vector3.Distance(to, from);
        float amountOfMovement = 0;

        Vector3 direction = (to - from).normalized;

        animator.SetBool("isRush", true);

        while (true)
        {
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle <= 0.1f)
            {
                isLook = false;
            }
            if (animator.GetBool("isStunned") == false && animator.GetBool("isRush") == false)
            {
                navMeshAgent.speed = 0;
                navMeshAgent.enabled = true;
                break;
            }
            else if (animator.GetBool("isStunned") == true && animator.GetBool("isRush") == false)
            {
                navMeshAgent.speed = 0;
                navMeshAgent.enabled = true;

                yield return new WaitForSeconds(stunnedTime);

                animator.SetBool("isStunned", false);

                break;
            }
            else if (amountOfMovement >= distance + rushOverDistance)
            {
                animator.SetBool("isRush", false);
                rushAttackTrigger.SetActive(false);
                navMeshAgent.speed = 0;
                navMeshAgent.enabled = true;
                break;
            }

            transform.position += moveDirection * rushSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, -0.25f, transform.position.z);
            amountOfMovement = Vector3.Distance(from, new Vector3(transform.position.x, 0, transform.position.z));
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine("Think");
    }
    private IEnumerator Jump()
    {
        isLook = true;
        if (viewTf != target)
        {
            while (true)
            {
                if (viewTf == target)
                {
                    navMeshAgent.ResetPath();
                    navMeshAgent.speed = 0;
                    if (isRun)
                    {
                        isRun = false;
                        animator.SetTrigger("onIdle");
                    }

                    break;
                }
                else if (!isRun)
                {
                    navMeshAgent.speed = runSpeed;
                    isRun = true;
                    animator.SetTrigger("onRun");
                }
                navMeshAgent.SetDestination(target.position);

                yield return null;
            }
        }

        childRigidbody.isKinematic = false;

        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 to = new Vector3(target.position.x, 0, target.position.z) + moveDir;
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        float distance = Vector3.Distance(to, from);

        float currentTime = 0;
        while (true)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= 2)
            {
                break;
            }

            float horizInput = Input.GetAxis("Horizontal");
            float vertInput = Input.GetAxis("Vertical");

            moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDir = target.TransformDirection(moveDir);
            moveDir *= 15;
            moveDir.y = 0;

            to = new Vector3(target.position.x, 0, target.position.z) + moveDir;
            distance = Vector3.Distance(to, from);

        }

        isLook = false;
        isInvincibility = true;
        animator.SetTrigger("onJump");
        childRigidbody.velocity = new Vector3(0, jumpSpeed, 0);


        navMeshAgent.speed = 60f;
        navMeshAgent.SetDestination(to);

        while (true)
        {
            from = new Vector3(transform.position.x, 0, transform.position.z);
            distance = Vector3.Distance(to, from);
            if (distance <= 1f)
            {
                child0.Translate(Vector3.up * (jumpSpeed * -1 * 2.5f) * Time.deltaTime);

                if (child0.localPosition.y <= 0.1f)
                {
                    animator.SetTrigger("onLand");
                    navMeshAgent.velocity = Vector3.zero;
                    navMeshAgent.ResetPath();
                    childRigidbody.isKinematic = true;
                    isInvincibility = false;
                    child0.localPosition = Vector3.zero;
                    child0.localRotation = Quaternion.Euler(Vector3.zero);
                    break;
                }
            }
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine("Think");
    }

    private IEnumerator SplitBreath()
    {
        isLook = true;
        animator.SetTrigger("onSplitBreath");

        yield return new WaitForSeconds(1f); // ready splitBreath motion

        isLook = false;

        splitBreath.SetActive(true);

        yield return new WaitForSeconds(splitBreathHoldingTime);

        animator.SetTrigger("onIdle");

        yield return new WaitForSeconds(0.5f);


        StartCoroutine("Think");
    }

    private IEnumerator VeneerBreath()
    {
        veneerBreathCharge = 0;

        animator.SetTrigger("onVeneerPattern");

        yield return new WaitForSeconds(1f); // ready veneerBreath motion

        if (veneerPattern.activeSelf == false)
        {
            veneerPattern.SetActive(true);
        }

        veneerBreath.SetActive(true);


        yield return new WaitForSeconds(veneerBreathHoldingTime);

        animator.SetTrigger("onVeneerPatternEnd");

        yield return new WaitForSeconds(0.5f);

        StartCoroutine("Think");
    }
    private IEnumerator Kick()
    {
        isLook = true;

        float currentTime = 0;
        navMeshAgent.speed = runSpeed;

        while (true)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            currentTime += Time.deltaTime;

            if (distance <= 2.5f)
            {
                navMeshAgent.ResetPath();
                navMeshAgent.speed = 0;
                navMeshAgent.velocity = Vector3.zero;
                isRun = false;
                animator.SetTrigger("onKick");
                break;
            }
            else if (!isRun)
            {
                isRun = true;
                animator.SetTrigger("onRun");
            }
            else if (distance > meleeRange)
            {
                navMeshAgent.ResetPath();
                navMeshAgent.speed = 0;
                animator.SetTrigger("onIdle");
                isRun = false;

                break;
            }
            else if (currentTime >= pursuitLimitTime)
            {
                navMeshAgent.ResetPath();
                navMeshAgent.speed = 0;
                animator.SetTrigger("onIdle");
                isRun = false;

                break;
            }
            navMeshAgent.SetDestination(target.position);

            yield return null;
        }

        isLook = false;

        yield return new WaitForSeconds(0.5f);

        StartCoroutine("Think");
    }

    private IEnumerator Punch()
    {
        isLook = true;

        float currentTime = 0;
        navMeshAgent.speed = runSpeed;

        while (true)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            currentTime += Time.deltaTime;

            if (distance <= 3.5f)
            {
                navMeshAgent.ResetPath();
                navMeshAgent.speed = 0;
                isRun = false;
                animator.SetTrigger("onPunch");

                break;
            }
            else if (!isRun)
            {
                isRun = true;
                animator.SetTrigger("onRun");
            }
            else if (distance > meleeRange)
            {
                navMeshAgent.ResetPath();
                navMeshAgent.speed = 0;
                animator.SetTrigger("onIdle");
                isRun = false;

                break;
            }
            else if (currentTime >= pursuitLimitTime)
            {
                navMeshAgent.ResetPath();
                navMeshAgent.speed = 0;
                animator.SetTrigger("onIdle");
                isRun = false;

                break;
            }
            navMeshAgent.SetDestination(target.position);

            yield return null;
        }

        isLook = false;

        yield return new WaitForSeconds(1.5f);

        StartCoroutine("Think");
    }

    public void TakeDamage(float damage) // 플레이어 공격 trigger에 들어갈 시, 발동
    {
        if (currentHealth <= 0 || isInvincibility) return;

        if (damageTextPrefab && !isInvincibility)
        {
            GameObject cloneDmgText = Instantiate(damageTextPrefab, damageTextSpawPoint.position, target.rotation);
            cloneDmgText.GetComponent<DamageText>().SetUp(damage);
        }

        currentHealth -= damage;

        if (currentHealth <= 0 && !isPhase2)
        {
            if (navMeshAgent.enabled == true) navMeshAgent.ResetPath();
            isInvincibility = true;
            currentHealth = maxHealth_Phase2;
            isRun = false;
            isLook = false;
            bigBreath.SetActive(false);
            splitBreath.SetActive(false);
            veneerPattern.SetActive(false);
            animator.SetTrigger("onPhase2");
            StopAllCoroutines();

            StartCoroutine("OnPhase2");
        }
        else if (currentHealth <= 0 && isPhase2)
        {
            currentHealth = 0;
            animator.SetTrigger("onDie");
            isLook = false;
            bigBreath.SetActive(false);
            splitBreath.SetActive(false);
            veneerPattern.SetActive(false);
            fireZone.SetActive(false);
            StopAllCoroutines();
            Clear();
            Invoke("OnDie", 10f);
        }
    }

    private IEnumerator OnPhase2()
    {
        yield return new WaitForSeconds(2f);

        GetComponentInChildren<SkinnedMeshRenderer>().materials = phase2_Materials;
        animator.SetTrigger("onPhase2Start");
        fireZone.SetActive(true);

        yield return new WaitForSeconds(2f);

        isPhase2 = true;
        isInvincibility = false;

        // increase speed by acceleration animator & moveSpeed
        animator.speed *= acceleration;
        runSpeed *= acceleration;
        rushSpeed *= acceleration;
        jumpSpeed *= acceleration;

        StartCoroutine("Think");
    }

    private void OnDie()
    {
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos() // Debug Range View
    {
        if (navMeshAgent == null) return;

        Gizmos.color = Color.blue; // Melee Range
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
