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
            Debug.Log("�÷��̾ ��ݿ� ���� " + damage + "�� ���ظ� ����");
        }
    }
}
