using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public static ItemData instance;
    public bool Consum_Heal = false;
    public bool Bool_HealLobby = false;
    public bool Bool_HealInGame = false;

    public bool Consum_Grenade = false;
    //public bool Consum_Mine = false; 이펙트 없음
    //public bool Consum_PoisonMIne = false; 이펙트 없음
    public bool Consum_FlashGrenade = false;
    public bool Consum_FrozenGrenade = false;
    public bool Consum_ParalysGrenade = false;
    //public bool Consum_Indicator = false; 애니메이션 없음

    public bool Buff_IncreaseHp = false;
    public bool Buff_IncreaseDeffense = false;
    public bool Buff_IncreaseMagazine = false;
    public bool Buff_IncreaseAmmo = false;
    //public bool Buff_IncreaseHealEffect = false; 구현 가능?
    public bool Buff_IncreaseSpeed = false;
    public bool Buff_IncreaseDamage = false;
    public bool Buff_DecreaseReceivedDamage = false;

    public bool Scout_Drone = false;
    public bool Scout_Raider = false;
    //public bool Scout_Flare = false; 레이더와 겹침
    //public bool Scout_Telescope = false; 빌드 후 안개가 없어진 관계로 제외
    //public bool Scout_Tower = false; 정찰드론과 겹침
    //public bool Scout_Sonar = false; 정찰드론과 겹침
    //public bool Scout_NinjaKit = false; 빌드 후 안개가 없어진 관계로 제외
    public bool Scout_Movement = false;
    public bool Scout_Jumppad = false;
    public bool Scout_Thruster = false;
    public bool Scout_Jetpack = false;
    public bool Scout_Barricade = false;
    //public bool Scout_Anchor = false; 몬스터 안보여서 뺌
    //public bool Scout_Camouflage = false; 몬스터 안보여서 뺌
    public bool Scout_Drill = false;

    public int ThrusterTime = 3;



    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Consum_Heal)
        {
            Heal();
        }
        if (Consum_Grenade)
        {
            Grenade();
        }

        if (Buff_IncreaseHp)
        {
            IncreaseHp();
        }
        if (Buff_IncreaseDeffense)
        {
            IncreaseDeffense();
        }
        if (Buff_IncreaseMagazine)
        {
            IncreaseMagazine();
        }
        if (Buff_IncreaseAmmo)
        {
            IncreaseAmmo();
        }
        if (Buff_IncreaseSpeed)
        {
            IncreaseSpeed();
        }
        if (Buff_IncreaseDamage)
        {
            IncreaseDamage();
        }
        if (Buff_DecreaseReceivedDamage)
        {
            DecreaseReceivedDamage();
        }

        if (Scout_Drone)
        {
            Drone();
        }
        if (Scout_Raider)
        {
            Raider();
        }
        if (Scout_Movement)
        {
            Movement();
        }
        if (Scout_Jumppad)
        {
            Jumppad();
        }
        if (Scout_Thruster)
        {
            Debug.Log("추진기 작동");
            StartCoroutine("Thruster");
            Scout_Thruster = false;
        }
        if (Scout_Jetpack)
        {
            Jetpack();
        }
        if (Scout_Barricade)
        {
            Barricade();
        }
        if (Scout_Drill)
        {
            Drill();
        }
    }

    public void Heal()
    {
        if (Bool_HealLobby)
        {
            Debug.Log("체력 회복 - 로비");
            PlayerData.instance.currentHp = PlayerData.instance.MaxHp;
            Bool_HealLobby = false;
            Consum_Heal = false;
        }

        if (Bool_HealInGame)
        {
            Debug.Log("체력 회복 - 게임");
            PlayerController.instance.currentHp = PlayerData.instance.MaxHp;
            Bool_HealInGame = false;
            Consum_Heal = false;
        }
    }
    public void Grenade()
    {
        //애니메이션 grenade 실행
        //수류탄 오브젝트 플레이어 머리 우측 뒤에서 생성 + 수류탄은 중력 적용
        //수류탄 정면 상향 방향으로 이동
        //수류탄 폭발 판정 및 데미지 등은 수류탄.cs만들어서 따로 작업
    }

    public void IncreaseHp()
    {
        Debug.Log("체력 최대치 증가");
        PlayerData.instance.MaxHp += 10;
        PlayerData.instance.currentHp += 10;
        Buff_IncreaseHp = false;
    }
    public void IncreaseDeffense()
    {

    }
    public void IncreaseMagazine()
    {

    }
    public void IncreaseAmmo()
    {

    }
    public void IncreaseSpeed()
    {

    }
    public void IncreaseDamage()
    {
        Debug.Log("데미지 증가");
        Debug.Log("데미지 : " + GunController.instance.currentGun.damage);
        GunController.instance.currentGun.damage += 10;
        Debug.Log("증가된 데미지 : " + GunController.instance.currentGun.damage);
        ItemData.instance.Buff_IncreaseDamage = false;
    }
    public void DecreaseReceivedDamage()
    {

    }

    public void Drone()
    {

    }
    public void Raider()
    {

    }
    public void Movement()
    {

    }
    public void Jumppad()
    {

    }
    IEnumerator Thruster()
    {
        Debug.Log("추진기");
        int thrust = 0;
        while (true)
        {
            Debug.Log("추진");
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("클릭");
                Ray ray = MapManager.instance.mainCamera.ScreenPointToRay(Input.mousePosition);
                int layerMask = 1 << LayerMask.NameToLayer("Water");
                layerMask = ~layerMask;

                if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, layerMask))
                {
                    Debug.Log("이동");
                    string xy;
                    MapData.instance.TileDic.TryGetValue(raycastHit.transform.name, out xy);

                    string[] a = xy.Split(',');

                    int x = int.Parse(a[0].ToString());
                    int y = int.Parse(a[1].ToString());
                    MapManager.instance.PlayerMove(x, y);

                    thrust++;
                }
                if (thrust >= ThrusterTime)
                {
                    Debug.Log("탈출");
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Jetpack()
    {

    }
    public void Barricade()
    {

    }
    public void Anchor()
    {

    }
    public void Camouflage()
    {

    }
    public void Drill()
    {

    }
}
