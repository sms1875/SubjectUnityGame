using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidBoss1_Egg : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public bool isJustEgg;

    public Material[] destroyMat;

    public GameObject baby;
    public GameObject effect;

    public AudioClip readyClip;
    public AudioClip spawnClip;

    private MeshRenderer[] meshRenderers;
    private Material[] materials;
    private Light light;
    private AudioSource audio;

    private void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        materials = new Material[meshRenderers.Length];
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            materials[i] = meshRenderers[i].materials[0];
        }
        light = GetComponentInChildren<Light>();
        audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (!isJustEgg)
        {
            StartCoroutine("Up");
        }
    }


    private IEnumerator Up()
    {
        audio.clip = readyClip;
        audio.volume = 0.1f;
        audio.Play();
        float intensity = 0f;
        while (true)
        {
            intensity += 0.005f;
            if (intensity >= 2f)
            {
                StartCoroutine("Down");
                yield break;
            }
            else
            {
                light.intensity = intensity;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat("_Translucency", intensity);
                }
            }
            yield return null;
        }
    }

    private IEnumerator Down()
    {
        float intensity = 2f;
        while (true)
        {
            intensity -= 0.005f;
            if (intensity <= 0f)
            {
                StartCoroutine("Born");
                yield break;
            }
            else
            {
                light.intensity = intensity;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat("_Translucency", intensity);
                }
            }
            yield return null;
        }
    }

    private IEnumerator Born()
    {
        audio.Stop();
        audio.clip = spawnClip;
        audio.volume = 0.1f;
        float intensity = 0f;
        while (true)
        {
            intensity += 0.05f;
            if (intensity >= 4f)
            {
                audio.Play();
                effect.GetComponent<ParticleSystemRenderer>().material.SetFloat("_Translucency", intensity);
                effect.SetActive(true);
                yield return new WaitForSeconds(0.2f);
                baby.SetActive(true);
                gameObject.SetActive(false);
                yield break;
            }
            else
            {
                light.intensity = intensity;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat("_Translucency", intensity);
                }
            }
            yield return null;
        }
    }

    public void TakeDamage(float damage) // 플레이어 공격 trigger에 들어갈 시, 발동
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            StopAllCoroutines();

            StartCoroutine("OnDie");
        }
    }

    private IEnumerator OnDie()
    {
        audio.Stop();
        audio.clip = spawnClip;
        float intensity = 0f;
        light.intensity = intensity;
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_Translucency", intensity);
        }

        audio.Play();
        effect.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        gameObject.SetActive(false);
    }
}
