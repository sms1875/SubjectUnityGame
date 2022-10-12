using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidBoss2_AudioController : MonoBehaviour
{
    public AudioClip stepClip;
    public AudioClip rushStepClip;
    public AudioClip punchClip;
    public AudioClip shoutClip;
    public AudioClip throwClip;
    public AudioClip crashClip;
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

    public void OnRushStepSound()
    {
        audio.Stop();
        audio.clip = rushStepClip;
        audio.Play();
    }

    public void OnPunchSound()
    {
        audio.Stop();
        audio.clip = punchClip;
        audio.Play();
    }

    public void OnShoutSound()
    {
        audio.Stop();
        audio.clip = shoutClip;
        audio.Play();
    }

    public void OnThrowSound()
    {
        audio.Stop();
        audio.clip = throwClip;
        audio.Play();
    }

    public void OnCrashSound()
    {
        audio.Stop();
        audio.clip = crashClip;
        audio.Play();
    }

    public void OnDieSound()
    {
        audio.Stop();
        audio.clip = dieClip;
        audio.Play();
    }
}
