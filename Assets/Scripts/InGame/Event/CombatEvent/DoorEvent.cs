using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEvent : MonoBehaviour
{
    public AudioClip openClip;
    public AudioClip closeClip;

    public Collider doorOpenTrigger;
    public Collider doorCloseTrigger;

    public DigitalRuby.RainMaker.BaseRainScript rain;

    float angle = 90;
    bool isOpen;

    AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        if (isOpen)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    private void Open()
    {
        if (angle <= -45)
        {
            return;
        }
        else
        {
            angle -= 45 * Time.deltaTime;
            transform.localEulerAngles = new Vector3(0, angle, 0);
        }
    }

    private void Close()
    {
        if (angle >= 90)
        {
            return;
        }
        else
        {
            angle += 135 * Time.deltaTime;
            transform.localEulerAngles = new Vector3(0, angle, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isOpen)
            {
                isOpen = false;
                audio.Stop();
                audio.clip = closeClip;
                audio.Play();
                doorOpenTrigger.enabled = true;
                doorCloseTrigger.enabled = false;
            }
            else
            {
                isOpen = true;
                audio.Stop();
                audio.clip = openClip;
                audio.Play();
                doorOpenTrigger.enabled = false;
                doorCloseTrigger.enabled = true;

                rain.RainIntensity = 0;
            }
        }
    }
}
