using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFire : MonoBehaviour
{
    public float damage = 2f;

    private ParticleSystem ps;
    private AudioSource audio;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Invoke("StopSound", 5f);
    }

    private void StopSound()
    {
        audio.Stop();
    }

    private void Update()
    {
        if (ps.isStopped)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.GetComponent<PlayerController>().TakeDamage((int)damage);
        }
        if (other.CompareTag("Enemy") && other.transform.GetComponent<EnemyFSM>().ai_Type == AI_Type.Obstacle)
        {
            other.transform.GetComponent<EnemyFSM>().TakeDamage(0.02f);
        }
    }

    public void OnStop()
    {
        ps.Stop(true);
    }
}
