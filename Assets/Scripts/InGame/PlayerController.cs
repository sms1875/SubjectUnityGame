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
}
