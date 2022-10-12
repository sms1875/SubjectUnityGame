using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuidedMissile : MonoBehaviour
{
    public float damage = 5f;
    public float health = 100f;
    public GameObject destroyImpact;
    private NavMeshAgent nav;
    private Transform target;
    private Transform missile;
    private MemoryPool memoryPool;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        missile = transform.GetChild(0);
        nav = GetComponent<NavMeshAgent>();
    }

    public void SetUp(MemoryPool memoryPool, float y)
    {
        this.memoryPool = memoryPool;
        missile.localPosition = new Vector3(missile.localPosition.x, y - 1, missile.localPosition.z);
        missile.GetComponent<Missile>().GetMemoryPool(memoryPool, health, damage, destroyImpact);
        StartCoroutine("Move");
    }

    private IEnumerator Move()
    {
        bool isShake = true;

        float randX = 0;
        float randY = 0;
        float randZ = 0;

        while (true)
        {
            if (missile.localPosition.y >= 20)
            {
                break;
            }

            if (isShake)
            {
                isShake = false;

                randX = Random.Range(-0.02f, 0.02f);
                randY = Random.Range(-0.02f, 0.02f);
                randZ = Random.Range(-0.01f, 0.01f);

                missile.localPosition = new Vector3(missile.localPosition.x + randX, missile.localPosition.y + 0.05f + randY, missile.localPosition.z + randZ);
            }
            else
            {
                missile.localPosition = new Vector3(missile.localPosition.x - randX, missile.localPosition.y + 0.05f - randY, missile.localPosition.z - randZ);
            }
           
            yield return null;
        }
        float underY = 0.002f;
        while (true)
        {
            if (missile.localPosition.y < 6)
            {
                underY = 0;
            }
            if (isShake)
            {
                isShake = false;

                randX = Random.Range(-0.02f, 0.02f);
                randY = Random.Range(-0.02f, 0.02f);
                randZ = Random.Range(-0.01f, 0.01f);

                missile.localPosition = new Vector3(missile.localPosition.x + randX, missile.localPosition.y - underY + randY, missile.localPosition.z + randZ);
            }
            else
            {
                missile.localPosition = new Vector3(missile.localPosition.x - randX, missile.localPosition.y - underY - randY, missile.localPosition.z - randZ);
            }
            nav.SetDestination(target.position);

            missile.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.position - missile.position), 10 * Time.deltaTime) * Quaternion.Euler(new Vector3(90,0,0));

            float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= 15f)
            {
                nav.enabled = false;
                StartCoroutine("OffNavMesh", new Vector3(target.position.x, target.position.y + 3, target.position.z));
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator OffNavMesh(Vector3 targetPosition)
    {
        float currentTime = 0f;
        float lifeTime = 3f;
        while (true)
        {
            missile.rotation = Quaternion.LookRotation(targetPosition - missile.position) * Quaternion.Euler(new Vector3(90, 0, 0));
            float distance = Vector3.Distance(targetPosition, missile.position);
            if (distance <= 1f)
            {
                GameObject clone = Instantiate(destroyImpact, missile.position, transform.rotation);
                memoryPool.DeactivatePoolItem(gameObject);
            }
            
            currentTime += Time.deltaTime;
            if (currentTime >= lifeTime)
            {
                GameObject clone = Instantiate(destroyImpact, missile.position, transform.rotation);
                memoryPool.DeactivatePoolItem(gameObject);
            }

            missile.position = Vector3.MoveTowards(missile.position, targetPosition, 40 * Time.deltaTime);
            yield return null;
        }
    }

    private void OnDisable()
    {
        nav.enabled = true;
    }
}
