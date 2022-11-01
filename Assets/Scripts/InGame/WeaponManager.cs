using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponManager : MonoBehaviour
{
    /*
    // ���� �ߺ� ��ü ���� ����.
    public static bool isChangeWeapon = false;

    // ���� ����� ���� ������ �ִϸ��̼�.
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    [SerializeField] private float changeWeaponDelayTime;//���� �ִ� �ð�
    [SerializeField] private float changeWeaponEndDelayTime;//���� ������ �ð�
    //private float changeWeaponTime;

    // ���� ������ ���� ����.
    [SerializeField] private Gun[] guns;

    // ���� �������� ���� ���� ������ �����ϵ��� ����.
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();

    // �ʿ��� ������Ʈ.
    private GunController theGunController;


    private void Awake()
    {        

        theGunController = FindObjectOfType<GunController>();
    }
    void Start()
    {
        //���� �߰�
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
    }

    void Update()
    {
        if (!isChangeWeapon && !theGunController.GetGunFire())
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeapon.gameObject.name != "AR1")
                StartCoroutine(ChangeWeaponCoroutine("GUN", "AR1"));
            else if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeapon.gameObject.name != "AR2")
                StartCoroutine(ChangeWeaponCoroutine("GUN", "AR2"));
        }
        if (!Inventory.inventoryActivated)
        {
            if (!isChangeWeapon)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1)) StartCoroutine(ChangeWeaponCoroutine());
            }
        }
        //changeGunCalc();
    }

    /* private void changeGunCalc()//���� ��ü ������
     {
         if (changeWeaponTime > 0)
         {
             changeWeaponTime -= Time.deltaTime / 2;
         }
     }*/
    /*
    // ���� ��ü �ڷ�ƾ.
    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;

        currentWeaponAnim.SetBool("Change",true);

        CancelPreWeaponAction();

        yield return new WaitForSeconds(changeWeaponDelayTime);//���� �ִ� ������

        if (currentWeapon != null)//���� ���� ��Ȱ��ȭ
            currentWeapon.gameObject.SetActive(false);

        WeaponChange(_type, _name);//��Ʈ�ѷ����� ���� ���� ��ü

        currentWeapon = theGunController.GetGun().GetComponent<Transform>();//��ü�� ���� Ȱ��ȭ
        currentWeaponAnim.SetBool("Change", false);//��ü �ִϸ��̼� ����


        currentWeaponAnim.SetTrigger(_name);//��ü�� �� �ִϸ��̼� ����

        yield return new WaitForSeconds(changeWeaponEndDelayTime);//���� ������ ������

        theGunController.GetGun().gameObject.SetActive(true);//������Ʈ Ȱ��ȭ
 
        isChangeWeapon = false;       
        GunController.isActivate = true;//���� Ȱ��ȭ
        //changeWeaponTime = changeWeaponDelayTime+ changeWeaponEndDelayTime+0.1f;//�� ��ü ������
    }

    // ���� �ൿ ��� �Լ�.
    private void CancelPreWeaponAction()
    {
        theGunController.CancelReload();
        theGunController.setActivate (false);
    }

    // ���� ��ü �Լ�.
    private void WeaponChange(string _type, string _name)
    {
        theGunController.GunChange(gunDictionary[_name]);
    }
    */
}