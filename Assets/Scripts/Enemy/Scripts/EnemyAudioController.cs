using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{
    [Header("Audio Clip")]
    [SerializeField]
    private AudioClip walkClip;
    [SerializeField]
    private AudioClip purSuitClip;
    [SerializeField]
    private AudioClip attackClip;
    [SerializeField]
    private AudioClip hitClip;
    [SerializeField]
    private AudioClip dieClip;
    [SerializeField]
    private AudioClip shoutClip;

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
        audioSource.volume = 0.01f;
        audioSource.Play();
    }

    public void OnPurSuitSound()
    {
        audioSource.Stop();
        audioSource.clip = purSuitClip;
        audioSource.volume = 0.05f;
        audioSource.Play();
    }

    public void OnAttackSound()
    {
        audioSource.Stop();
        audioSource.clip = attackClip;
        audioSource.volume = 0.1f;
        audioSource.Play();
    }

    public void OnHitSound()
    {
        audioSource.Stop();
        audioSource.clip = hitClip;
        audioSource.volume = 0.1f;
        audioSource.Play();
    }

    public void OnDieSound()
    {
        audioSource.Stop();
        audioSource.clip = dieClip;
        audioSource.volume = 0.1f;
        audioSource.Play();
    }

    public void OnShoutSound()
    {
        audioSource.Stop();
        audioSource.clip = shoutClip;
        audioSource.volume = 1f;
        audioSource.Play();
    }
}