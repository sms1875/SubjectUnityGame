using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEvent : CombatEvent
{
    public static int cnt;

    private void Update()
    {
        Debug.Log(cnt);

        if (cnt <= 0)
        {
            Clear();
        }
    }
}
