using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject01_Porjectile : MonoBehaviour
{
    [SerializeField]
    private float projectileSpeed = 1f;
    [SerializeField]
    private float projectileDistance = 80f;
    [SerializeField]
    private float projectileDamage = 3f;
    [SerializeField]
    private GameObject collisionEffect;

    private MemoryPool memoryPool;
    private Vector3 moveDirection = Vector3.zero;

    public void SetUp(Vector3 targetPosition, MemoryPool memoryPool)
    {
        this.memoryPool = memoryPool;
        StartCoroutine("OnMove", targetPosition);
    }

    private IEnumerator OnMove(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        moveDirection = (targetPosition - startPosition).normalized;

        while (true)
        {
            transform.position += moveDirection * projectileSpeed * Time.deltaTime;

            if(Vector3.Distance(transform.position, startPosition) >= projectileDistance)
            {
                memoryPool.DeactivatePoolItem(gameObject);
                yield break;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage((int)projectileDamage);

            memoryPool.DeactivatePoolItem(gameObject);
        }
        if (other.CompareTag("SpaceShip"))
        {
            other.transform.GetComponent<SpaceShip>().TakeDamage(projectileDamage);
            memoryPool.DeactivatePoolItem(gameObject);
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Floor"))
        {
            Debug.Log("º®¿¡ ºÎµúÈû");
            //GameObject clone = Instantiate(collisionEffect, transform.position, transform.rotation);
            memoryPool.DeactivatePoolItem(gameObject);
        }
    }
}