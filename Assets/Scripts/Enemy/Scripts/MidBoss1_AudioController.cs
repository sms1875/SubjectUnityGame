using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidBoss1_AudioController : MonoBehaviour
{
    public AudioClip stepClip;
    public AudioClip reapClip;
    public AudioClip gasClip;
    public AudioClip jumpClip;
    public AudioClip landClip;
    public AudioClip shoutClip;
    public AudioClip acidClip;
    public AudioClip dieClip;
    public float stepVolume = 0.1f;
    public float reapVolume = 0.1f;

    private AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    public void OnStopSound()
    {
        audio.Stop();
    }

    public void OnStepSound()
    {
        audio.Stop();
        audio.clip = stepClip;
        audio.volume = stepVolume;
        audio.Play();
    }

    public void OnReapSound()
    {
        audio.Stop();
        audio.clip = reapClip;
        audio.volume = reapVolume;
        audio.Play();
    }

    public void OnGasSound()
    {
        audio.Stop();
        audio.clip = gasClip;
        audio.volume = 0.07f;
        audio.Play();
    }

    public void OnjumpSound()
    {
        audio.Stop();
        audio.clip = jumpClip;
        audio.volume = 0.1f;
        audio.Play();
    }

    public void OnlandSound()
    {
        audio.Stop();
        audio.clip = landClip;
        audio.volume = 0.1f;
        audio.Play();
    }

    public void OnShoutSound()
    {
        audio.Stop();
        audio.clip = shoutClip;
        audio.volume = 0.2f;
        audio.Play();
    }

    public void OnAcidSound()
    {
        audio.Stop();
        audio.clip = acidClip;
        audio.volume = 0.1f;
        audio.Play();
    }

    public void OnDieSound()
    {
        audio.Stop();
        audio.clip = dieClip;
        audio.volume = 0.1f;
        audio.Play();
    }
}
