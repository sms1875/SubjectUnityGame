using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    public float maxHealht;
    private float healht;

    private void Awake()
    {
        healht = maxHealht;
    }

    public void TakeDamage(float damage)
    {
        if (healht <= 0)
        {
            return;
        }

        healht -= damage;

        if (healht <= 0)
        {
            healht = 0;
            gameObject.SetActive(false);
        }
    }
}
