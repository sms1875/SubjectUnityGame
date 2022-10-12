using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEvent : MonoBehaviour
{
    public static int cnt;
    private bool isClear;

    private void Update()
    {
        Debug.Log(cnt);
        if (isClear)
        {
            return;
        }
        if (cnt <= 0)
        {
            isClear = true;
        }
    }

    private void Clear()
    {
        Debug.Log("Clear");
    }
}
