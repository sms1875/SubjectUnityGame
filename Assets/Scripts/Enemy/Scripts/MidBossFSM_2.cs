using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MidBossFSM_2 : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float runSpeed;
    public float rushSpeed;
    public float thinkTime;
    public float rushRate = 20f;
    public float rockRate = 40f;
    public float meleeRange = 10f;
    public float pursuitLimitTime = 8f;
    public float stunnedTime = 3f;
    public float rushOverDistance = 10f;

    public Transform rockHand;
    public GameObject rushAttackTrigger;
    public GameObject ShoutObj;

    public List<BossPattern> midBossMeleePatterns;
    private float meleeTotalWeight = 0f;

    [Header("DamageText")]
    [SerializeField]
    private GameObject damageTextPrefab;
    [SerializeField]
    private Transform damageTextSpawPoint;

    private float rushCharge = 0f;
    private float rockCharge = 0f;
    private float spinSpeed = 3f;

    private bool isDie;
    private bool isLook = true;
    private bool isRun;
    private bool isLock;

    private Transform viewTf;

    private GameObject targetObj;
    private Transform target;

    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private SkinnedMeshRenderer skin;

    private void Awake()
    {
        GameManager.isBoss = true;

        targetObj = GameObject.FindGameObjectWithTag("Player");
        target = targetObj.transform;

        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        skin = GetComponentInChildren<SkinnedMeshRenderer>();

        for (int i = 0; i < midBossMeleePatterns.Count; i++)
        {
            meleeTotalWeight += midBossMeleePatterns[i].weight;
        }
    }

    private void OnEnable()
    {
        MeleeEvent.cnt++;
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

        if (angle <= 0.1f)
        {
            isLock = true;
            return;
        }
        else
        {
            isLock = false;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(to - from), spinSpeed);
            return;
        }
    }


    private void FieldOfView()
    {
        Ray ray = new Ray(transform.position + transform.up, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, ~LayerMask.GetMask("MidBoss2")))
        {
            viewTf = hit.transform;
        }
    }

    private void Charge()
    {
        rushCharge += Time.deltaTime;
        rockCharge += Time.deltaTime;
    }

    private IEnumerator Think()
    {
        yield return new WaitForSeconds(thinkTime);

        float distance = Vector3.Distance(target.position, transform.position);

        if (rockRate <= rockCharge) // 즉사급 레이저 쿨타임 돌아옴
        {
            StartCoroutine("Rock");
            yield break;
        }

        else if (rushRate <= rushCharge && distance > meleeRange) // 돌진 쿨타임 돌아옴
        {
            StartCoroutine("Rush");
            yield break;
        }

        else
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
            Debug.Log(patternName);

            switch (patternName)
            {
                case "Punch":
                    StartCoroutine("Punch");
                    break;
                case "Shout":
                    StartCoroutine("Shout");
                    break; ;
                default:
                    Debug.Log("Melee Attack Error");
                    break;
            }

            yield break;
        }

    }

    private IEnumerator Rock()
    {
        rockCharge = 0f;

        Collider[] colls = Physics.OverlapSphere(transform.position, 100f, LayerMask.GetMask("InterActive"));

        if (colls.Length == 0) // 주변에 상호작용할 상자가 없음 다시 패턴을 정함
        {
            Debug.Log("NoInteractiveStone");
            StartCoroutine("Think");
            yield break;
        }

        int shortestIndex = 0;
        float shortestDistance = Vector3.Distance(colls[shortestIndex].transform.position, transform.position);

        for (int i = 1; i < colls.Length; i++) // 가장 짧은 거리에 있는 상호작용할 오브젝트 선별
        {
            float distance = Vector3.Distance(colls[i].transform.position, transform.position);
            if (shortestDistance > distance)
            {
                shortestIndex = i;
                shortestDistance = distance;
            }
        }

        Transform interActiveTf = colls[shortestIndex].transform;
        interActiveTf.GetComponent<NavMeshObstacle>().enabled = false;
        RockProjectile rockProjectile = interActiveTf.GetComponent<RockProjectile>();

        isLook = false;
        isLock = false;

        animator.SetTrigger("onRun");
        navMeshAgent.speed = runSpeed;
        navMeshAgent.SetDestination(interActiveTf.position);

        while (true)
        {
            if (rockProjectile.isEnter)
            {
                navMeshAgent.speed = 0;
                navMeshAgent.ResetPath();
                if (rockProjectile.midBoss2 != transform)
                {
                    yield return new WaitForSeconds(1f);

                    StartCoroutine("Think");
                }
                animator.SetTrigger("onIdle");
                animator.SetTrigger("onGet"); // 줍기 애니메이션

                interActiveTf.SetParent(rockHand);
                interActiveTf.localPosition = Vector3.zero;
                interActiveTf.gameObject.layer = 0;

                spinSpeed = 5f;
                isLook = true;
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(1.5f);


        while (true)
        {
            if (isLock)
            {
                spinSpeed = 3f;
                animator.SetTrigger("onThrow"); //던저기 애니메이션
                yield return new WaitForSeconds(0.25f);
                interActiveTf.SetParent(null);
                rockProjectile.SetUp(transform, target);
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine("Think");
    }

    private IEnumerator Rush()
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
                    animator.SetTrigger("onIdle");

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

        isLook = false;
        rushCharge = 0f;
        Vector3 direction = (transform.position - target.position).normalized;
        Vector3 rushPoint = target.position + direction * -rushOverDistance;

        navMeshAgent.acceleration = 15f;
        navMeshAgent.speed = rushSpeed;

        navMeshAgent.SetDestination(rushPoint);
        rushPoint.y = 0;

        animator.SetBool("isRush", true);

        yield return new WaitForSeconds(0.1f);
        float currentTime = 0;
        while (true)
        {
            if (animator.GetBool("isStunned") == false && animator.GetBool("isRush") == false)
            {
                navMeshAgent.speed = 0;
                navMeshAgent.acceleration = 8f;

                break;
            }
            else if (animator.GetBool("isStunned") == true && animator.GetBool("isRush") == false)
            {
                navMeshAgent.speed = 0;
                navMeshAgent.acceleration = 8f;

                yield return new WaitForSeconds(stunnedTime);

                animator.SetBool("isStunned", false);

                break;
            }
            else if (Vector3.Distance(rushPoint, new Vector3(transform.position.x, 0, transform.position.z)) <= 0.1f)
            {
                rushAttackTrigger.SetActive(false);
                animator.SetBool("isRush", false);
                rushAttackTrigger.SetActive(false);
                navMeshAgent.ResetPath();
                navMeshAgent.speed = 0;
                navMeshAgent.acceleration = 8f;

                break;
            }
            else if (currentTime > 3)
            {
                rushAttackTrigger.SetActive(false);
                animator.SetBool("isRush", false);
                rushAttackTrigger.SetActive(false);
                navMeshAgent.ResetPath();
                navMeshAgent.speed = 0;
                navMeshAgent.acceleration = 8f;

                break;
            }

            yield return null;

            currentTime += Time.deltaTime;
        }

        yield return new WaitForSeconds(1f);


        isLook = true;

        rushAttackTrigger.SetActive(false); // animator가 더 늦게 끝나는것을 대비
        StartCoroutine("Think");
    }

    private IEnumerator Punch()
    {
        float currentTime = 0;
        navMeshAgent.speed = runSpeed;

        while (true)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            currentTime += Time.deltaTime;

            if (distance <= 3f)
            {
                navMeshAgent.ResetPath();
                navMeshAgent.speed = 0;
                navMeshAgent.velocity = Vector3.zero;
                isRun = false;
                animator.SetTrigger("onPunch");
                isLook = false;
                break;
            }
            else if (!isRun)
            {
                isRun = true;
                animator.SetTrigger("onRun");
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

        yield return new WaitForSeconds(1.5f);

        isLook = true;

        StartCoroutine("Think");
    }

    private IEnumerator Shout()
    {
        float currentTime = 0;
        navMeshAgent.speed = runSpeed;

        while (true)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            currentTime += Time.deltaTime;

            if (distance <= 6f)
            {
                navMeshAgent.ResetPath();
                navMeshAgent.speed = 0;
                navMeshAgent.velocity = Vector3.zero;

                if (isRun)
                {
                    isRun = false;
                    animator.SetTrigger("onIdle");
                }
                isLook = false;
                break;
            }
            else if (!isRun)
            {
                isRun = true;
                animator.SetTrigger("onRun");
            }

            else if (currentTime >= pursuitLimitTime)
            {
                navMeshAgent.ResetPath();
                navMeshAgent.speed = 0;
                animator.SetTrigger("onIdle");
                isRun = false;

                StartCoroutine("Think");
                yield break;
            }
            navMeshAgent.SetDestination(target.position);

            yield return null;
        }

        skin.materials[0].color = Color.blue;

        yield return new WaitForSeconds(1f);

        animator.SetTrigger("onShout");

        yield return new WaitForSeconds(0.5f);

        skin.materials[0].color = Color.white;

        ShoutObj.SetActive(true);

        yield return new WaitForSeconds(3f);

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

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isLook = false;
            isDie = true;
            StopAllCoroutines();
            navMeshAgent.ResetPath();
            navMeshAgent.velocity = Vector3.zero;
            animator.SetTrigger("onDie");

            StartCoroutine("OnDie");
        }
    }

    private IEnumerator OnDie()
    {
        GameManager.isBoss = false;

        yield return new WaitForSeconds(3f);
        MeleeEvent.cnt--;
        gameObject.SetActive(false);
    }


    private void OnDrawGizmos() // Debug Range View
    {
        if (navMeshAgent == null) return;

        Gizmos.color = Color.blue; // Melee Range
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }

}
