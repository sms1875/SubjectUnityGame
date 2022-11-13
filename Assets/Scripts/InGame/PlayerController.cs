using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    #region 상태변수
    public bool isWalk = false;
    public bool isRun = false;
    public bool isCrouch = false;
    public bool isGround = false;
    #endregion

    public Animator anim;

    public CharacterController charController;
    //private Crosshair theCrosshair;
    private DataManager dataManager;

    public float currentHp;
    public float currentDp;
    public float currentDashGauge;

    public Gun[] currentGunList; // 현재 장착된 총

    public AudioSource PlayerSoundSource;
    public AudioClip[] PlayerSoundClip;

    public static PlayerController instance;

    // [SerializeField] private Inventory inventory;

    private void Awake()
    {
        PlayerController.instance = this;
        init();
    }
    
    private void Start()
    {
        GameController.instance.Result();
    }

    //초기화
    private void init()
    {
        charController = GetComponent<CharacterController>();
        //theCrosshair = FindObjectOfType<Crosshair>();
        anim = GetComponent<Animator>();
        currentGunList = PlayerData.instance.currentGunList;

        if (PlayerData.instance != null)
        {
            currentHp = PlayerData.instance.currentHp;
            currentDp = PlayerData.instance.currentDp;
            currentDashGauge = PlayerData.instance.DashGauge;
        }
        StartCoroutine(setSound());
        TakeDamage(20);

    }

    private void Update()
    {
        //초당 6씩 게이지 증가
        if (currentDashGauge < PlayerData.instance.DashGauge)
        {
            currentDashGauge += 0.1f;
        }
    /*
        if (!Inventory.inventoryActivated)
        {
            CameraRotation();
            CharacterRotation();
        }
    */
    }

    private void FixedUpdate()
    {
        SetAnimation();
    }

    private void SetAnimation()//애니메이션 상태 설정
    {
        anim.SetBool("Run", isRun);
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Crouch", isCrouch);

        //theCrosshair.WalkingAnimation(GetWalk());//크로스헤어 걷기
        //theCrosshair.RunningAnimation(GetRun());//크로스헤어 달리기설정
        //theCrosshair.CrouchingAnimation(GetCrouch());//크로스헤어 앉기
        //theCrosshair.JumpingAnimation(!GetIsGround());//크로스헤어 점프
    }

  IEnumerator setSound()
    {
        if (isRun)
        {
            PlayerSoundSource.clip = PlayerSoundClip[1];
            if(!PlayerSoundSource.isPlaying)
            PlayerSoundSource.Play();
        }
        else if (isWalk)
        {
            PlayerSoundSource.clip = PlayerSoundClip[0];
            if (!PlayerSoundSource.isPlaying)
                PlayerSoundSource.Play();
        }
        else
            PlayerSoundSource.Stop();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(setSound());
    }

    public void TakeDamage(int damage)//데미지 처리
    {
        currentHp -= damage - currentDp;
        Debug.Log("현재 플레이어 체력:"+currentHp);
        if (currentHp <= 0)
        {
            Debug.Log("사망했습니다");
        }
    }


    // 몬스터 공격 상호작용(?)
    private bool isStunned;  // 몬스터에게 공격 받으면 카메라 및 움직임 조작 불가

    public bool IsStunned => isStunned;
    public void OnStun(float time)
    {
        StartCoroutine("Stunned", time);
    }

    private IEnumerator Stunned(float time)
    {
        isStunned = true;
        isWalk = false;
        isRun = false;
        isCrouch = false;
        isGround = false;

        yield return new WaitForSeconds(time);

        isStunned = false;
    }

    public void OnBounce(Vector3 direction)
    {
        StartCoroutine("Bounce", direction);
    }

    private IEnumerator Bounce(Vector3 direction)
    {
        isStunned = true;
        isWalk = false;
        isRun = false;
        isCrouch = false;
        isGround = false;

        float currentTime = 0;
        while (true)
        {
            if (currentTime >= 1)
            {
                break;
            }
            charController.Move(direction * -15f * Time.deltaTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        isStunned = false;
    }

    public void OnShake(float time, float intensity, float speed)
    {
        StartCoroutine(Shake(time, intensity, speed));
    }

    private IEnumerator Shake(float time, float intensity, float speed)
    {
        Transform cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        Vector3 originPosition = cam.localPosition;
        float currentTime = 0;
        while (true)
        {
            if (currentTime >= time)
            {
                cam.localPosition = originPosition;
                yield break;
            }
            Vector3 randomPoint = originPosition + Random.insideUnitSphere * intensity;
            cam.localPosition = Vector3.Lerp(cam.localPosition, randomPoint, Time.deltaTime * speed);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    /*
    private void ItemPickUp()
    {
        if (pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponet<ItemPickUp>().item.itemName + "획득 했습니다.");
                theInventory.AcquireItem(hitInfo.transform.GetComponet<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }
    */
}
