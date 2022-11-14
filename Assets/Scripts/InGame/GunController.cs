using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // ���� ����� ���� ������ �ִϸ��̼�.
    public Gun currentGun;
    private Animator weaponAnim;
    private Transform gunObj;

    private Vector3 cameraCenter;

    public ImpactMemoryPool impactMemoryPool; // ����Ʈ ��¿� �޸� ������Ʈ

    // ���� ����
    public static bool isActivate = true; //���� Ȱ��ȭ ����.
    private bool isReload = false;
    private bool isFire = false;
    public  bool isChangeWeapon = false;

    private WaitForSeconds gunDelay=new WaitForSeconds(0.01f);

    // �ʿ��� ������Ʈ
    //[SerializeField] private Crosshair theCrosshair;

    // ���� �������� ���� ���� ������ �����ϵ��� ����.
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();

    public static GunController instance;

    private void Awake()
    {
        GunController.instance = this;
    }
    void Start()
    {
        cameraCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2); // ȭ�� �߾��� ���ϱ�

        //���� �߰�
        for (int i = 0; i < PlayerController.instance.currentGunList.Length; i++)
        {
            gunDictionary.Add(PlayerController.instance.currentGunList[i].gunName, PlayerController.instance.currentGunList[i]);
        }
        init();

        impactMemoryPool = GetComponent<ImpactMemoryPool>(); // ����Ʈ ���
    }
    void init()
    {
        //ù��° ���� ����
        currentGun = PlayerController.instance.currentGunList[0];
        gunObj = GameObject.Find("Weapon").transform.Find(currentGun.name);
        gunObj.gameObject.SetActive(true);
        //gunObj.GetComponent<Gun>().init(currentGun);//���� �ʱ�ȭ
        currentGun = gunObj.GetComponent<Gun>();
        setGunAnimation();
        checkItem();
    }
    void checkItem()
    {
        if (ItemData.instance.damage == true)
        {
            if (currentGun.name == "SG2")
            {
                currentGun.damage += 1.5f;
            }
            else if (currentGun.name == "SG1")
            {
                currentGun.damage += 3.5f;
            }
            else if (currentGun.name == "AR2" || currentGun.name == "SMG1")
            {
                currentGun.damage += 5f;
            }
            else if (currentGun.name == "AR1")
            {
                currentGun.damage += 7.5f;
            }
            else if (currentGun.name == "HG2")
            {
                currentGun.damage += 9f;
            }
            else if (currentGun.name == "HG1" || currentGun.name == "LMG")
            {
                currentGun.damage += 10f;
            }
            else if (currentGun.name == "DMR2")
            {
                currentGun.damage += 12f;
            }
            else if (currentGun.name == "DMR1")
            {
                currentGun.damage += 15f;
            }
            else
            {
                currentGun.damage += 40f;
            }
        }
        if (ItemData.instance.ammo == true)
        {
            if (currentGun.name == "SG1")
            {
                currentGun.carryBulletCount += 4;
            }
            else if (currentGun.name == "SR1")
            {
                currentGun.carryBulletCount += 5;
            }
            else if (currentGun.name == "SG2")
            {
                currentGun.carryBulletCount += 6;
            }
            else if (currentGun.name == "DMR1")
            {
                currentGun.carryBulletCount += 15;
            }
            else if (currentGun.name == "DMR2")
            {
                currentGun.carryBulletCount += 20;
            }
            else if (currentGun.name == "HG1")
            {
                currentGun.carryBulletCount += 30;
            }
            else if (currentGun.name == "HG2")
            {
                currentGun.carryBulletCount += 34;
            }
            else if (currentGun.name == "AR1")
            {
                currentGun.carryBulletCount += 40;
            }
            else if (currentGun.name == "AR2")
            {
                currentGun.carryBulletCount += 60;
            }
            else if (currentGun.name == "SMG1")
            {
                currentGun.carryBulletCount += 75;
            }
            else
            {
                currentGun.carryBulletCount += 100;
            }
        }
        if (ItemData.instance.magazine == true)
        {
            if (currentGun.name == "SR1")
            {
                currentGun.reloadBulletCount += 1;
            }
            else if (currentGun.name == "HG1" || currentGun.name == "DMR1" || currentGun.name == "SG1")
            {
                currentGun.reloadBulletCount += 2;
            }
            else if (currentGun.name == "HG2" || currentGun.name == "DMR2" || currentGun.name == "SG2")
            {
                currentGun.reloadBulletCount += 3;
            }
            else if (currentGun.name == "AR1")
            {
                currentGun.reloadBulletCount += 4;
            }
            else if (currentGun.name == "AR2")
            {
                currentGun.reloadBulletCount += 6;
            }
            else if (currentGun.name == "SMG1")
            {
                currentGun.reloadBulletCount += 10;
            }
            else
            {
                currentGun.reloadBulletCount += 20;
            }
        }
    }

    void Update()
    {
        if (PlayerController.instance.IsStunned || DebugMod.isDebug)
        {
            return;
        }
        TryReload();

        if (!isChangeWeapon && !isFire)//���ⱳü
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && currentGun.name != PlayerController.instance.currentGunList[0].name)
                StartCoroutine(ChangeWeaponCoroutine("GUN", PlayerController.instance.currentGunList[0].name));
            else if (Input.GetKeyDown(KeyCode.Alpha2) && currentGun.name != PlayerController.instance.currentGunList[1].name)
                StartCoroutine(ChangeWeaponCoroutine("GUN", PlayerController.instance.currentGunList[1].name));
        }
    }

    private void FixedUpdate()
    {
        if (PlayerController.instance.IsStunned || DebugMod.isDebug)
        {
            return;
        }
        if (isActivate)
        {
            Fire(); 
            Aim();
        }
    }

    private void Fire()
    {
      
        if (Input.GetButton("Fire1") && !isFire)
        {
            if (isReload&& currentGun.name == "SG1")
            {
                CancelReload();
                weaponAnim.SetTrigger("ReloadEnd");//�ִϸ��̼Ǽ���
            }
            if (!isReload)
            {
                if (currentGun.currentBulletCount > 0)
                {
                    StartCoroutine(Shoot());
                }
                else
                {
                    Debug.Log("������ �Ѿ��� �����ϴ�");//�Ѿ��� ���� ��Ĭ�Ÿ��� ȿ���� ���
                }
            }
        }
    }

    IEnumerator Shoot()
    {
        if (!PlayerController.instance.isRun && weaponAnim.GetCurrentAnimatorStateInfo(0).IsName("armature_" + currentGun.name + "_Idle"))//�޸����Ǵ�,Idle������ ���
        {
            isFire = true;
            weaponAnim.SetTrigger("Fire");//�ִϸ��̼� ����
            currentGun.MuzzleFlash(); // ����Ʈ
            Hit(); // RayCast ���
            currentGun.currentBulletCount--;//�Ѿ˰���

            //PlaySE(currentGun.fire_Sound); // ����
            //StartCoroutine(RetroActionCoroutine()); // �ݵ�
            /*
            while (!weaponAnim.GetCurrentAnimatorStateInfo(0).IsName("armature_" + currentGun.name + "_Fire"))//������
            {
                yield return null;
            }*/
            yield return new WaitForSeconds(currentGun.fireRate);
            isFire = false;
        }
    }

    private void TryReload()//������
    {
       
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        { 
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {            
            weaponAnim.SetTrigger("Reload");//�ִϸ��̼Ǽ���
        isReload = true;
        if (currentGun.name == "SG1")//���� ����
        {
            while (currentGun.currentBulletCount < currentGun.reloadBulletCount && isReload)
            {
                yield return new WaitForSeconds(currentGun.reloadTime);//������ �ð�
                currentGun.currentBulletCount += 1;
            }
        }
        else
        {
            yield return new WaitForSeconds(currentGun.reloadTime);//������ �ð�
            currentGun.currentBulletCount = currentGun.reloadBulletCount;
        }

        if (currentGun.currentBulletCount == currentGun.reloadBulletCount)
        {
            if (currentGun.name == "SG1")//���� ����
                weaponAnim.SetTrigger("ReloadEnd");//�ִϸ��̼Ǽ���
            isReload = false;
        }

    }

    public void CancelReload()//������ ���
    {
        if (isReload)
        {
            StopCoroutine(ReloadCoroutine());
            isReload = false;
        }
    }

    public void Aim()
    {
        if (Input.GetMouseButtonDown(1) && currentGun.name == "SR1")
        {
            weaponAnim.SetTrigger("Aim");//�ִϸ��̼Ǽ���
        }
    }

    private void Hit() // RAyCast �̿��� ���
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        ray = Camera.main.ScreenPointToRay(cameraCenter);

        if (Physics.Raycast(ray, out hit, currentGun.range))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * currentGun.range;
        }

        Vector3 attackDirection = (targetPoint - currentGun.bulletSpawnPoint.position).normalized;
        if (Physics.Raycast(currentGun.bulletSpawnPoint.position, attackDirection, out hit, currentGun.range))
        {
            impactMemoryPool.SpawnImpact(hit); // ����

            if (hit.transform.CompareTag("Enemy")) // �� ����
            {
                hit.transform.GetComponent<EnemyFSM>().TakeDamage(currentGun.damage);
            }
            if (hit.transform.CompareTag("EnemyHead")) // �� �Ӹ�
            {
                hit.transform.GetComponent<EnemyFSM>().TakeDamage(currentGun.damage * 1.5f);
            }
            if (hit.transform.CompareTag("Turret")) // �ͷ�
            {
                hit.transform.GetComponent<Turret>().TakeDamage(currentGun.damage);
            }

            if (hit.transform.CompareTag("MidBoss1")) // �߰�����1 ����
            {
                hit.transform.GetComponent<MidBossFSM_1>().TakeDamage(currentGun.damage);
            }
            if (hit.transform.CompareTag("MidBoss1_Head")) // �߰�����1 �Ӹ�
            {
                hit.transform.GetComponent<MidBossFSM_1>().TakeDamage(currentGun.damage * 1.5f);
            }
            if (hit.transform.CompareTag("Baby")) // �߰�����1 ����
            {
                hit.transform.GetComponent<MidBoss1_Baby>().TakeDamage(currentGun.damage);
            }
            if (hit.transform.CompareTag("Egg")) // �߰�����1 ��
            {
                hit.transform.GetComponent<MidBoss1_Egg>().TakeDamage(currentGun.damage);
            }

            if (hit.transform.CompareTag("SnowMan")) // �� �߰�����2 ������
            {
                hit.transform.GetComponent<SnowMan>().TakeDamage(currentGun.damage);
            }
            if (hit.transform.CompareTag("MidBoss2")) // �߰�����2 ����
            {
                hit.transform.GetComponent<MidBossFSM_2>().TakeDamage(currentGun.damage);
            }
            if (hit.transform.CompareTag("MidBoss2_Head")) // �߰�����2 �Ӹ�
            {
                hit.transform.GetComponent<MidBossFSM_2>().TakeDamage(currentGun.damage * 1.5f);
            }

            if (hit.transform.CompareTag("MidBoss3")) // �߰�����3 (���� �Ӹ����� ����)
            {
                hit.transform.GetComponent<MidBoss3>().TakeDamage(currentGun.damage);
            }
            if (hit.transform.CompareTag("Missile"))
            {
                hit.transform.GetComponent<Missile>().TakeDamage(currentGun.damage);
            }

            if (hit.transform.CompareTag("Stage1_Boss")) // �߰�����4 ����
            {
                hit.transform.GetComponentInParent<BossFSM>().TakeDamage(currentGun.damage);
            }
            if (hit.transform.CompareTag("Stage1_Boss_Head")) // �߰�����4 �Ӹ�
            {
                hit.transform.GetComponent<BossFSM>().TakeDamage(currentGun.damage * 1.5f);
            }

            if (hit.transform.CompareTag("SunBoss"))
            {
                hit.transform.GetComponent<SunBoss>().TakeDamage((int)currentGun.damage);
            }
            if (hit.transform.CompareTag("Ball")) // �� ���� ������ ��ü
            {
                hit.transform.GetComponent<Ball>().TakeDamage((int)currentGun.damage);
            }

        }

    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;

        weaponAnim.SetBool("Change", true);

        CancelPreWeaponAction();
        yield return new WaitForSeconds(currentGun.outWeaponTime);

        if (gunObj != null)//���� ���� ��Ȱ��ȭ
            gunObj.gameObject.SetActive(false);

        currentGun = gunDictionary[_name];//��Ʈ�ѷ����� ���� ���� ��ü

        gunObj = GameObject.Find("Weapon").transform.Find(currentGun.name);

        gunObj.gameObject.SetActive(true);//��ü�� ���� Ȱ��ȭ
        //gunObj.GetComponent<Gun>().init(currentGun);
        currentGun = gunObj.GetComponent<Gun>();

        //��ü�� �� �ִϸ��̼� ����
        setGunAnimation();
        /*
        while (!weaponAnim.GetCurrentAnimatorStateInfo(0).IsName("armature_" + currentGun.name + "_Get"))
        {
            yield return null;
        }*/
        yield return new WaitForSeconds(currentGun.getWeaponTime);//���� ������ ������
        weaponAnim.SetBool("Change", false);
        weaponAnim.SetTrigger(_name);

        isChangeWeapon = false;
        isActivate = true;//���� Ȱ��ȭ
    }

    // ���� �ൿ ��� �Լ�.
    private void CancelPreWeaponAction()
    {
        CancelReload();
        isActivate = false;
    }

    public void setGunAnimation()
    {
        Debug.Log("����:" + currentGun.name);
        weaponAnim = PlayerController.instance.anim;
        weaponAnim.SetFloat("getWeaponTime", currentGun.getWeaponTime);
        weaponAnim.SetFloat("outWeaponTime", currentGun.outWeaponTime);
        weaponAnim.SetFloat("reloadTime", currentGun.reloadTime);
        weaponAnim.SetBool(currentGun.name, true);
    }

}
