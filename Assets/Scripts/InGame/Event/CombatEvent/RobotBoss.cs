using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBoss : CombatEvent
{
    public static int cnt;

    private void Awake()
    {
        cnt = 3;
    }

    private void Update()
    {
        if(cnt <= 0)
        {
            Clear();
        }
    }
}
