using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charging : MonoBehaviour
{
    public GameObject lazerSphere;
    public GameObject explosionEffect;
    public GameObject beam;
    public float chargeTime = 2.2f;
    public float beamTime = 2f;

    private void OnEnable()
    {
        lazerSphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        StartCoroutine("OnCharge");

        Invoke("OnAutoDisable", chargeTime + beamTime);
    }

    private IEnumerator OnCharge()
    {
        float currentTime = 0f;
        int index = 0;

        while (chargeTime >= currentTime)
        {
            lazerSphere.transform.localScale += new Vector3(0.0015f, 0.0015f, 0.0015f);

            
            currentTime += Time.deltaTime;
            index++;

            yield return null;
        }

        explosionEffect.SetActive(true);
        lazerSphere.SetActive(false);
        beam.SetActive(true);
    }

    private void OnDisable()
    {
        explosionEffect.SetActive(false);
        lazerSphere.SetActive(true);
        beam.SetActive(false);
    }

    private void OnAutoDisable()
    {
        gameObject.SetActive(false);
    }
}
