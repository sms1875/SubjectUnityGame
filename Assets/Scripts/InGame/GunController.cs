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

    //private ImpactMemoryPool impactMemoryPool; // ����Ʈ ��¿� �޸� ������Ʈ

    // ���� ����
    public static bool isActivate = true; //���� Ȱ��ȭ ����.
    private bool isReload = false;
    private bool isFire = false;
    public  bool isChangeWeapon = false;

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

        //impactMemoryPool = GetComponent<ImpactMemoryPool>(); // ����Ʈ ���
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
        if (ItemData.instance.IncreaseDamage)
        {
            ItemData.instance.DamageUp();
        }
    }

    void Update()
    {
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
        if (isActivate)
        {
            Fire();
            TryReload();
            Aim();
        }
    }

    private void Fire()
    {
        if (Input.GetButton("Fire1") && !isFire)
        {
            if (isReload)
            {
                CancelReload();
                if (currentGun.name == "SG1")//���� ����
                    weaponAnim.SetTrigger("ReloadEnd");//�ִϸ��̼Ǽ���
            }
            if (currentGun.currentBulletCount > 0)
                StartCoroutine(Shoot());
            else
            {
                Debug.Log("������ �Ѿ��� �����ϴ�");//�Ѿ��� ���� ��Ĭ�Ÿ��� ȿ���� ���
            }
        }
    }

    IEnumerator Shoot()
    {
        isFire = true;
        if (!PlayerController.instance.isRun && weaponAnim.GetCurrentAnimatorStateInfo(0).IsName("armature_" + currentGun.name + "_Idle"))//�޸����Ǵ�,Idle������ ���
        {
            weaponAnim.SetTrigger("Fire");//�ִϸ��̼� ����
            Hit(); // RayCast ���
            currentGun.MuzzleFlash(); // ����Ʈ
            currentGun.currentBulletCount--;//�Ѿ˰���
            //currentGun.anim.SetBool("Run", false);
            //playerController.makeRunFalse();//�ȱ���·� ���ϰ� ����
        }
        //PlaySE(currentGun.fire_Sound); // ����
        //StartCoroutine(RetroActionCoroutine()); // �ݵ�
        yield return new WaitForSeconds(currentGun.fireRate);
        isFire = false;
    }

    private void TryReload()//������
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            isReload = true;
            weaponAnim.SetTrigger("Reload");//�ִϸ��̼Ǽ���
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {

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
        //Debug.DrawRay(ray.origin, ray.direction * currentGun.range, Color.red);

        //Vector3 attackDirection = (targetPoint - currentGun.bulletSpawnPoint.position).normalized;
        //if (Physics.Raycast(currentGun.bulletSpawnPoint.position, attackDirection, out hit, currentGun.range))
        {
            //   impactMemoryPool.SpawnImpact(hit); // ����
            // Debug.Log("Ray ���� �� ȿ�� ���");
            /*
            if (hit.transform.CompareTag("ImpactEnemy")) // �� ����
            {
                hit.transform.GetComponent<EnemyFSM>().TakeDamage(currentGun.damage);
            } Debug.Log("�� ����")
            if (hit.transform.CompareTag("ImpactEnemy_Head")) // �� �Ӹ�
            {
                hit.transform.GetComponent<EnemyFSM>().TakeDamage(currentGun.damage * 1.5);
            } Debug.Log("�� �Ӹ�")
            if (hit.transform.CompareTag("ImpactMidBoss1")) // �߰�����1 ����
            {
                hit.transform.GetComponent<MidBoss1_FSM>().TakeDamage(currentGun.damage);
            } Debug.Log("�ߺ�1 ����")
            if (hit.transform.CompareTag("ImpactMidBoss1_Head")) // �߰�����1 �Ӹ�
            {
                hit.transform.GetComponent<MidBoss1_FSM>().TakeDamage(currentGun.damage * 1.5);
            } Debug.Log("�ߺ�1 �Ӹ�")
            if (hit.transform.CompareTag("ImpactMidBoss2")) // �߰�����2 ����
            {
                hit.transform.GetComponent<MidBoss2_FSM>().TakeDamage(currentGun.damage);
            } Debug.Log("�ߺ�2 ����")
            if (hit.transform.CompareTag("ImpactMidBoss2_Head")) // �߰�����2 �Ӹ�
            {
                hit.transform.GetComponent<MidBoss_2FSM>().TakeDamage(currentGun.damage * 1.5);
            } Debug.Log("�ߺ�2 �Ӹ�")
            if (hit.transform.CompareTag("ImpactDrone")) // ���
            {
                hit.transform.GetComponent<DroneController>().TakeDamage(currentGun.damage);
            } Debug.Log("���")
            if (hit.transform.CompareTag("ImpactStage1_Boss")) // 1���� ����
            {
                hit.transform.GetComponent<BossFSM>().TakeDamage(currentGun.damage * 1.5);
            } Debug.Log("1���� ����")
            */ // ������ ��� ����
        }
        //Debug.DrawRay(currentGun.bulletSpawnPoint.position, attackDirection * currentGun.range, Color.blue);

        // Debug.DrayRay�� üũ���̶� ������ ����

    }

    /*
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
    */

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;

        weaponAnim.SetBool("Change", true);

        CancelPreWeaponAction();

        yield return new WaitForSeconds(currentGun.outWeaponTime);
        while (!weaponAnim.GetCurrentAnimatorStateInfo(0).IsName("armature_" + currentGun.name + "_Out"))
        {
            yield return null;
        }

        if (gunObj != null)//���� ���� ��Ȱ��ȭ
            gunObj.gameObject.SetActive(false);

        currentGun = gunDictionary[_name];//��Ʈ�ѷ����� ���� ���� ��ü

        gunObj = GameObject.Find("Weapon").transform.Find(currentGun.name);

        gunObj.gameObject.SetActive(true);//��ü�� ���� Ȱ��ȭ
        //gunObj.GetComponent<Gun>().init(currentGun);
        currentGun = gunObj.GetComponent<Gun>();
 

        //��ü�� �� �ִϸ��̼� ����
        setGunAnimation();      
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
