using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathAndFireZoneTrigger : MonoBehaviour
{
    public float damage = 10f;
    public float damageRate = 0.5f;

    private bool isOnDamage;

    private void OnEnable()
    {
        isOnDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !isOnDamage)
        {
            StartCoroutine("OnDamage", other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isOnDamage = false;
            StopCoroutine("OnDamage");
        }
    }

    private IEnumerator OnDamage(Transform tf)
    {
        isOnDamage = true;
        while (true)
        {
            tf.GetComponent<PlayerController>().TakeDamage((int)damage);

            yield return new WaitForSeconds(damageRate);
        }
    }
}
