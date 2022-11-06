using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidBoss4_AudioController : MonoBehaviour
{
    public AudioClip stepClip;
    public AudioClip punchClip;
    public AudioClip kickClip;
    public AudioClip rushClip;
    public AudioClip crashClip;
    public AudioClip jumpClip;
    public AudioClip landClip;
    public AudioClip breathClip1;
    public AudioClip breathClip2;
    public AudioClip breathClip3;
    public AudioClip breathChargeClip;
    public AudioClip kneeClip;
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
        audio.volume = 0.02f;
        audio.Play();
    }

    public void OnPunchSound()
    {
        audio.Stop();
        audio.clip = punchClip;
        audio.volume = 0.1f;
        audio.Play();
    }

    public void OnKickSound()
    {
        audio.Stop();
        audio.clip = kickClip;
        audio.volume = 0.07f;
        audio.Play();
    }

    public void OnRushSound()
    {
        audio.Stop();
        audio.clip = rushClip;
        audio.volume = 0.1f;
        audio.Play();
    }

    public void OnCrashSound()
    {
        if (audio.clip != crashClip)
        {
            audio.Stop();
            audio.clip = crashClip;
            audio.volume = 0.1f;
            audio.Play();
        }
    }

    public void OnJumpSound()
    {
        audio.Stop();
        audio.clip = jumpClip;
        audio.volume = 0.1f;
        audio.Play();
    }

    public void OnLandSound()
    {
        audio.Stop();
        audio.clip = landClip;
        audio.volume = 0.1f;
        audio.Play();
    }

    public void OnBreath1Sound()
    {
        audio.Stop();
        audio.clip = breathClip1;
        audio.volume = 0.1f;
        audio.Play();
    }

    public void OnBreath2Sound()
    {
        audio.Stop();
        audio.clip = breathClip2;
        audio.volume = 0.02f;
        audio.Play();
    }

    public void OnBreath3Sound()
    {
        if (audio.clip != breathClip3)
        {
            audio.Stop();
            audio.clip = breathClip3;
            audio.volume = 0.1f;
            audio.Play();
        }
        else
        {
            if (!audio.isPlaying)
            {
                audio.volume = 0.1f;
                audio.Play();
            }
        }
    }

    public void OnBreathChargeSound()
    {
        audio.Stop();
        audio.clip = breathChargeClip;
        audio.volume = 0.1f;
        audio.Play();
    }

    public void OnKneeSound()
    {
        audio.Stop();
        audio.clip = kneeClip;
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
