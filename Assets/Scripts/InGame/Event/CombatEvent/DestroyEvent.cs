using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEvent : CombatEvent
{
    private int length;
    private bool isReady;
    private GameObject[] targets;

    private void Awake()
    {
        length = transform.childCount;
        targets = new GameObject[transform.GetChild(length - 1).childCount];
        for(int i = 0; i < targets.Length; i++)
        {
            targets[i] = transform.GetChild(length - 1).GetChild(i).GetChild(0).gameObject;
        }
    }

    private void FixedUpdate()
    {
        if (isReady)
        {
            DestroyCheck();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            for (int i = 0; i < length; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            isReady = true;
        }
    }

    private void DestroyCheck()
    {
        for(int i = 0; i < targets.Length; i++)
        {
            if (targets[i].activeSelf)
            {
                return;
            }
        }

        Clear();
    }
}
