using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MidBossFSM_1 : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float runSpeed;
    public float jumpSpeed;
    public float thinkTime;
    public float acidRate = 40f;
    public float eggRate = 20f;
    public float meleeRange = 45f;
    public float pursuitLimitTime = 8f;
    public float stunnedTime = 3f;
    public float rushOverDistance = 10f;

    public GameObject acid;
    public GameObject jumpAtaackTrigger;
    public GameObject reapAttackTrigger;

    public List<BossPattern> midBossMeleePatterns;
    private float meleeTotalWeight = 0f;

    public ForestBoss forestBoss;

    public RectTransform warningUI;
    public Text warningText;

    [Header("DamageText")]
    [SerializeField]
    private GameObject damageTextPrefab;
    [SerializeField]
    private Transform damageTextSpawPoint;

    private float acidCharge = 0f;
    private float eggCharge = 0f;

    private bool isDie;
    private bool isLook = true;
    private bool isRun;
    private bool isThink;

    private Transform viewTf;

    private GameObject targetObj;
    private Transform target;

    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rigid;
    private Collider coll;

    private void Awake()
    {
        GameManager.isBoss = true;
        targetObj = GameObject.FindGameObjectWithTag("Player");
        target = targetObj.transform;

        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();

        for (int i = 0; i < midBossMeleePatterns.Count; i++)
        {
            meleeTotalWeight += midBossMeleePatterns[i].weight;
        }
    }

    private void OnEnable()
    {
        StartCoroutine("Think");
    }

    private void Update()
    {
        if (isDie) return;

        Charge();
        FieldOfView();

        if (isLook)
        {
            LookTarget();
        }
    }

    private void LookTarget()
    {
        Vector3 to = new Vector3(target.position.x, 0f, target.position.z);
        Vector3 from = new Vector3(transform.position.x, 0f, transform.position.z);

        Vector3 direction = (to - from).normalized;
        float angle = Vector3.Angle(direction, transform.forward);

        if (angle <= 10f)
        {
            if (isThink && isRun)
            {
                isRun = false;
                animator.SetTrigger("onIdle");
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(to - from), 70f * Time.deltaTime);
        }
        else
        {
            if(isThink && !isRun)
            {
                isRun = true;
                animator.SetTrigger("onRun");
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(to - from), 60f * Time.deltaTime);
            return;
        }
    }

    private IEnumerator GetAway()
    {
        if (!isRun)
        {
            isRun = true;
            animator.SetTrigger("onRun");
        }
        isLook = false;
        isThink = false;
        navMeshAgent.speed = runSpeed;
        Vector3 destination = transform.position - transform.forward * 20;
        navMeshAgent.SetDestination(destination);

        float curretTime = 0;
        while (true)
        {
            curretTime += Time.deltaTime;
            if (Vector3.Distance(destination, transform.position) <= 1 || curretTime >= 3)
            {
                navMeshAgent.ResetPath();
                if (isRun)
                {
                    animator.SetTrigger("onIdle");
                    isRun = false;
                }
                isLook = true;
                StartCoroutine("Think");
                yield break;
            }
        
            yield return null;
        }
    }

    private void FieldOfView()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 0.2f, 0), transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, ~LayerMask.GetMask("MidBoss1")))
        {
            viewTf = hit.transform;
        }
    }

    private void Charge()
    {
        acidCharge += Time.deltaTime;
        eggCharge += Time.deltaTime;
    }

    private void FixedUpdate()
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
        isThink = true;
        yield return new WaitForSeconds(thinkTime);

        while (true)
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if (acidRate <= acidCharge) // 즉사급 레이저 쿨타임 돌아옴
            {
                isThink = false;
                StartCoroutine("Acid");
                yield break;
            }
            else if(eggRate <= eggCharge)
            {
                isThink = false;
                StartCoroutine("Egg");
                yield break;
            }
            else if (distance > meleeRange) // 근접 범위를 벗어남 돌진과 점프 공격
            {
                isThink = false;
                StartCoroutine("Jump");
                yield break;
            }
            else if (distance <= meleeRange) // 근접 범위에 있음
            {
                List<BossPattern> bossMeleePatternWeightSortList = midBossMeleePatterns.OrderBy(x => x.weight).ToList();
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

                        break;
                    }
                }

                switch (patternName)
                {
                    case "PoisonGas":
                        isThink = false;
                        StartCoroutine("PoisonGas");
                        break;
                    case "Reap":
                        isThink = false;
                        StartCoroutine("Reap");
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

    private IEnumerator Acid()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= meleeRange)
        {
            //StartCoroutine("WarningMessage");

            yield return new WaitForSeconds(1.5f);

            acidCharge = 0f;

            animator.SetTrigger("onAcid");

            yield return new WaitForSeconds(1.1f); // ready breath motion

            acid.SetActive(true);
            acid.transform.LookAt(target);

            yield return new WaitForSeconds(3f);

            acid.SetActive(false);
            animator.SetTrigger("onIdle");

            yield return new WaitForSeconds(0.5f);

            isRun = false;
            StartCoroutine("Think");
        }
        else
        {
            StartCoroutine("Jump");
            yield break;
        }
    }
    private IEnumerator WarningMessage()
    {
        warningText.text = "!산성욕액!";

        for (int i = 0; i < 3; i++)
        {
            warningUI.localPosition = Vector3.zero;
            yield return new WaitForSeconds(0.3f);
            warningUI.localPosition = Vector3.down * 1000;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator Egg()
    {
        eggCharge = 0f;

        Collider[] colls = Physics.OverlapSphere(transform.position, 500f, LayerMask.GetMask("InterEgg"));

        if (colls.Length == 0)
        {
            Debug.Log("No Egg Start Think");
            StartCoroutine("Think");
            yield break;
        }

        int cnt = 0;

        for (int i = 0; i < colls.Length; i++)
        {
            cnt = 0;
            float distance = Vector3.Distance(transform.position, colls[0].transform.position);
            for (int j = 0; j < colls.Length - (i + 1); j++)
            {
                float nextDistance = Vector3.Distance(transform.position, colls[j + 1].transform.position);
                if (distance > nextDistance)
                {
                    cnt++;
                    Collider temp = colls[j];
                    colls[j] = colls[j + 1];
                    colls[j + 1] = temp;

                }
                else distance = nextDistance;
            }
            if (cnt == 0) break;
        }

        isLook = false;

        animator.SetTrigger("onShout");

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < 3; i++)
        {
            if (i == colls.Length) break;
            GameObject temp = colls[i].gameObject;

            if (temp.GetComponent<MidBoss1_Egg_AND_Baby>())
            {
                temp.GetComponent<MidBoss1_Egg_AND_Baby>().SetUp();
            }
        }

        yield return new WaitForSeconds(1.5f);

        isLook = true;

        StartCoroutine("Think");
    }

    private IEnumerator PoisonGas()
    {
        float currentTime = 0;
        navMeshAgent.speed = runSpeed;

        while (true)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            currentTime += Time.deltaTime;

            if (distance <= meleeRange - 10 && viewTf == target)
            {
                if (distance <= 5)
                {
                    StartCoroutine("GetAway");
                    yield break;
                }
                navMeshAgent.ResetPath();
                navMeshAgent.speed = 0;
                navMeshAgent.velocity = Vector3.zero;
                isRun = false;
                animator.SetTrigger("onPoisonGas");
                isLook = false;

                yield return new WaitForSeconds(1.5f);

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


        yield return new WaitForSeconds(2f);

        isLook = true;

        StartCoroutine("Think");
    }

    private IEnumerator Reap()
    {
        float currentTime = 0;
        navMeshAgent.speed = runSpeed;

        while (true)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            currentTime += Time.deltaTime;

            if (distance <= 10f && viewTf == target)
            {
                if (distance <= 5)
                {
                    StartCoroutine("GetAway");
                    yield break;
                }
                navMeshAgent.ResetPath();
                navMeshAgent.speed = 0;
                navMeshAgent.velocity = Vector3.zero;
                isRun = false;
                animator.SetTrigger("onReap");
                isLook = false;
                yield return new WaitForSeconds(0.7f);

                reapAttackTrigger.SetActive(true);

                yield return new WaitForSeconds(1.8f);

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

        yield return new WaitForSeconds(0.5f);

        isLook = true;

        StartCoroutine("Think");
    }

    private IEnumerator Jump()
    {
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

        Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 moveDirection = (to - from).normalized;
        float distance = Vector3.Distance(to, from);

        if (distance <= meleeRange)
        {
            animator.SetTrigger("onIdle");
            StartCoroutine("Reap");
            yield break;
        }

        isLook = false;
        animator.SetTrigger("onJump");

        yield return new WaitForSeconds(0.2f);

        navMeshAgent.speed = jumpSpeed;
        navMeshAgent.acceleration = 50f;
        navMeshAgent.SetDestination(to);
        jumpAtaackTrigger.SetActive(true);


        while (true)
        {
            from = new Vector3(transform.position.x, 0, transform.position.z);
            distance = Vector3.Distance(to, from);
            if (distance < 5f)
            {
                navMeshAgent.speed = runSpeed;
                navMeshAgent.acceleration = 8f;
                navMeshAgent.ResetPath();
                animator.SetTrigger("onLand");

                yield return new WaitForSeconds(0.1f);
                jumpAtaackTrigger.SetActive(false);
                navMeshAgent.velocity = Vector3.zero;
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        isLook = true;

        StartCoroutine("Think");
    }

    public void TakeDamage(float damage) // 플레이어 공격 trigger에 들어갈 시, 발동
    {
        if (currentHealth <= 0) return;

        if (damageTextPrefab)
        {
            GameObject cloneDmgText = Instantiate(damageTextPrefab, damageTextSpawPoint.position, target.rotation);
            cloneDmgText.GetComponent<DamageText>().SetUp(damage);
        }

        currentHealth -= damage * 0.5f;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isLook = false;
            isDie = true;
            StopAllCoroutines();
            animator.SetTrigger("onDie");

            StartCoroutine("OnDie");
        }
    }

    private IEnumerator OnDie()
    {
        GameManager.isBoss = false;
        forestBoss.BossDead();
        yield return new WaitForSeconds(5f);

        gameObject.SetActive(false);
    }

    private void OnDrawGizmos() // Debug Range View
    {
        if (navMeshAgent == null) return;

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, navMeshAgent.destination - transform.position);

        Gizmos.color = Color.blue; // Melee Range
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
