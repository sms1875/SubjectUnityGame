using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    // 현재 무기와 현재 무기의 애니메이션.
    public Gun currentGun;
    private Animator weaponAnim;
    private Transform gunObj;

    private Vector3 cameraCenter;

    public ImpactMemoryPool impactMemoryPool; // 이펙트 출력용 메모리 컴포넌트

    // 상태 변수
    public static bool isActivate = true; //무기 활성화 여부.
    private bool isReload = false;
    private bool isFire = false;
    public  bool isChangeWeapon = false;

    private WaitForSeconds gunDelay=new WaitForSeconds(0.01f);

    // 필요한 컴포넌트
    //[SerializeField] private Crosshair theCrosshair;

    // 관리 차원에서 쉽게 무기 접근이 가능하도록 만듦.
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();

    public static GunController instance;

    //플레이어 머리
    private CameraController cameraController;

    private void Awake()
    {
        GunController.instance = this;
        cameraController = GetComponentInChildren<CameraController>();
    }
    void Start()
    {
        cameraCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2); // 화면 중앙점 구하기

        //무기 추가
        for (int i = 0; i < PlayerController.instance.currentGunList.Length; i++)
        {
            gunDictionary.Add(PlayerController.instance.currentGunList[i].gunName, PlayerController.instance.currentGunList[i]);
        }
        init();

        impactMemoryPool = GetComponent<ImpactMemoryPool>(); // 이펙트 출력
    }
    void init()
    {
        //첫번째 무기 장착
        currentGun = PlayerController.instance.currentGunList[0];
        gunObj = GameObject.Find("Weapon").transform.Find(currentGun.name);
        gunObj.gameObject.SetActive(true);
        //gunObj.GetComponent<Gun>().init(currentGun);//원본 초기화
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

        if (!isChangeWeapon && !isFire)//무기교체
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
                weaponAnim.SetTrigger("ReloadEnd");//애니메이션설정
            }
            if (!isReload)
            {
                if (currentGun.currentBulletCount > 0)
                {
                    StartCoroutine(Shoot());
                }
                else
                {
                    Debug.Log("장전된 총알이 없습니다");//총알이 없어 찰칵거리는 효과음 출력
                }
            }
        }
    }

    IEnumerator Shoot()
    {
        if (!PlayerController.instance.isRun && weaponAnim.GetCurrentAnimatorStateInfo(0).IsName("armature_" + currentGun.name + "_Idle"))//달리기판단,Idle에서만 사격
        {
            isFire = true;
            weaponAnim.SetTrigger("Fire");//애니메이션 설정
            currentGun.MuzzleFlash(); // 이펙트
            Hit(); // RayCast 사격
            currentGun.currentBulletCount--;//총알감소

            //PlaySE(currentGun.fire_Sound); // 사운드
            StartCoroutine(RetroActionCoroutine()); // 반동
            /*
            while (!weaponAnim.GetCurrentAnimatorStateInfo(0).IsName("armature_" + currentGun.name + "_Fire"))//공격중
            {
                yield return null;
            }*/
            yield return new WaitForSeconds(currentGun.fireRate);
            isFire = false;
        }
    }

    private IEnumerator RetroActionCoroutine()
    {
        PlayerController.instance.OnShake(0.15f, currentGun.retroActionForce, 1f);

        float aspect = Random.Range(-1f, 1f);
        float currentTime = 0f;
        while (isFire)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + currentGun.retroActionForce * aspect * Time.deltaTime, 0);
            cameraController.BoundY(currentGun.retroActionForce * Time.deltaTime);

            if (currentTime >= 0.2f)
            {
                yield break;
            }

            currentTime += Time.deltaTime;

            yield return null;
        }
    }

    private void TryReload()//재장전
    {
       
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        { 
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {            
            weaponAnim.SetTrigger("Reload");//애니메이션설정
        isReload = true;
        if (currentGun.name == "SG1")//샷건 장전
        {
            while (currentGun.currentBulletCount < currentGun.reloadBulletCount && isReload)
            {
                yield return new WaitForSeconds(currentGun.reloadTime);//재장전 시간
                currentGun.currentBulletCount += 1;
            }
        }
        else
        {
            yield return new WaitForSeconds(currentGun.reloadTime);//재장전 시간
            currentGun.currentBulletCount = currentGun.reloadBulletCount;
        }

        if (currentGun.currentBulletCount == currentGun.reloadBulletCount)
        {
            if (currentGun.name == "SG1")//샷건 장전
                weaponAnim.SetTrigger("ReloadEnd");//애니메이션설정
            isReload = false;
        }

    }

    public void CancelReload()//재장전 취소
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
            weaponAnim.SetTrigger("Aim");//애니메이션설정
        }
    }

    private void Hit() // RAyCast 이용한 사격
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
            impactMemoryPool.SpawnImpact(hit); // 명중

            if (hit.transform.CompareTag("Enemy")) // 적 몸통
            {
                hit.transform.GetComponent<EnemyFSM>().TakeDamage(currentGun.damage);
            }
            if (hit.transform.CompareTag("EnemyHead")) // 적 머리
            {
                hit.transform.GetComponent<EnemyFSM>().TakeDamage(currentGun.damage * 1.5f);
            }
            if (hit.transform.CompareTag("Turret")) // 터렛
            {
                hit.transform.GetComponent<Turret>().TakeDamage(currentGun.damage);
            }

            if (hit.transform.CompareTag("MidBoss1")) // 중간보스1 몸통
            {
                hit.transform.GetComponent<MidBossFSM_1>().TakeDamage(currentGun.damage);
            }
            if (hit.transform.CompareTag("MidBoss1_Head")) // 중간보스1 머리
            {
                hit.transform.GetComponent<MidBossFSM_1>().TakeDamage(currentGun.damage * 1.5f);
            }
            if (hit.transform.CompareTag("Baby")) // 중간보스1 새끼
            {
                hit.transform.GetComponent<MidBoss1_Baby>().TakeDamage(currentGun.damage);
            }
            if (hit.transform.CompareTag("Egg")) // 중간보스1 알
            {
                hit.transform.GetComponent<MidBoss1_Egg>().TakeDamage(currentGun.damage);
            }

            if (hit.transform.CompareTag("SnowMan")) // 찐 중간보스2 스노우맨
            {
                hit.transform.GetComponent<SnowMan>().TakeDamage(currentGun.damage);
            }
            if (hit.transform.CompareTag("MidBoss2")) // 중간보스2 몸통
            {
                hit.transform.GetComponent<MidBossFSM_2>().TakeDamage(currentGun.damage);
            }
            if (hit.transform.CompareTag("MidBoss2_Head")) // 중간보스2 머리
            {
                hit.transform.GetComponent<MidBossFSM_2>().TakeDamage(currentGun.damage * 1.5f);
            }

            if (hit.transform.CompareTag("MidBoss3")) // 중간보스3 (기계라 머리구분 없음)
            {
                hit.transform.GetComponent<MidBoss3>().TakeDamage(currentGun.damage);
            }
            if (hit.transform.CompareTag("Missile"))
            {
                hit.transform.GetComponent<Missile>().TakeDamage(currentGun.damage);
            }

            if (hit.transform.CompareTag("Stage1_Boss")) // 중간보스4 몸통
            {
                hit.transform.GetComponentInParent<BossFSM>().TakeDamage(currentGun.damage);
            }
            if (hit.transform.CompareTag("Stage1_Boss_Head")) // 중간보스4 머리
            {
                hit.transform.GetComponent<BossFSM>().TakeDamage(currentGun.damage * 1.5f);
            }

            if (hit.transform.CompareTag("SunBoss"))
            {
                hit.transform.GetComponent<SunBoss>().TakeDamage((int)currentGun.damage);
            }
            if (hit.transform.CompareTag("Ball")) // 선 보스 에너지 구체
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

        if (gunObj != null)//현재 무기 비활성화
            gunObj.gameObject.SetActive(false);

        currentGun = gunDictionary[_name];//컨트롤러에서 현재 무기 교체

        gunObj = GameObject.Find("Weapon").transform.Find(currentGun.name);

        gunObj.gameObject.SetActive(true);//교체한 무기 활성화
        //gunObj.GetComponent<Gun>().init(currentGun);
        currentGun = gunObj.GetComponent<Gun>();

        //교체한 총 애니메이션 설정
        setGunAnimation();
        /*
        while (!weaponAnim.GetCurrentAnimatorStateInfo(0).IsName("armature_" + currentGun.name + "_Get"))
        {
            yield return null;
        }*/
        yield return new WaitForSeconds(currentGun.getWeaponTime);//무기 꺼내는 딜레이
        weaponAnim.SetBool("Change", false);
        weaponAnim.SetTrigger(_name);

        isChangeWeapon = false;
        isActivate = true;//무기 활성화
    }

    // 무기 행동 취소 함수.
    private void CancelPreWeaponAction()
    {
        CancelReload();
        isActivate = false;
    }

    public void setGunAnimation()
    {
        Debug.Log("무기:" + currentGun.name);
        weaponAnim = PlayerController.instance.anim;
        weaponAnim.SetFloat("getWeaponTime", currentGun.getWeaponTime);
        weaponAnim.SetFloat("outWeaponTime", currentGun.outWeaponTime);
        weaponAnim.SetFloat("reloadTime", currentGun.reloadTime);
        weaponAnim.SetBool(currentGun.name, true);
    }

}
