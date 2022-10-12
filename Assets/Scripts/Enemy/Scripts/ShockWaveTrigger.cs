using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveTrigger : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 충격에 의해 " + damage + "의 피해를 입음");
        }
    }
}
