using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBoss : CombatEvent
{
    public static int cnt;
    public GameObject[] robots;

    private void Awake()
    {
        cnt = 3;

        robots[0].SetActive(false);
        robots[1].SetActive(false);
        Invoke("OnEnableFire", 25f);
        Invoke("OnEnableMissile", 45f);
    }

    private void Update()
    {
        if(cnt <= 0)
        {
            Clear();
        }
    }

    private void OnEnableFire()
    {
        robots[0].SetActive(true);
    }

    private void OnEnableMissile()
    {
        robots[1].SetActive(true);
    }
}
