using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArController : MonoBehaviour
{
    [SerializeField]
    private int damage = 2;
    [SerializeField]
    private int numberOfBullet =  30;
    [SerializeField]
    private float AttackDistance = 100f;
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform endPoint;
    [SerializeField]
    private GameObject muzzleEffect;
    [SerializeField]
    private GameObject sound;

    private Vector3 targetPosition;

    private EnemyAnimatorController enemyAnimatorController;
    private LineRenderer lineRenderer;
    private ImpactMemoryPool impactMemoryPool;
    private LayerMask layerMask;
    private void Awake()
    {
        enemyAnimatorController = GetComponentInParent<EnemyAnimatorController>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
        layerMask = LayerMask.GetMask("Enemy");
    }
    public void Shoot()
    {
        numberOfBullet--;

        muzzleEffect.SetActive(true);
        GameObject clone = Instantiate(sound, transform.position, transform.rotation);

        Vector3 direction = (targetPosition - startPoint.position).normalized;
        Ray ray = new Ray(startPoint.position, direction);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, AttackDistance, ~layerMask))
        {
            if (hit.transform.CompareTag("Player"))
            {
                hit.transform.GetComponent<PlayerController>().TakeDamage(damage);
            }
            else
            {
                impactMemoryPool.SpawnImpact(hit);
            }
            impactMemoryPool.SpawnImpact(hit); //임펙트 생성 및 데미지 입히는 부분
        }

        if (numberOfBullet == 0)
        {
            if (enemyAnimatorController)
            {
                enemyAnimatorController.IsReload = true;
                StartCoroutine("Reload");
            }
        }
    }

    private  IEnumerator Reload()
    {
        yield return new WaitForSeconds(0.5f);

        if (GetComponentInParent<EnemyFSM>().isDead)
        {
            yield break;
        }
        enemyAnimatorController.OnReload();

        yield return new WaitForSeconds(0.5f);

        numberOfBullet = 30;

        enemyAnimatorController.IsReload = false;
    }

    public void BulletLine(Transform targetTf)
    {    
        StartCoroutine("SetBulletLine", targetTf);
    }

    private IEnumerator SetBulletLine(Transform targetTf)
    {
        float originTime = Time.time;
        float currentTime = 0f;
        float limitTime = 0.5f;

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.red;
        lineRenderer.enabled = true;

        this.targetPosition = new Vector3(targetTf.position.x, targetTf.position.y + 0.1f, targetTf.position.z);

        while (true)
        {
            currentTime = Time.time - originTime;
            lineRenderer.SetPosition(0, startPoint.position);
            lineRenderer.SetPosition(1, targetPosition);

            if (limitTime < currentTime) break;

            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        lineRenderer.enabled = false;
    }
}