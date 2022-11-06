using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{
    [Header("Audio Clip")]
    [SerializeField]
    private AudioClip walkClip;
    public float walkVolume = 1f;

    [SerializeField]
    private AudioClip pursuitClip;
    public float pursuitVolume = 1f;

    [SerializeField]
    private AudioClip attackClip;
    public float attackVolume = 1f;

    [SerializeField]
    private AudioClip hitClip;
    public float hitVolume = 1f;

    [SerializeField]
    private AudioClip dieClip;
    public float dieVolume = 1f;

    [SerializeField]
    private AudioClip shoutClip;
    public float shoutVolume = 1f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponentInParent<AudioSource>();
    }

    public void OnStopSound()
    {
        audioSource.Stop();
    }

    public void OnWalkSound()
    {
        audioSource.Stop();
        audioSource.clip = walkClip;
        audioSource.volume = walkVolume;
        audioSource.Play();
    }

    public void OnPurSuitSound()
    {
        audioSource.Stop();
        audioSource.clip = pursuitClip;
        audioSource.volume = pursuitVolume;
        audioSource.Play();
    }

    public void OnAttackSound()
    {
        audioSource.Stop();
        audioSource.clip = attackClip;
        audioSource.volume = attackVolume;
        audioSource.Play();
    }

    public void OnHitSound()
    {
        audioSource.Stop();
        audioSource.clip = hitClip;
        audioSource.volume = hitVolume;
        audioSource.Play();
    }

    public void OnDieSound()
    {
        audioSource.Stop();
        audioSource.clip = dieClip;
        audioSource.volume = dieVolume;
        audioSource.Play();
    }

    public void OnShoutSound()
    {
        audioSource.Stop();
        audioSource.clip = shoutClip;
        audioSource.volume = shoutVolume;
        audioSource.Play();
    }
}