using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private float damage;
    private float health;
    private GameObject destroyImpact;
    private MemoryPool memoryPool;

    private Vector3 startLocalPosition;
    private Vector3 startLocalRotation;

    public bool isVeneer;
    private bool isRecognizeEnemy;

    private VeneerMemoryPool veneerMemoryPool;

    private void Awake()
    {
        startLocalPosition = transform.localPosition;
        startLocalRotation = transform.localEulerAngles;
        if (isVeneer)
        {
            veneerMemoryPool = GetComponent<VeneerMemoryPool>();
        }
    }

    private void OnEnable()
    {
        Invoke("OnRecognizeEnemy", 3f);
    }

    private void OnRecognizeEnemy()
    {
        isRecognizeEnemy = true;
    }

    public void GetMemoryPool(MemoryPool memoryPool, float health, float damage, GameObject destroyImpact)
    {
        this.memoryPool = memoryPool;
        this.health = health;
        this.damage = damage;
        this.destroyImpact = destroyImpact;
    }

    public void TakeDamage(float damage)
    {
        if (health <= 0)
        {
            return;
        }

        health -= damage;

        if (health <= 0)
        {
            health = 0;

            StopAllCoroutines();

            GameObject clone = Instantiate(destroyImpact, transform.position, transform.rotation);
            memoryPool.DeactivatePoolItem(transform.parent.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isRecognizeEnemy)
        {
            if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Floor") || collision.transform.CompareTag("Player"))
            {
                if (isVeneer)
                {
                    veneerMemoryPool.SetVeneer();
                }

                GameObject clone = Instantiate(destroyImpact, transform.position, transform.rotation);
                memoryPool.DeactivatePoolItem(transform.parent.gameObject);
            }
        }
        else
        {
            if (collision.transform.CompareTag("Wall") || collision.transform.CompareTag("Floor") || collision.transform.CompareTag("Player") || collision.transform.CompareTag("Enemy"))
            {
                if (isVeneer)
                {
                    veneerMemoryPool.SetVeneer();
                }

                GameObject clone = Instantiate(destroyImpact, transform.position, transform.rotation);
                memoryPool.DeactivatePoolItem(transform.parent.gameObject);
            }
        }
    }

    private void OnDisable()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 5, Vector3.up, 0);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.CompareTag("Player"))
            {
                hit.transform.GetComponent<PlayerController>().TakeDamage((int)damage);
            }
            if (hit.transform.CompareTag("Enemy") && hit.transform.GetComponent<EnemyFSM>().ai_Type == AI_Type.Obstacle)
            {
                hit.transform.GetComponent<EnemyFSM>().TakeDamage(damage);
            }
            if (hit.transform.CompareTag("MidBoss3"))
            {
                hit.transform.GetComponent<MidBoss3>().TakeDamage(damage);
            }
        }
        transform.localPosition = startLocalPosition;
        transform.localRotation = Quaternion.Euler(startLocalRotation);

        isRecognizeEnemy = false;
    }
}
