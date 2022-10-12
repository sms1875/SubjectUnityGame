using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBullet : MonoBehaviour
{
    public float damage = 1f;
    public float force = 5f;
    public float lifeTime = 4f;

    private float currentTime = 0f;

    private Rigidbody rigid;
    private TrailRenderer trail; 

    private MemoryPool memoryPool;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= lifeTime)
        {
            memoryPool.DeactivatePoolItem(gameObject);
        }
        else if (currentTime >= 0.1f)
        {
            trail.enabled = true;
        }
    }

    public void SetUp(MemoryPool memoryPool)
    {
        this.memoryPool = memoryPool;
        rigid.AddForce(transform.forward * force, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage((int)damage);
        }
        if (other.CompareTag("Enemy") && other.GetComponent<EnemyFSM>().ai_Type == AI_Type.Obstacle)
        {
            other.GetComponent<EnemyFSM>().TakeDamage(damage);
        }
        memoryPool.DeactivatePoolItem(gameObject);
    }

    private void OnDisable()
    {
        currentTime = 0;
        trail.enabled = false;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
}
