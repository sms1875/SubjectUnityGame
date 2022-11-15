using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MidBoss3 : MonoBehaviour
{
    public bool isDead;
    public AudioClip dieClip;

    public float maxHealth;
    public float health;

    public GameObject destroyEffect;
    public Material destroyMat;
    protected NavMeshAgent nav;
    protected Animator anim;
    protected AudioSource audio;

    [Header("HPbarUI")]
    public GameObject healthBarBackground;
    public Slider healthBarSliderFill;

    private bool isHealthUIActive;
    private float hpBarTime = 0;

    public void TakeDamage(float damage)
    {
        if (health <= 0)
        {
            return;
        }

        health -= damage * 0.5f;

        healthBarSliderFill.maxValue = maxHealth;
        healthBarSliderFill.value = health;
        hpBarTime = 0;
        if (!isHealthUIActive)
        {
            isHealthUIActive = true;
            StartCoroutine(WaitCoroutine());
        }

        if (health <= 0)
        {
            isDead = true;
            audio.Stop();
            audio.clip = dieClip;
            audio.Play();
            nav.ResetPath();

            health = 0;

            destroyEffect.SetActive(true);

            anim.SetTrigger("OnDie");

            GetComponentInChildren<SkinnedMeshRenderer>().material = destroyMat;
            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
            {
                renderer.material = destroyMat;
            }
            RobotBoss.cnt--;
            Invoke("ActiveFalse", 3f);
        }
    }

    IEnumerator WaitCoroutine()
    {
        healthBarBackground.SetActive(true);
        while (true)
        {
            if (hpBarTime >= 3f)
            {
                break;
            }
            hpBarTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("false");
        healthBarBackground.SetActive(false);
        isHealthUIActive = false;
    }

    private void ActiveFalse()
    {
        gameObject.SetActive(false);
    }

    public void OnStepSound()
    {
        if (!audio.isPlaying)
        {
            audio.Play();
        }
    }
}
