using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Veneer : MonoBehaviour
{
    public bool isIce;
    public float damage = 10;

    private bool isSlow;
    private float damageRate = 1.8f;
    private float damageTime = 0;

    private MemoryPool memoryPool;
    private ParticleSystem ps;
    private PlayerController playerController;

    private void Awake()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnDisable()
    {
        if (isSlow)
        {
            playerController.SpeedReset();
            isSlow = false;
        }
        damageTime = 0;
    }

    private void Update()
    {
        damageTime += Time.deltaTime;
    }

    public void SetUp(MemoryPool memoryPool)
    {
        this.memoryPool = memoryPool;
        StartCoroutine("OnVeneer");
    }

    private IEnumerator OnVeneer()
    {
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            var main = ps.main;
            main.loop = true;
        }

        float currentTime = 0f;
        float lifeTime = 20f;

        while (true)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lifeTime)
            {
                foreach(ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
                {
                    var main = ps.main;
                    main.loop = false;
                }

                StartCoroutine("OffVeneer");
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator OffVeneer()
    {
        while (true)
        {
            if (ps.isStopped)
            {
                memoryPool.DeactivatePoolItem(gameObject);
                yield break;
            }
            yield return null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isIce && !isSlow)
            {
                playerController.SpeedDown();
                isSlow = true;
            }
            if(damageTime >= damageRate)
            {
                playerController.TakeDamage((int)damage);
                damageTime = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isSlow)
            {
                playerController.SpeedReset();
                isSlow = false;
            }
        }
    }
}