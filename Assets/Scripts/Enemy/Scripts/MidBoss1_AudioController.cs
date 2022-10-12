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
        audio.Play();
    }

    public void OnReapSound()
    {
        audio.Stop();
        audio.clip = reapClip;
        audio.Play();
    }

    public void OnGasSound()
    {
        audio.Stop();
        audio.clip = gasClip;
        audio.Play();
    }

    public void OnjumpSound()
    {
        audio.Stop();
        audio.clip = jumpClip;
        audio.Play();
    }

    public void OnlandSound()
    {
        audio.Stop();
        audio.clip = landClip;
        audio.Play();
    }

    public void OnShoutSound()
    {
        audio.Stop();
        audio.clip = shoutClip;
        audio.Play();
    }

    public void OnAcidSound()
    {
        audio.Stop();
        audio.clip = acidClip;
        audio.Play();
    }

    public void OnDieSound()
    {
        audio.Stop();
        audio.clip = dieClip;
        audio.Play();
    }
}
