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

    public bool Scout_Drone = false;                    //시야내 적 확인
    public bool Scout_Raider = false;
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
        while (true)
        {
            Debug.Log("레이더");
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