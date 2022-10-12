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
            StartCoroutine("OnDamage");
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

    private IEnumerator OnDamage()
    {
        isOnDamage = true;
        while (true)
        {
            Debug.Log("플레이어가" + damage + "의 피해를 입음");

            yield return new WaitForSeconds(damageRate);
        }
    }
}
