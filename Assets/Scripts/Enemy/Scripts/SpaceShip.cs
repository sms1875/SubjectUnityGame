using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    public float maxHealht;
    public float healht;

    public GameObject explosionEffect;

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

            Instantiate(explosionEffect, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }
}
