using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prist_Projectile : MonoBehaviour
{
    [SerializeField]
    private float projectileDamage = 3f;
    [SerializeField]
    private GameObject magicCircle;
    [SerializeField]
    private GameObject fireSound;

    private ParticleSystem ps;
    private ParticleSystem.MinMaxCurve resetSize;

    private Transform target;
    private Animator animator;

    private MemoryPool memoryPool;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        resetSize =  ps.main.startSize;
    }

    private void OnDisable()
    {
        var main = ps.main;
        main.startSize = resetSize;
    }

    public void SetUp(Transform target, Animator animator, MemoryPool memoryPool)
    {
        this.memoryPool = memoryPool;
        this.target = target;
        this.animator = animator;
        StartCoroutine("Charge");
    }

    private IEnumerator Charge()
    {
        yield return new WaitForSeconds(1.5f);

        float currentTime = 0f;
        float chargeTime = 2f;

        while (true)
        {
            if (!animator.GetBool("isBattle"))
            {
                memoryPool.DeactivatePoolItem(gameObject);
            }
            currentTime += Time.deltaTime;
            if (currentTime >= chargeTime)
            {
                GameObject clone = Instantiate(fireSound);
                magicCircle.SetActive(false);
                StartCoroutine("Move");
                yield break;
            }

            var main = ps.main;
            var size = main.startSize;

            size.constantMin += 0.005f;
            size.constantMax += 0.005f;

            main.startSize = size;

            yield return null;
        }
    }

    private IEnumerator Move()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        float currentTime = 0f;
        float guidedTime = 3f;
        float lifeTime = 5f;

        while (true)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lifeTime)
            {
                memoryPool.DeactivatePoolItem(gameObject);
            }
            if (currentTime >= guidedTime)
            {
                transform.Translate(Vector3.forward * 10 * Time.deltaTime);
            }
            else
            {
                Vector3 targetUpPosition = new Vector3(target.position.x, target.position.y + 0.5f, target.position.z);
                transform.LookAt(targetUpPosition);
                transform.position = Vector3.MoveTowards(transform.position, targetUpPosition, 3 * Time.deltaTime);
            }

            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage((int)projectileDamage);
            memoryPool.DeactivatePoolItem(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            memoryPool.DeactivatePoolItem(gameObject);
        }
    }
}
