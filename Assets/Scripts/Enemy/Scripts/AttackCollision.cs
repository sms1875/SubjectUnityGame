using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    [SerializeField]
    private float disableTime = 0.1f;

    private EnemyStatus enemyStatus;

    private void Awake()
    {
        enemyStatus = GetComponentInParent<EnemyStatus>();
    }
    private void OnEnable()
    {
        StartCoroutine("AutoDisable");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.GetComponent<PlayerController>().TakeDamage((int)enemyStatus.AttackDamage);
        }
        if (other.CompareTag("SpaceShip"))
        {
            other.transform.GetComponent<SpaceShip>().TakeDamage((int)enemyStatus.AttackDamage);
        }
    }

    private IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(disableTime);

        this.gameObject.SetActive(false);
    }
}
