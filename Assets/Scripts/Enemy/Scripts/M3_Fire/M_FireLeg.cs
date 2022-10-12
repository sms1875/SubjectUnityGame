using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class M_FireLeg : MidBoss3
{
    public M_FireTop top;
    public GameObject fireZone;
    public Material mat;

    private Transform target;
    private Transform viewTf;
    Color color = Color.white;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        top.target = target;
    }

    private void LateUpdate()
    {
        if (isDead)
        {
            top.isDead = true;
            fireZone.SetActive(false);
            return;
        }

        if (!top.isFireZone)
        {
            CalculateDistanceAndSetMove();
            FieldOfView();
        }
        if (top.isShoot && color.g > 0.5f && color.b > 0.5f)
        {
            color.g -= 0.027f * Time.deltaTime;
            color.b -= 0.027f * Time.deltaTime;
            mat.color = color;
        }
    }

    private void FieldOfView()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, 50, LayerMask.GetMask("Player"));
        int index = 0;

        while (index < colls.Length)
        {
            Transform targetTf = colls[index].transform;

            if (targetTf.CompareTag("Player"))
            {
                Vector3 direction = (targetTf.position - transform.position).normalized;
                float angle = Vector3.Angle(direction, transform.forward);

                if (angle <= 60 * 0.5f)
                {
                    Ray ray = new Ray(transform.position + transform.up, direction);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 50, ~LayerMask.GetMask("Enemy")))
                    {
                        viewTf = hit.transform;
                    }
                }
            }
            index++;
        }
    }

    private void CalculateDistanceAndSetMove()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance < 20 && !top.isShoot && top.isFireZoneReady)
        {
            anim.SetBool("IsMove", false);
            nav.ResetPath();
            top.isFireZone = true;
            StartCoroutine("OnFireZone");
        }
        else if (distance <= 35 && viewTf != null && viewTf.CompareTag("Player"))
        {
            anim.SetBool("IsMove", false);
            nav.ResetPath();
        }
        else
        {
            anim.SetBool("IsMove", true);
            nav.SetDestination(target.position);
        }
    }

    private IEnumerator OnFireZone()
    {
        yield return new WaitForSeconds(1f);

        fireZone.SetActive(true);

        yield return new WaitForSeconds(0.4f);

        while (true)
        {
            if (color.g <= 0.01 && color.b <= 0.01)
            {
                break;
            }

            color.g -= 0.3f * Time.deltaTime;
            color.b -= 0.3f * Time.deltaTime;

            mat.color = color;

            yield return null;
        }

        while (true)
        {
            top.OnFireZone();
            if (color.g >= 1 && color.b >= 1)
            {
                mat.color = Color.white;
                break;
            }

            color.g += 0.15f * Time.deltaTime;
            color.b += 0.15f * Time.deltaTime;

            mat.color = color;

            yield return null;
        }

        top.isFireZone = false;
    }

    private void OnEnable()
    {
        mat.color = Color.white;
    }
}
