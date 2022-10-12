using System.Collections.Generic;
using UnityEngine;

public class EnemyMemoryPool
{
    private class PoolItem
    {
        public bool isActive;
        public GameObject gameObject;
    }

    private int increaseCount = 5;
    private int maxCount;
    private int activeCount;

    private GameObject poolObject;
    private List<PoolItem> poolItemList;

    public EnemyMemoryPool(GameObject poolObject, Transform spawnPointTf)
    {
        maxCount = 0;
        activeCount = 0;

        if (poolObject == null) return;
        if (spawnPointTf == null) return;

        this.poolObject = poolObject;
        poolItemList = new List<PoolItem>();

        InstantiateObject(spawnPointTf);
    }

    private void InstantiateObject(Transform transform)
    {
        maxCount += increaseCount;

        for(int i = 0; i < increaseCount; i++)
        {   
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false;
            poolItem.gameObject = GameObject.Instantiate(poolObject, transform);
            poolItem.gameObject.SetActive(false);

            poolItemList.Add(poolItem);
        }
    } 

    public void DestroyObject()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;

        for(int i =0; i < count; i++)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }
        poolItemList.Clear();
    }

    public GameObject ActivatePoolItem(Transform spawnPointTf)
    {
        if (poolItemList == null) return null;

        if (maxCount == activeCount) InstantiateObject(spawnPointTf);

        int count = poolItemList.Count;
        for(int i = 0; i < count; i++)
        {
            PoolItem poolItem = poolItemList[i];
            if (poolItem.isActive == false)
            {
                activeCount++;
                
                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;
            }
        }

        return null;

    }

    public void DeActivatePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null) return;

        int count = poolItemList.Count;
        for(int i=0; i < count; i++)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject == removeObject)
            {
                activeCount--;

                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);

                return;
            }
        }
    }

    public void DeActivateAllPoolItem()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for(int i = 0; i < count; i++)
        {
            PoolItem poolItem = poolItemList[i];
            if (poolItem.gameObject != null && poolItem.isActive == true)
            {
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }

        activeCount = 0;
    }
}

