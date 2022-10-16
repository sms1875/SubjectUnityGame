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

    //private ImpactMemoryPool impactMemoryPool; // 이펙트 출력용 메모리 컴포넌트

    // 상태 변수
    public static bool isActivate = true; //무기 활성화 여부.
    private bool isReload = false;
    private bool isFire = false;
    public  bool isChangeWeapon = false;

    // 필요한 컴포넌트
    //[SerializeField] private Crosshair theCrosshair;

    // 관리 차원에서 쉽게 무기 접근이 가능하도록 만듦.
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();

    public static GunController instance;

    private void Awake()
    {
        GunController.instance = this;
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

        //impactMemoryPool = GetComponent<ImpactMemoryPool>(); // 이펙트 출력
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
        if (ItemData.instance.IncreaseDamage)
        {
            ItemData.instance.DamageUp();
        }
    }

    void Update()
    {
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
                if (currentGun.name == "SG1")//샷건 장전
                    weaponAnim.SetTrigger("ReloadEnd");//애니메이션설정
            }
            if (currentGun.currentBulletCount > 0)
                StartCoroutine(Shoot());
            else
            {
                Debug.Log("장전된 총알이 없습니다");//총알이 없어 찰칵거리는 효과음 출력
            }
        }
    }

    IEnumerator Shoot()
    {
        isFire = true;
        if (!PlayerController.instance.isRun && weaponAnim.GetCurrentAnimatorStateInfo(0).IsName("armature_" + currentGun.name + "_Idle"))//달리기판단,Idle에서만 사격
        {
            weaponAnim.SetTrigger("Fire");//애니메이션 설정
            Hit(); // RayCast 사격
            currentGun.MuzzleFlash(); // 이펙트
            currentGun.currentBulletCount--;//총알감소
            //currentGun.anim.SetBool("Run", false);
            //playerController.makeRunFalse();//걷기상태로 변하게 변경
        }
        //PlaySE(currentGun.fire_Sound); // 사운드
        //StartCoroutine(RetroActionCoroutine()); // 반동
        yield return new WaitForSeconds(currentGun.fireRate);
        isFire = false;
    }

    private void TryReload()//재장전
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            isReload = true;
            weaponAnim.SetTrigger("Reload");//애니메이션설정
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {

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
        //Debug.DrawRay(ray.origin, ray.direction * currentGun.range, Color.red);

        //Vector3 attackDirection = (targetPoint - currentGun.bulletSpawnPoint.position).normalized;
        //if (Physics.Raycast(currentGun.bulletSpawnPoint.position, attackDirection, out hit, currentGun.range))
        {
            //   impactMemoryPool.SpawnImpact(hit); // 명중
            // Debug.Log("Ray 명중 및 효과 출력");
            /*
            if (hit.transform.CompareTag("ImpactEnemy")) // 적 몸통
            {
                hit.transform.GetComponent<EnemyFSM>().TakeDamage(currentGun.damage);
            } Debug.Log("적 몸통")
            if (hit.transform.CompareTag("ImpactEnemy_Head")) // 적 머리
            {
                hit.transform.GetComponent<EnemyFSM>().TakeDamage(currentGun.damage * 1.5);
            } Debug.Log("적 머리")
            if (hit.transform.CompareTag("ImpactMidBoss1")) // 중간보스1 몸통
            {
                hit.transform.GetComponent<MidBoss1_FSM>().TakeDamage(currentGun.damage);
            } Debug.Log("중보1 몸통")
            if (hit.transform.CompareTag("ImpactMidBoss1_Head")) // 중간보스1 머리
            {
                hit.transform.GetComponent<MidBoss1_FSM>().TakeDamage(currentGun.damage * 1.5);
            } Debug.Log("중보1 머리")
            if (hit.transform.CompareTag("ImpactMidBoss2")) // 중간보스2 몸통
            {
                hit.transform.GetComponent<MidBoss2_FSM>().TakeDamage(currentGun.damage);
            } Debug.Log("중보2 몸통")
            if (hit.transform.CompareTag("ImpactMidBoss2_Head")) // 중간보스2 머리
            {
                hit.transform.GetComponent<MidBoss_2FSM>().TakeDamage(currentGun.damage * 1.5);
            } Debug.Log("중보2 머리")
            if (hit.transform.CompareTag("ImpactDrone")) // 드론
            {
                hit.transform.GetComponent<DroneController>().TakeDamage(currentGun.damage);
            } Debug.Log("드론")
            if (hit.transform.CompareTag("ImpactStage1_Boss")) // 1스테 보스
            {
                hit.transform.GetComponent<BossFSM>().TakeDamage(currentGun.damage * 1.5);
            } Debug.Log("1스테 보스")
            */ // 데미지 계산 관련
        }
        //Debug.DrawRay(currentGun.bulletSpawnPoint.position, attackDirection * currentGun.range, Color.blue);

        // Debug.DrayRay는 체크용이라 지워도 무방

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

        if (gunObj != null)//현재 무기 비활성화
            gunObj.gameObject.SetActive(false);

        currentGun = gunDictionary[_name];//컨트롤러에서 현재 무기 교체

        gunObj = GameObject.Find("Weapon").transform.Find(currentGun.name);

        gunObj.gameObject.SetActive(true);//교체한 무기 활성화
        //gunObj.GetComponent<Gun>().init(currentGun);
        currentGun = gunObj.GetComponent<Gun>();
 

        //교체한 총 애니메이션 설정
        setGunAnimation();      
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
