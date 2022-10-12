using System.Collections.Generic;
using UnityEngine;

public class MemoryPool
{
    private class PoolItem
    {
        public bool isActive; // "gameObject"�� Ȱ��ȭ/��Ȱ��ȭ
        public GameObject gameObject; // ȭ�鿡 ���̴� ���� ���� ������Ʈ
    }

    private int increaseCount = 100; // ������Ʈ�� ������ �� Instantiate()�� �߰� �����Ǵ� ������Ʈ
    private int maxCount; // ���� ����Ʈ�� ��ϵǾ� �ִ� ������Ʈ ����
    private int activeCount; // ���� ���ӿ� ���ǰ� �ִ�(Ȱ��ȭ) ������Ʈ ����

    private GameObject poolObject; // ������Ʈ Ǯ������ �����ϴ� ���� ������Ʈ ������
    private List<PoolItem> poolItemList; // �����Ǵ� ��� ������Ʈ�� �����ϴ� ����Ʈ

    private bool isDontDestroyOnLoad;

    public int MaxCount => maxCount; // �ܺο��� ���� ����Ʈ�� ��ϵǾ��ִ� ������Ʈ ���� Ȯ���� ���� ������Ƽ
    public int ActiveCount => activeCount; // �ܺο��� ���� Ȱ��ȭ �Ǿ��ִ� ������Ʈ ���� Ȯ���� ���� ������Ƽ

    public MemoryPool(GameObject poolObject, bool isDontDestroyOnLoad = true, int increaseCount = 100)
    {
        this.isDontDestroyOnLoad = isDontDestroyOnLoad;
        this.increaseCount = increaseCount;
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObject;
        poolItemList = new List<PoolItem>();

        InstantiateObjects();
    }

    //incraseCount ������ ������Ʈ�� ����
    public void InstantiateObjects()
    {
        maxCount += increaseCount;

        for (int i = 0; i < increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();
            poolItem.isActive = false;
            poolItem.gameObject = GameObject.Instantiate(poolObject);
            if (isDontDestroyOnLoad)
            {
                GameObject.DontDestroyOnLoad(poolItem.gameObject); // �� �̵��� ����Ʈ �޸�Ǯ ������� �ʰ� �� Han
            }
            poolItem.gameObject.SetActive(false);
            poolItemList.Add(poolItem);
        }
    }

    // ���� ��������(Ȱ��/��Ȱ��) ��� ������Ʈ ����
    public void DestroyObjects()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }

        poolItemList.Clear();
    }

    // poolItemList�� ����Ǿ� �ִ� ������Ʈ�� Ȱ��ȭ�ؼ� ���
    // ��� ������Ʈ�� ���� ������̸� InstantiateObjects()�� �߰� ����
    public GameObject ActivatePoolItem()
    {
        if (poolItemList == null) return null;

        // ���� �����ؼ� �����ϴ� ��� ������Ʈ ������ ���� Ȱ��ȭ ������ ������Ʈ ���� ��
        // ��� ������Ʈ�� Ȱ��ȭ �����̸� �� ������Ʈ �ʿ�
        if (maxCount == activeCount)
        {
            InstantiateObjects();
        }

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
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

    // ���� ����� �Ϸ�� ������Ʈ�� ��Ȱ��ȭ ���·� ����
    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
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

    // ���ӿ� ���� ��� ������Ʈ�� ��Ȱ��ȭ ���·� ����
    public void DeactivateAllPoolItems()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
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
