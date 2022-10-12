using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneController : MonoBehaviour
{
    [SerializeField]
    private float maxHealthPoint = 50f;
    [SerializeField]
    private float healthPoint = 50f;
    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private float moveDirection = 1f;

    [SerializeField]
    private GameObject effectObject;
    [SerializeField]
    private GameObject droneObject;

    [SerializeField]
    private AudioClip moveClip;
    [SerializeField]
    private AudioClip destroyClip;

    private AudioSource audioSource;

    private LayerMask layerMask;

    private bool isDie = false;
    private bool isSpin = false;

    private float lifeTime = 0f;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Wall");

        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GameManager.enemyCurrentNum++;
        audioSource.clip = moveClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void Update()
    {
        if (isDie == true) return;
        if (isSpin == true) return;

        lifeTime += Time.deltaTime;

        if (lifeTime >= 20f) DeActive();

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f, layerMask))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.transform.CompareTag("Wall"))
            {
                isSpin = true;
                StartCoroutine("Spin");
            }
        }

        Vector3 destination = transform.position + transform.forward;
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        healthPoint -= damage;

        if(healthPoint <= 0)
        {
            isDie = true;

            healthPoint = 0;

            audioSource.Stop();
            audioSource.clip = destroyClip;
            audioSource.Play();
            audioSource.loop = false;

            effectObject.SetActive(true);
            GameManager.enemyKillNum++;
            droneObject.SetActive(false);

            Invoke("DeActive", 1.5f);
        }
    }

    private void DeActive()
    {
        GameManager.enemyCurrentNum--;
        GetComponentInParent<SpawnerController>().OnDeactivateItem(gameObject);
    }

    private IEnumerator Spin()
    {
        Vector3 direction = (-transform.right * moveDirection).normalized;

        while (true)
        {
            if (Vector3.Angle(transform.forward, direction) < 1f)
            {
                isSpin = false;
                yield break;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDirection * -transform.right), 3f);
            yield return null;
        }
    }
}