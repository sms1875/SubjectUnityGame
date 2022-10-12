using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid_Spark : MonoBehaviour
{
    public float damage = 2f;


    private float durationTime;
    private float currentTime = 0;

    private void Awake()
    {
        var main = GetComponent<ParticleSystem>().main;
        durationTime = main.duration;
    }

    private void OnEnable()
    {
        currentTime = 0;
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage((int)damage);
        }
        else if (other.CompareTag("Pillar"))
        {
            other.GetComponent<Pillar>().SetUp(durationTime - currentTime);

        }
    }
}
