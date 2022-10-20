using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class M_PistolLeg : MidBoss3
{
    public Vector3 destination;

    public M_PistolTop top;
    public GameObject[] turretGroups;

    private int patternCnt = 2;
    private int turretCnt = 0;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        top.destination = destination;

        turretGroups = GameObject.FindGameObjectsWithTag("Turret");
        foreach (GameObject turret in turretGroups)
        {
            turret.SetActive(false);
        }
    }

    private void OnEnable()
    {
        anim.SetTrigger("OnMove");
        nav.SetDestination(destination);
        Invoke("SetUp", 10f);
    }

    private void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            top.isDead = true;
        }
    }

    private void SetUp()
    {
        anim.SetTrigger("OnIdle");
        nav.ResetPath();
        StartCoroutine("Think");
    }

    private IEnumerator Think()
    {
        yield return new WaitForSeconds(0.5f);

        anim.SetTrigger("OnMove");
        nav.SetDestination(destination);

        float currentTime = 0;
        float patterTime = 7f;
        while (true)
        {
            float distance = Vector3.Distance(transform.position, destination);
            currentTime += Time.deltaTime;
            if (currentTime >= patterTime)
            {
                audio.Stop();
                int rand = Random.Range(0, 2);
                rand = 1;
                if (rand == 0)
                {
                    StartCoroutine("Pattern2");
                }
                else
                {
                    StartCoroutine("Pattern1");
                }
                yield break;
            }
            if(distance < 5f)
            {
                audio.Stop();
                StartCoroutine("Pattern3");
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator Pattern1()
    {
        yield return new WaitForSeconds(0.1f);

        top.isAimShot = true;
        anim.SetTrigger("OnIdle");
        nav.ResetPath();

        while (true)
        {
            if (top.isShoot)
            {
                yield return new WaitForSeconds(6.5f);

                top.isAimShot = false;
                top.isShoot = false;

                if (turretCnt > 0)
                {
                    patternCnt++;
                    StartCoroutine("Pattern3");
                }
                else
                {
                    StartCoroutine("Think");
                }
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator Pattern2()
    {
        yield return new WaitForSeconds(0.1f);

        top.isSpinShot = true;
        anim.SetTrigger("OnIdle");
        nav.ResetPath();

        while (true)
        {
            if (top.isShoot)
            {
                yield return new WaitForSeconds(16.5f);

                top.isSpinShot = false;
                top.isShoot = false;

                if (turretCnt > 0)
                {
                    patternCnt++;
                    StartCoroutine("Pattern3");
                }
                else
                {
                    StartCoroutine("Think");
                }
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator Pattern3()
    {
        if (turretCnt < turretGroups.Length && patternCnt >= 2)
        {
            yield return new WaitForSeconds(0.1f);

            patternCnt = 0;

            top.isTurret = true;
            anim.SetTrigger("OnIdle");
            nav.ResetPath();

            yield return new WaitForSeconds(2f);

            turretGroups[turretCnt++].SetActive(true);

            yield return new WaitForSeconds(2f);
            top.isTurret = false;
        }

        yield return new WaitForSeconds(2f);

        int rand = Random.Range(0, 3);
        if (rand == 0)
        {
            StartCoroutine("Pattern2");
        }
        else
        {
            StartCoroutine("Pattern1");
        }
    }
}
