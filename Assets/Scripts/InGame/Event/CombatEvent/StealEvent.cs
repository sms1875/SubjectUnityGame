using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealEvent : CombatEvent
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Clear();
        }
    }
}
