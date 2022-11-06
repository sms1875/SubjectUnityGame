using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeneerProjectile : MonoBehaviour
{
    public GameObject projectile;
    public float projectileDamage;

    public GameObject veneer;
    public float veneerHoldingTime = 20f;

    private bool isGround;
    private bool isHit;

    private float currentTime = 0f;

    private Rigidbody rigid;
    private BoxCollider boxCollider;
    private SphereCollider sphereCollider;
    private BreathAndFireZoneTrigger BAF;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        sphereCollider = GetComponent<SphereCollider>();
        BAF = GetComponent<BreathAndFireZoneTrigger>();
    }

    private void OnEnable()
    {
        currentTime = 0f;
        rigid.isKinematic = false;

        float randomF_x = Random.Range(-10, 10);
        float randomF_y = Random.Range(-10, 10);
        float randomF_z = Random.Range(-10, 10);

        rigid.velocity = new Vector3(randomF_x, randomF_y, randomF_z);

        StartCoroutine("AutoDisable");
    }

    private IEnumerator AutoDisable()
    {
        while (true)
        {
            currentTime += Time.deltaTime;

            if (isGround == false && currentTime >= 10f)
            {
                transform.localPosition = new Vector3(0, 0, 0);
                gameObject.SetActive(false);
            }
            else if (isGround == true && currentTime >= veneerHoldingTime)
            {
                transform.localPosition = new Vector3(0, 0, 0);
                isGround = false;
                sphereCollider.enabled = false;
                projectile.SetActive(true);
                veneer.SetActive(false);
                boxCollider.isTrigger = true;
                boxCollider.size = new Vector3(0.8f, 0.8f, 0.8f);
                gameObject.SetActive(false);
            }


            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGround == false)
        {
            if (other.CompareTag("Player") && isHit == false)
            {
                isHit = true;
                other.GetComponent<PlayerController>().TakeDamage((int)projectileDamage);
            }
            else if (other.CompareTag("Floor"))
            {
                isGround = true;
                rigid.velocity = Vector3.zero;
                rigid.angularVelocity = Vector3.zero;
                rigid.isKinematic = true;

                currentTime = 0f;
                projectile.SetActive(false);

                boxCollider.isTrigger = false;
                boxCollider.size = new Vector3(0.1f, 0.1f, 0.1f);

                sphereCollider.enabled = true;
                transform.localPosition = new Vector3(transform.localPosition.x, -0.5f, transform.localPosition.z);
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                veneer.SetActive(true);
                BAF.enabled = true;
            }
            else if (other.CompareTag("Wall"))
            {
                boxCollider.isTrigger = false;

                Invoke("OnTrigger_True", 0.5f);
            }
        }
    }

    private void OnTrigger_True()
    {
        boxCollider.isTrigger = true;
    }
}