using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField]
    private int numberOfEnemy = 0;
    [SerializeField]
    private int oneTimeSpawnNumber = 0;
    [SerializeField]
    private float spawnTime = 0;
    [SerializeField]
    private bool isDrone = false;
    public bool isSurvive;
    private int totalNumberOfEnemy = 0;

    private GameObject enemyObj;

    private EnemyMemoryPool enemyMemoryPool;
    
    public int NumberOfEnemy
    {
        set => numberOfEnemy = value;
        get => numberOfEnemy;
    }

    public int OneTimeSpawnNumber
    {
        set => oneTimeSpawnNumber = value;
        get => oneTimeSpawnNumber;
    }

    private void Awake()
    {
        enemyObj = transform.GetChild(0).gameObject;
        enemyMemoryPool = new EnemyMemoryPool(enemyObj, transform);

        if (isDrone || isSurvive) StartCoroutine("SpawnEnemy");

    }

    public void SetUp()
    {
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        if (isDrone)
        {
            numberOfEnemy = Random.Range(1, 4);
            oneTimeSpawnNumber = 1;
            spawnTime = Random.Range(1, 5);

            yield return new WaitForSeconds(Random.Range(5, 15));
        }

        GameManager.enemyTotalNum += numberOfEnemy;
        totalNumberOfEnemy = 0;
        while (true)
        {
            for (int i = 0; i < oneTimeSpawnNumber; i++)
            {
                GameObject item = enemyMemoryPool.ActivatePoolItem(transform);
                item.transform.position = transform.position;
                totalNumberOfEnemy++;
                if (totalNumberOfEnemy == numberOfEnemy) yield break;
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void OnDeactivateItem(GameObject removeItem)
    {
        enemyMemoryPool.DeActivatePoolItem(removeItem);
    }

    public void ReSpawn()
    {
        StopCoroutine("SpawnEnemy");
        StartCoroutine("SpawnEnemy");
    }
}