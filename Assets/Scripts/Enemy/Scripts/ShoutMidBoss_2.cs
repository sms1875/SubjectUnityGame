using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoutMidBoss_2 : MonoBehaviour
{
    public float damage;

    private bool isHitted;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        isHitted = false;
        StartCoroutine("Control");
    }

    private IEnumerator Control()
    {
        yield return new WaitForSeconds(0.3f);

        StartCoroutine("UpScale");

        yield return new WaitForSeconds(1f);

        StopCoroutine("UpScale");

        meshRenderer.enabled = false;

        transform.localScale = new Vector3(5, 5, 5);

        gameObject.SetActive(false);
    }

    private IEnumerator UpScale()
    {
        meshRenderer.enabled = true;
        while (true)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            transform.Rotate(Vector3.up * Time.deltaTime * 180);

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isHitted)
        {
            isHitted = true;
            other.GetComponent<PlayerController>().TakeDamage((int)damage);
        }
    }
}
