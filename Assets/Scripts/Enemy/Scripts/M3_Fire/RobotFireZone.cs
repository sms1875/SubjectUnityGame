using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFireZone : MonoBehaviour
{
    public float damage = 10;
    public float maxRadius = 3;
    private bool isHitted;

    private void OnEnable()
    {
        isHitted = false;
        StartCoroutine("Control");
    }

    private IEnumerator Control()
    {
        yield return new WaitForSeconds(1.8f);

        StartCoroutine("UpScale");
    }

    private IEnumerator UpScale()
    {
        while (true)
        {
            transform.localScale += new Vector3(0.0025f, 0.0025f, 0.0025f);
            if (transform.localScale.x >= maxRadius)
            {
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isHitted)
        {
            isHitted = true;
            other.transform.GetComponent<PlayerController>().TakeDamage((int)damage);
        }
        if (other.CompareTag("Enemy") && other.GetComponent<EnemyFSM>().ai_Type == AI_Type.Obstacle)
        {
            other.GetComponent<EnemyFSM>().TakeDamage((int)damage);
        }
    }
}
