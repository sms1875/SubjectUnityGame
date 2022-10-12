using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerGroup : MonoBehaviour
{
    [Header("TotalNumOfEnemy")]
    public int min = 1;
    public int max = 3;

    private bool isSet;

    private int[] eachNumOfEnemy;

    public int[] EachNumOfEnemy => eachNumOfEnemy;

    public void ReSet()
    {
        eachNumOfEnemy = new int[transform.childCount];
        isSet = true;
    }

    public void SetUp()
    {
        int totalEnemyNum = Random.Range(min, max + 1);

        int numOfChild = transform.childCount;
        eachNumOfEnemy = new int[numOfChild];

        int cnt = 0;
        while (true)
        {
            for (int i = 0; i < eachNumOfEnemy.Length; i++)
            {
                int num = Random.Range(0, totalEnemyNum + 1 - cnt);
                eachNumOfEnemy[i] += num;
                cnt += num;
            }
            if (cnt >= totalEnemyNum) break;
        }

        for (int i = 0; i < numOfChild; i++)
        {
            SpawnerController spawnerController = transform.GetChild(i).GetComponent<SpawnerController>();
            spawnerController.NumberOfEnemy = eachNumOfEnemy[i];
            spawnerController.OneTimeSpawnNumber = eachNumOfEnemy[i];
            spawnerController.SetUp();
        }

        isSet = true;
    }

    private void LateUpdate()
    {
        if (!isSet) return;
        int cnt;
        for (int i = 0; i < transform.childCount; i++)
        {
            cnt = 0;
            Transform spawner = transform.GetChild(i);
            for (int j = 0; j < spawner.childCount; j++)
            {
                if (spawner.GetChild(j).gameObject.activeSelf) cnt++;
            }
            eachNumOfEnemy[i] = cnt;
        }
    }
}