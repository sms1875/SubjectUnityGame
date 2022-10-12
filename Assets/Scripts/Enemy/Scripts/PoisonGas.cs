using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonGas : MonoBehaviour
{
    public float damage = 10f;
    public ParticleSystem particle;

    private BoxCollider trigger;
    private bool isPoisoning;

    private void Awake()
    {
        trigger = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        StartCoroutine("Control");
    }

    private IEnumerator Control()
    {
        yield return new WaitForSeconds(0.5f);

        trigger.enabled = true;

        while (true)
        {
            if (particle.isStopped && !isPoisoning)
            {
                trigger.enabled = false;
                gameObject.SetActive(false);
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPoisoning)
        {
            isPoisoning = true;
            StartCoroutine("Poisoning", other.GetComponent<PlayerController>());
        }
    }

    private IEnumerator Poisoning(PlayerController player)
    {
        for(int i = 0; i < 3; i++)
        {
            player.TakeDamage((int)damage);

            yield return new WaitForSeconds(1.5f);
        }
        isPoisoning = false;
    }
}