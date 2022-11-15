using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public float damagedown = 1;

    public Slider hpSlider;
    public Slider staSlider;

    [SerializeField] private Image bsImage;
 

    // [SerializeField] private Inventory inventory;

    private void Awake()
    {
        PlayerController.instance = this;
        init();
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

    }

    private void Update()
    {
        hpSlider.maxValue = PlayerData.instance.MaxHp;
        staSlider.maxValue = PlayerData.instance.DashGauge;

        hpSlider.value = currentHp;
        staSlider.value = currentDashGauge;
        //�ʴ� 6�� ������ ����
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
            if (!PlayerSoundSource.isPlaying)
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

        currentHp -= (damage - currentDp) * damagedown;
        Debug.Log("���� �÷��̾� ü��:" + currentHp);
        if (currentHp > 0)
        {
            StartCoroutine(ShowBloodScreen());
        }
        if (currentHp <= 0)
        {
            Debug.Log("����߽��ϴ�");
        }
    }

    private IEnumerator ShowBloodScreen()
    {
        bsImage.color = new Color(1, 0, 0, UnityEngine.Random.Range(0.2f, 0.3f));
        Debug.Log("BS On");
        yield return new WaitForSeconds(0.1f);
        bsImage.color = Color.clear;
        Debug.Log("BS Off");
    }

    // ���� ���� ��ȣ�ۿ�(?)
    private bool isStunned;  // ���Ϳ��� ���� ������ ī�޶� �� ������ ���� �Ұ�

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

    private IEnumerator OnBounce(Vector3 direction)
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
            charController.Move(direction * -10f * Time.deltaTime);
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
                Debug.Log(hitInfo.transform.GetComponet<ItemPickUp>().item.itemName + "ȹ�� �߽��ϴ�.");
                theInventory.AcquireItem(hitInfo.transform.GetComponet<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }
    */
}
