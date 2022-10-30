using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeneerMissile : MonoBehaviour
{
    public float damage = 5f;
    public float health = 100f;
    public GameObject destroyImpact;

    private Transform missile;
    private MemoryPool memoryPool;

    private void Awake()
    {
        missile = transform.GetChild(0);
    }

    public void SetUp(MemoryPool memoryPool, Vector3 point)
    {
        this.memoryPool = memoryPool;
        missile.GetComponent<Missile>().GetMemoryPool(memoryPool, health, damage, destroyImpact);
        StartCoroutine("Move", point);
    }

    private IEnumerator Move(Vector3 point)
    {
        int rand = Random.Range(0, 2);
        if(rand == 0)
        {
            rand = -1;
        }
        int rotateSpeed = 180 * rand;
        float acceleration = 1f;

        transform.LookAt(point);
        Vector3 halfPoint = Vector3.Lerp(transform.position, point, 0.5f);

        while (true)
        {
            missile.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
            missile.RotateAround(halfPoint, transform.right, (45 * acceleration) * Time.deltaTime);

            acceleration += 0.001f;

            yield return null;
        }
    }
}
