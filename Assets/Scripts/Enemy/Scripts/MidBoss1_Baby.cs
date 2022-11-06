using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MidBoss1_Baby : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float runSpeed;

    public GameObject attackTrigger;

    private bool isRun;

    private GameObject targetObj;
    private Transform target;
    private Animator animator;
    private NavMeshAgent navMeshAgent;


    private void Awake()
    {
        targetObj = GameObject.FindGameObjectWithTag("Player");
        target = targetObj.transform;

        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        StartCoroutine("Attack");
    }

    private void Update()
    {
        LookTarget();
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

    private IEnumerator Attack()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);

        while (true)
        {
            distance = Vector3.Distance(target.transform.position, transform.position);

            Vector3 to = new Vector3(target.position.x, 0f, target.position.z);
            Vector3 from = new Vector3(transform.position.x, 0f, transform.position.z);

            Vector3 direction = (to - from).normalized;
            float angle = Vector3.Angle(direction, transform.forward);

            if (distance <= 3.5f)
            {
                isRun = false;
                navMeshAgent.speed = 0;
                navMeshAgent.ResetPath();
                navMeshAgent.velocity = Vector3.zero;
                animator.SetTrigger("onAttack");

                yield return new WaitForSeconds(0.35f);

                attackTrigger.SetActive(true);

                yield return new WaitForSeconds(1f);

                continue;
            }
            else if (!isRun)
            {
                isRun = true;
                navMeshAgent.speed = runSpeed;
                animator.SetTrigger("onRun");
            }
            navMeshAgent.SetDestination(target.position);
            yield return null;
        }

    }

    public void TakeDamage(float damage) // 플레이어 공격 trigger에 들어갈 시, 발동
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            StopAllCoroutines();
            animator.SetTrigger("onDie");

            StartCoroutine("OnDie");
        }
    }

    public IEnumerator OnDie()
    {
        navMeshAgent.speed = 0;
        navMeshAgent.ResetPath();

        yield return new WaitForSeconds(3f);

        gameObject.SetActive(false);
    }
}
