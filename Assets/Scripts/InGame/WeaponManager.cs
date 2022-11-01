using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponManager : MonoBehaviour
{
    /*
    // 무기 중복 교체 실행 방지.
    public static bool isChangeWeapon = false;

    // 현재 무기와 현재 무기의 애니메이션.
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    [SerializeField] private float changeWeaponDelayTime;//무기 넣는 시간
    [SerializeField] private float changeWeaponEndDelayTime;//무기 꺼내는 시간
    //private float changeWeaponTime;

    // 무기 종류들 전부 관리.
    [SerializeField] private Gun[] guns;

    // 관리 차원에서 쉽게 무기 접근이 가능하도록 만듦.
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();

    // 필요한 컴포넌트.
    private GunController theGunController;


    private void Awake()
    {        

        theGunController = FindObjectOfType<GunController>();
    }
    void Start()
    {
        //무기 추가
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

    /* private void changeGunCalc()//무기 교체 딜레이
     {
         if (changeWeaponTime > 0)
         {
             changeWeaponTime -= Time.deltaTime / 2;
         }
     }*/
    /*
    // 무기 교체 코루틴.
    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;

        currentWeaponAnim.SetBool("Change",true);

        CancelPreWeaponAction();

        yield return new WaitForSeconds(changeWeaponDelayTime);//무기 넣는 딜레이

        if (currentWeapon != null)//현재 무기 비활성화
            currentWeapon.gameObject.SetActive(false);

        WeaponChange(_type, _name);//컨트롤러에서 현재 무기 교체

        currentWeapon = theGunController.GetGun().GetComponent<Transform>();//교체한 무기 활성화
        currentWeaponAnim.SetBool("Change", false);//교체 애니메이션 종료


        currentWeaponAnim.SetTrigger(_name);//교체한 총 애니메이션 설정

        yield return new WaitForSeconds(changeWeaponEndDelayTime);//무기 꺼내는 딜레이

        theGunController.GetGun().gameObject.SetActive(true);//오브젝트 활성화
 
        isChangeWeapon = false;       
        GunController.isActivate = true;//무기 활성화
        //changeWeaponTime = changeWeaponDelayTime+ changeWeaponEndDelayTime+0.1f;//총 교체 딜레이
    }

    // 무기 행동 취소 함수.
    private void CancelPreWeaponAction()
    {
        theGunController.CancelReload();
        theGunController.setActivate (false);
    }

    // 무기 교체 함수.
    private void WeaponChange(string _type, string _name)
    {
        theGunController.GunChange(gunDictionary[_name]);
    }
    */
}