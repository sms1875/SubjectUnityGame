using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackTrigger : MonoBehaviour
{
    public float disableTime = 0.1f;
    public float damage = 1f;
    public bool isRushAttack;

    public Animator animator;

    private void OnEnable()
    {
        if (isRushAttack == false)
        {
            StartCoroutine("AutoDisable");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isRushAttack == false)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().TakeDamage((int)damage);
                gameObject.SetActive(false);
            }
        }

        else if(isRushAttack == true)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().TakeDamage((int)damage);
                animator.SetBool("isRush", false);
                gameObject.SetActive(false);
            }
            else if (other.CompareTag("Wall") || other.CompareTag("Pillar"))
            {
                animator.SetBool("isStunned", true);
                animator.SetBool("isRush", false);
                gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(disableTime);
        if (gameObject.activeSelf == true)
        {
            this.gameObject.SetActive(false);
        }
    }
}
