using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RockProjectile : MonoBehaviour
{
    public float damage = 30f;

    private bool isStop = true;
    private bool isHitted;
    public bool isEnter;

    public AudioClip hitClip;
    public AudioClip crashClip;

    private Rigidbody rigid;
    private NavMeshObstacle obstacle;
    private AudioSource audio;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        obstacle = GetComponent<NavMeshObstacle>();
        audio = GetComponent<AudioSource>();
    }

    public void SetUp(Transform from, Transform to)
    {
        isStop = false;
        rigid.isKinematic = false;
        float distance = Vector3.Distance(from.position, to.position);
        rigid.velocity = from.forward * (distance + 30);
        rigid.AddTorque(transform.right * 200, ForceMode.Impulse);
        StartCoroutine("AutoReset");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.name);
        if (collision.transform.CompareTag("Player") && !isStop && !isHitted)
        {
            isHitted = true;
            audio.Stop();
            audio.clip = hitClip;
            audio.Play();
            collision.transform.GetComponent<PlayerController>().OnStun(2);
            collision.transform.GetComponent<PlayerController>().TakeDamage((int)damage);
        }
        if ((collision.transform.CompareTag("Floor") || collision.transform.CompareTag("Wall")) && !isStop && !audio.isPlaying)
        {
            audio.clip = crashClip;
            audio.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MidBoss2"))
        {
            isEnter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MidBoss2"))
        {
            isEnter = false;
        }
    }

    private IEnumerator AutoReset()
    {
        while (true)
        {
            if(rigid.velocity == Vector3.zero)
            {
                rigid.isKinematic = true;
                obstacle.enabled = true;
                isHitted = false;
                isStop = true;
                gameObject.layer = LayerMask.NameToLayer("InterActive");
                break;
            }
            yield return null;
        }
    }
}
