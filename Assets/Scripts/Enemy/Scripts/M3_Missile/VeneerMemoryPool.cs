using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeneerMemoryPool : MonoBehaviour
{
    public GameObject veneer;

    private MemoryPool memoryPool;

    private void Awake()
    {
        memoryPool = new MemoryPool(veneer, false, 1);
    }

    public void SetVeneer()
    {
        GameObject veneerClone = memoryPool.ActivatePoolItem();
        veneerClone.transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        veneerClone.GetComponent<Veneer>().SetUp(memoryPool);
    }
}
