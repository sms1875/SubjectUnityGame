using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccupationEvent : CombatEvent
{
    public float stayTime = 30f;

    private bool isReady;
    private GameObject[] spawners;

    private void Awake()
    {
        spawners = new GameObject[transform.childCount - 1];
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject;
        }            
    }

    private IEnumerator Stay()
    {
        float curretTime = 0;
        while (true)
        {
            curretTime += Time.deltaTime;
            if (curretTime >= stayTime)
            {

                Clear();
                yield break;
            }
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine("Stay");
            if (!isReady)
            {
                isReady = true;
                foreach (GameObject spawner in spawners)
                {
                    spawner.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
        }
    }
}
