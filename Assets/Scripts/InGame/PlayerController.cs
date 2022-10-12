using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    #region ���º���
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

    public Gun[] currentGunList; // ���� ������ ��

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

   //�ʱ�ȭ
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
        //�ʴ� 6�� ������ ����
        if (currentDashGauge < PlayerData.instance.DashGauge)
        {
            currentDashGauge += 0.1f;
        }
    }

    private void FixedUpdate()
    {
        SetAnimation();
    }

    private void SetAnimation()//�ִϸ��̼� ���� ����
    {
        anim.SetBool("Run", isRun);
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Crouch", isCrouch);

        //theCrosshair.WalkingAnimation(GetWalk());//ũ�ν���� �ȱ�
        //theCrosshair.RunningAnimation(GetRun());//ũ�ν���� �޸��⼳��
        //theCrosshair.CrouchingAnimation(GetCrouch());//ũ�ν���� �ɱ�
        //theCrosshair.JumpingAnimation(!GetIsGround());//ũ�ν���� ����
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

    public void TakeDamage(int damage)//������ ó��
    {
        currentHp -= damage - currentDp;
        Debug.Log("���� �÷��̾� ü��:"+currentHp);
        if (currentHp <= 0)
        {
            Debug.Log("����߽��ϴ�");
        }
    }


    // ���� ���� ��ȣ�ۿ�(?)
    private bool isStunned;  // ���Ϳ��� ���� ������ ī�޶� �� ������ ���� �Ұ�

    private float originWalkSpeed;
    private float originRunSpeed;
    private float originCrouchSpeed;
    private float originRunBuildUpSpeed;
    private float originJumpSpeed;

    // Awake���� �����ӵ� �޾ƿ���
    //    originWalkSpeed = walkSpeed;
    //    originRunSpeed = runSpeed;
    //    originCrouchSpeed = crouchSpeed;
    //    originRunBuildUpSpeed = runBuildUpSpeed;
    //    originJumpSpeed = jumpSpeed;
    public void SpeedDown()
    {
        //movementSpeed /= 2;
        //walkSpeed = originWalkSpeed / 2;
        //runSpeed = originRunSpeed / 2;
        //crouchSpeed = originCrouchSpeed / 2;
        //runBuildUpSpeed = originRunBuildUpSpeed / 2;
        //jumpSpeed = originJumpSpeed / 2;
    }

    public void SpeedReset()
    {
        //walkSpeed = originWalkSpeed;
        //runSpeed = originRunSpeed;
        //crouchSpeed = originCrouchSpeed;
        //runBuildUpSpeed = originRunBuildUpSpeed;
        //jumpSpeed = originJumpSpeed;
    }

    public void OnStun(float time)
    {
        StartCoroutine("Stunned", time);
    }

    private IEnumerator Stunned(float time)
    {
        isStunned = true;

        yield return new WaitForSeconds(time);

        isStunned = false;
    }

    public IEnumerator OnBounce(Vector3 direction)
    {
        isStunned = true;

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

    public IEnumerator Shake(float time, float intensity, float speed)
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
}
