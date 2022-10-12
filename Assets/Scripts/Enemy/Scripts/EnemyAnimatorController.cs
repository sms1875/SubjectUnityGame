using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAnimatorController : MonoBehaviour
{
    [SerializeField]
    private GameObject attakcObject;
    [SerializeField]
    private Transform projectileSpawnPoint;
    [SerializeField]
    private bool isBulletLine = false;
    [SerializeField]
    private bool isTutorial = false;

    private bool isStopAiming = false;
    private bool isReload = false;

    private MemoryPool memoryPool;

    private Animator animator;
    private EnemyFSM enemyFSM;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyFSM = GetComponentInParent<EnemyFSM>();

        if (projectileSpawnPoint != null)
        {
            memoryPool = new MemoryPool(attakcObject, false, 10);
        }
    }

    public bool IsStopAiming => isStopAiming;

    public bool IsBuuletLine => isBulletLine;
    public bool IsReload
    {
        set
        {
            if (animator) isReload = value;
        }
        get
        {
            if (animator) return isReload;
            else return false;
        }
    }
    public float MoveSpeed
    {
        set
        {
            if (animator) animator.SetFloat("MoveState", value);
        }
        get
        {
            if (animator) return animator.GetFloat("MoveState");
            else return -1;
        }
    }

    public bool IsSpin
    {
        set
        {
            if (animator) animator.SetBool("isSpin", value);
        }
        get
        {
            if (animator) return animator.GetBool("isSpin");
            else return false;
        }
    }

    public bool IsBattle
    {
        set
        {
            if (animator) animator.SetBool("isBattle", value);
        }
        get
        {
            if (animator) return animator.GetBool("isBattle");
            else return false;
        }
    }

    public float HitState
    {
        set => animator.SetFloat("HitState", value);
        get => animator.GetFloat("HitState");
    }
    public GameObject GameObject 
    {
        set => attakcObject = value;
        get => attakcObject; 
    }

    public void OnAttack()
    {
        if (isBulletLine)
        {
            StopCoroutine("OnBulletLine");
            StartCoroutine("OnBulletLine");
        }
        else
        {
            animator.SetTrigger("OnAttack");
        }
    }
    
    public void Play(string stateName, int layer, float normalizedTime)
    {
        animator.Play(stateName, layer, normalizedTime);
    }

    public void OnDie()
    {
        animator.SetTrigger("OnDie");
    }

    public void OnTakeOff()
    {
        animator.SetTrigger("OnTakeOff");
    }
    public void OnLand()
    {
        animator.SetTrigger("OnLand");
    }

    public void OnAttackCollision()
    {
        if (attakcObject == null) return;

        attakcObject.SetActive(true);
    }

    public void OnSubject01_PorjectilePrefab()
    {
        if (attakcObject == null) return;

        Vector3 targetPosition = new Vector3(enemyFSM.Target.position.x, enemyFSM.Target.position.y + 0.3f, enemyFSM.Target.position.z);

        GameObject clone = memoryPool.ActivatePoolItem();
        clone.transform.position = projectileSpawnPoint.position;
        clone.transform.rotation = projectileSpawnPoint.rotation;

        if (isTutorial)
        {
            clone.GetComponent<Subject01_Porjectile>().SetUp(GetComponentInParent<TutorialEnemy>().Target.position, memoryPool);
            return;
        }
        clone.GetComponent<Subject01_Porjectile>().SetUp(targetPosition, memoryPool);
    }

    public void OnPrist_PorjectilePrefab()
    {
        if (attakcObject == null) return;
        GameObject clone = memoryPool.ActivatePoolItem();
        clone.transform.position = projectileSpawnPoint.position;
        clone.transform.rotation = projectileSpawnPoint.rotation;

        clone.GetComponent<Prist_Projectile>().SetUp(enemyFSM.Target, animator, memoryPool);
    }

    public void OnFlyDragon_PorjectilePrefab()
    {
        if (attakcObject == null) return;

        GameObject clone = memoryPool.ActivatePoolItem();
        clone.transform.position = projectileSpawnPoint.position;
        clone.transform.rotation = projectileSpawnPoint.rotation;
        if (isTutorial)
        {
            clone.GetComponent<Subject01_Porjectile>().SetUp(GetComponentInParent<TutorialEnemy>().Target.position, memoryPool);
            return;
        }
        clone.GetComponent<Subject01_Porjectile>().SetUp(enemyFSM.Target.position, memoryPool);
    }

    private IEnumerator OnBulletLine()
    {
        if (attakcObject == null) yield break;

        attakcObject.GetComponent<EnemyArController>().BulletLine(enemyFSM.Target);

        yield return new WaitForSeconds(0.5f);
        isStopAiming = true;
        animator.SetBool("isSpin", false);

        yield return new WaitForSeconds(0.5f);

        animator.SetTrigger("OnAttack");

        yield return new WaitForSeconds(1f);
        isStopAiming = false;
    }
    public void OnShootWithRay()
    {
        if (attakcObject == null) return;

        attakcObject.GetComponent<EnemyArController>().Shoot();
    }
    public void OnReload()
    {
        animator.SetTrigger("OnReload");
    }
    public void OnHit()
    {
        animator.SetTrigger("OnHit");
    }

    public bool EndState(string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >=1f) return true;
            return false;
        }
        else return false;
    }

    public float AnimationSpeed
    {
        set => animator.speed = value;
        get => animator.speed;
    }
}
