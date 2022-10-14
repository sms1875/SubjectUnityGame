using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestBoss : CombatEvent
{
    public void BossDead()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Baby");
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.GetComponent<EnemyFSM>().TakeDamage(10000);
        }

        Clear();
    }
}
