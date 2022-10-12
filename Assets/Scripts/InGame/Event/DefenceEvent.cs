using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceEvent : MonoBehaviour
{
    public float limitTime = 180;
    private SpawnerController[] spawnerControllers;

    private void Awake()
    {
        spawnerControllers = GetComponentsInChildren<SpawnerController>();
    }
    private void Update()
    {
        ClearCheck();
    }

    private void ClearCheck()
    {
        if (limitTime <= 0)
        {
            return;
        }
        limitTime -= Time.deltaTime;
        if (limitTime <= 0)
        {
            limitTime = 0;
            foreach (SpawnerController spawnerController in spawnerControllers)
            {
                spawnerController.StopAllCoroutines();
            }

            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.GetComponent<EnemyFSM>().TakeDamage(10000);
            }

            Debug.Log("Clear");
        }
    }
}
