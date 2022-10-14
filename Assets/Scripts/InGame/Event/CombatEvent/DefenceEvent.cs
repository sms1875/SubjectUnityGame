using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceEvent : CombatEvent
{
    public float limitTime = 180;

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

            Clear();
        }
    }
}
