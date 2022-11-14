using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public static ItemData instance;
    public bool Consum_Heal_1 = false;                  //즉시 회복 아이템 사용
    public bool Consum_Heal_2 = false;                  //지속 회복 아이템 사용
    //public bool Consum_Grenade = false;
    //public bool Consum_Mine = false; 이펙트 없음
    //public bool Consum_PoisonMIne = false; 이펙트 없음
    //public bool Consum_FlashGrenade = false;
    //public bool Consum_FrozenGrenade = false;
    //public bool Consum_ParalysGrenade = false;
    //public bool Consum_Indicator = false; 애니메이션 없음

    public bool Buff_IncreaseHp = false;                //최대 체력 증가
    public bool Buff_IncreaseDeffense = false;          //방어력 증가
    public bool Buff_IncreaseSpeed = false;             //이동속도 증가
    public bool Buff_DecreaseReceivedDamage = false;    //받는데미지 감소
    public bool Buff_IncreaseMagazine = false;          //탄창 용량 증가
    public bool Buff_IncreaseAmmo = false;              //탄약 증가
    public bool Buff_IncreaseDamage = false;            //데미지 증가
    //public bool Buff_IncreaseHealEffect = false; 구현 가능?


    public bool Scout_Drone = false;                    //시야내 적 확인
    public bool Scout_Raider = false;                   //일정 범위 내 적 확인
    //public bool Scout_Flare = false; 레이더와 겹침
    //public bool Scout_Telescope = false; 빌드 후 안개가 없어진 관계로 제외
    //public bool Scout_Tower = false; 정찰드론과 겹침
    //public bool Scout_Sonar = false; 정찰드론과 겹침
    //public bool Scout_NinjaKit = false; 빌드 후 안개가 없어진 관계로 제외
    public bool Scout_Thruster = false;                 //턴소모 없이 이동거리 n칸 증가
    public bool Scout_Jumppad = false;                  //벽너머 이동
    //public bool Scout_Movement = false; 제트팩과 겹침
    public bool Scout_Jetpack = false;                  //이동거리 증가
    public bool Scout_Barricade = false;                //바리게이트설치
    //public bool Scout_Anchor = false; 몬스터 안보여서 뺌
    //public bool Scout_Camouflage = false; 몬스터 안보여서 뺌
    public bool Scout_Drill = false;                    //바리게이트 제거

    public int ThrusterTime = 3;
    public bool Jump = false;
    public float upHP = 0f;
    public float upDP = 0f;
    public float upSP = 1.0f;
    public int heal2count = 0;

    public bool ammo = false;
    public bool magazine = false;
    public bool damage = false;




    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Consum_Heal_1)
        {
            Debug.Log("체력 즉시 회복");
            Heal_1();
            Consum_Heal_1 = false;
        }
        if (Consum_Heal_2)
        {
            Debug.Log("체력 지속 회복");
            StartCoroutine("Heal_2");
            Consum_Heal_2 = false;
        }



        if (Buff_IncreaseHp)
        {
            Debug.Log("버프 체력 증가 작동");
            IncreaseHp();
            Buff_IncreaseHp = false;
        }
        if (Buff_IncreaseDeffense)
        {
            Debug.Log("버프 방어력 증가 작동");
            IncreaseDeffense();
            Buff_IncreaseDeffense = false;
        }
        if (Buff_IncreaseSpeed)
        {
            Debug.Log("이동속도 증가 작동");
            IncreaseSpeed();
            Buff_IncreaseSpeed = false;
        }
        if (Buff_DecreaseReceivedDamage)
        {
            Debug.Log("받는데미지 감소 작동");
            DecreaseReceivedDamage();
            Buff_DecreaseReceivedDamage = false;
        }
        if (Buff_IncreaseMagazine)
        {
            Debug.Log("탄창 용량 증가");
            IncreaseMagazine();
            Buff_IncreaseMagazine = false;
        }
        if (Buff_IncreaseAmmo)
        {
            Debug.Log("탄약 증가");
            IncreaseAmmo();
            Buff_IncreaseAmmo = false;
        }
        if (Buff_IncreaseDamage)
        {
            Debug.Log("데미지 증가");
            IncreaseDamage();
            Buff_IncreaseDamage = false;
        }



        if (Scout_Drone)
        {
            Debug.Log("드론 작동");
            Drone();
            Scout_Drone = false;
        }
        if (Scout_Raider)
        {
            Debug.Log("레이더 작동");
            StartCoroutine("Raider");
            Scout_Raider = false;
        }
        if (Scout_Thruster)
        {
            Debug.Log("추진기 작동");
            StartCoroutine("Thruster");
            Scout_Thruster = false;
        }
        if (Scout_Jumppad)
        {
            Debug.Log("점프패드 작동");
            StartCoroutine("Jumppad");
            Scout_Jumppad = false;
            Jump = false;
        }
        if (Scout_Jetpack)
        {
            Debug.Log("제트팩 작동");
            Jetpack();
            Scout_Jetpack = false;
        }
        if (Scout_Barricade)
        {
            Debug.Log("바리케이드 작동");
            StartCoroutine("Barricade");
            Scout_Barricade = false;
        }
        if (Scout_Drill)
        {
            Debug.Log("드릴 작동");
            StartCoroutine("Drill");
            Scout_Drill = false;
        }
    }

    public void Heal_1()
    {
        Debug.Log("체력 즉시 회복");
        PlayerData.instance.currentHp += PlayerData.instance.MaxHp*0.3f;
        if(PlayerData.instance.currentHp > PlayerData.instance.MaxHp)
        {
            PlayerData.instance.currentHp = PlayerData.instance.MaxHp;
        }
    }
    IEnumerator Heal_2()
    {
        while (true)
        {
            Debug.Log("체력 지속 회복");
            PlayerData.instance.currentHp += PlayerData.instance.MaxHp * 0.05f;
            heal2count += 1;
            if (PlayerData.instance.currentHp > PlayerData.instance.MaxHp)
            {
                PlayerData.instance.currentHp = PlayerData.instance.MaxHp;
            }
            if(heal2count > 10)
            {
                heal2count = 0;
                break;
            }
            yield return new WaitForSeconds(2f);
        }
    }



    public void IncreaseHp()
    {
        Debug.Log("체력 최대치 증가");
        float beforeHP = PlayerData.instance.MaxHp;
        PlayerData.instance.MaxHp = beforeHP * 1.2f;
        PlayerData.instance.currentHp += beforeHP * 0.2f;
        upHP = beforeHP * 0.2f;
    }
    public void AfterHP()
    {
        Debug.Log("체력 최대치 버프 종료");
        PlayerData.instance.MaxHp -= upHP;
        if (PlayerData.instance.MaxHp < PlayerData.instance.currentHp)
        {
            PlayerData.instance.currentHp = PlayerData.instance.MaxHp;
        }
        upHP = 0;
    }

    public void IncreaseDeffense()
    {
        Debug.Log("방어력 최대치 증가");
        float beforeDP = PlayerData.instance.MaxDp;
        PlayerData.instance.MaxDp = beforeDP * 1.1f;
        PlayerData.instance.currentDp += beforeDP * 0.1f;
        upDP = beforeDP * 0.2f;
    }
    public void AfterDeffense()
    {
        Debug.Log("방어력 최대치 버프 종료");
        PlayerData.instance.MaxDp -= upDP;
        if (PlayerData.instance.MaxDp < PlayerData.instance.currentDp)
        {
            PlayerData.instance.currentDp = PlayerData.instance.MaxDp;
        }
        upDP = 0;
    }


    public void IncreaseSpeed()
    {
        Debug.Log("이동속도 증가");
        upSP = 1.2f;
    }
    public void AfterSpeed()
    {
        Debug.Log("이동속도 버프 종료");
        upSP = 1.0f;
    }

    public void DecreaseReceivedDamage()
    {
        Debug.Log("데미지감소");
        PlayerController.instance.damagedown = 0.9f;
    }
    public void AfterDRD()
    {
        Debug.Log("데미지감소");
        PlayerController.instance.damagedown = 1.0f;
    }

    public void IncreaseDamage()
    {
        Debug.Log("데미지 증가");
        damage = true;
    }
    public void AfterDamage()
    {
        Debug.Log("데미지 버프 증가");
        damage = false;
    }

    public void IncreaseMagazine()
    {
        Debug.Log("탄창 증가");
        magazine = true;
    }
    public void AfterMagazine()
    {
        Debug.Log("탄창 증가 종료");
        magazine = false;
    }

    public void IncreaseAmmo()
    {
        Debug.Log("탄약 증가");
        ammo = true;
    }
    public void AfterAmmo()
    {
        Debug.Log("탄약 증가 종료");
        ammo = false;
    }



    public void Drone()
    {
        int px = MapManager.instance.playerNowPoint_X;
        int py = MapManager.instance.playerNowPoint_Y;
        for (int x = px-1; x <= px + 1; x++)
        {
            if (x < 0 || x > MapManager.instance.size)
            {
                Debug.Log("xover");
            }
            else
            {
                for (int y = py - 1; y <= py + 1; y++)
                {
                    if (y < 0 || y > MapManager.instance.size)
                    {
                        Debug.Log("yover");
                    }
                    else
                    {
                        if (MapData.instance._tile[x, y] == TileType.Enemy_Elite)
                        {
                            Debug.Log("엘리트 몬스터 " + x + ", " + y);
                        }
                        else if (MapData.instance._tile[x, y] == TileType.Enemy_Normal || MapData.instance._tile[x, y] == TileType.Event)//임시로 이벤트도 포함
                        {
                            Debug.Log("일반 몬스터 " + x + ", " + y);
                        }
                        else
                        {
                            Debug.Log("비어있음" + x + ", " + y);
                        }
                    }
                }
            }
        }
    }
    IEnumerator Raider()
    {
        Debug.Log("레이더");
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("레이더 클릭");
                Ray ray = MapManager.instance.mainCamera.ScreenPointToRay(Input.mousePosition);
                int layerMask = 1 << LayerMask.NameToLayer("Water");
                layerMask = ~layerMask;

                if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, layerMask))
                {
                    string xy;
                    MapData.instance.TileDic.TryGetValue(raycastHit.transform.name, out xy);

                    string[] a = xy.Split(',');

                    int x = int.Parse(a[0].ToString());
                    int y = int.Parse(a[1].ToString());

                    Debug.Log("중심" + x + ", " + y);

                    for (int rx = x - 1; rx <= x + 1; rx++)
                    {
                        if (rx < 0 || rx > MapManager.instance.size)
                        {
                            Debug.Log("xover");
                        }
                        else
                        {
                            for (int ry = y - 1; ry <= y + 1; ry++)
                            {
                                if (ry < 0 || ry > MapManager.instance.size)
                                {
                                    Debug.Log("yover");
                                }
                                else
                                {
                                    if (MapData.instance._tile[rx, ry] == TileType.Enemy_Elite)
                                    {
                                        Debug.Log("엘리트 몬스터 " + rx + ", " + ry);
                                    }
                                    else if (MapData.instance._tile[rx, ry] == TileType.Enemy_Normal || MapData.instance._tile[rx, ry] == TileType.Event)//임시로 이벤트도 포함
                                    {
                                        Debug.Log("일반 몬스터 " + rx + ", " + ry);
                                    }
                                    else
                                    {
                                        Debug.Log("비어있음" + rx + ", " + ry);
                                    }
                                }
                            }
                        }
                    }

                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator Thruster()
    {
        Debug.Log("추진기 준비");
        int thrust = 0;
        while (true)
        {
            Debug.Log("추진중");
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("추진기 클릭");
                Ray ray = MapManager.instance.mainCamera.ScreenPointToRay(Input.mousePosition);
                int layerMask = 1 << LayerMask.NameToLayer("Water");
                layerMask = ~layerMask;

                if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, layerMask))
                {
                    string xy;
                    MapData.instance.TileDic.TryGetValue(raycastHit.transform.name, out xy);

                    string[] a = xy.Split(',');

                    int x = int.Parse(a[0].ToString());
                    int y = int.Parse(a[1].ToString());

                    if (MapData.instance._tile[x, y] != TileType.Wall)
                    {
                        Debug.Log("추진기 이동");
                        MapManager.instance.PlayerMove(x, y);

                        thrust++;
                    }
                    else
                    {
                        Debug.Log("추진기 이동불가");
                    }
                }
                if (thrust >= ThrusterTime)
                {
                    Debug.Log("추진기 종료");
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator Jumppad()
    {
        Debug.Log("점프패드 준비");
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("점프패드 클릭");
                Ray ray = MapManager.instance.mainCamera.ScreenPointToRay(Input.mousePosition);
                int layerMask = 1 << LayerMask.NameToLayer("Water");
                layerMask = ~layerMask;

                if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, layerMask))
                {
                    string xy;
                    MapData.instance.TileDic.TryGetValue(raycastHit.transform.name, out xy);

                    string[] a = xy.Split(',');

                    int x = int.Parse(a[0].ToString());
                    int y = int.Parse(a[1].ToString());

                    if (MapData.instance._tile[x, y] != TileType.Wall)
                    {
                        Debug.Log("점프패드 사용");
                        MapManager.instance.PlayerMove(x, y);
                        break;
                    }
                    else
                    {
                        int relx = x - MapManager.instance.playerNowPoint_X;
                        int rely = y - MapManager.instance.playerNowPoint_Y;
                        if (relx > 0)
                        {
                            x++;
                        }
                        else if (relx < 0)
                        {
                            x--;
                        }
                        if (rely > 0)
                        {
                            y++;
                        }
                        else if (rely < 0)
                        {
                            y--;
                        }
                        Debug.Log("점프패드 사용-벽 이동");
                        Jump = true;
                        MapManager.instance.PlayerMove(x, y);
                        break;
                    }

                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    public void Jetpack()
    {
        MapManager.instance.movecount = 1;
    }
    IEnumerator Barricade()
    {
        Debug.Log("바리케이드 설치 준비");
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("바리케이드 클릭");
                Ray ray = MapManager.instance.mainCamera.ScreenPointToRay(Input.mousePosition);
                int layerMask = 1 << LayerMask.NameToLayer("Water");
                layerMask = ~layerMask;

                if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, layerMask))
                {
                    string xy;
                    MapData.instance.TileDic.TryGetValue(raycastHit.transform.name, out xy);

                    string[] a = xy.Split(',');

                    int x = int.Parse(a[0].ToString());
                    int y = int.Parse(a[1].ToString());

                    Debug.Log("바리케이드 설치");
                    //벽 오브젝트 생성
                    MapData.instance._tile[x, y] = TileType.Wall;
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator Drill()
    {
        Debug.Log("드릴 작동 준비");
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("드릴 클릭");
                Ray ray = MapManager.instance.mainCamera.ScreenPointToRay(Input.mousePosition);
                int layerMask = 1 << LayerMask.NameToLayer("Water");
                layerMask = ~layerMask;

                if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, layerMask))
                {
                    string xy;
                    MapData.instance.TileDic.TryGetValue(raycastHit.transform.name, out xy);

                    string[] a = xy.Split(',');

                    int x = int.Parse(a[0].ToString());
                    int y = int.Parse(a[1].ToString());

                    Debug.Log("벽 파괴");
                    //벽 오브젝트 제거
                    MapData.instance._tile[x, y] = TileType.Empty;
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}