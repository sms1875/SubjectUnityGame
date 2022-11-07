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
    //public bool Consum_Mine = false; ����Ʈ ����
    //public bool Consum_PoisonMIne = false; ����Ʈ ����
    public bool Consum_FlashGrenade = false;
    public bool Consum_FrozenGrenade = false;
    public bool Consum_ParalysGrenade = false;
    //public bool Consum_Indicator = false; �ִϸ��̼� ����

    public bool Buff_IncreaseHp = false;
    public bool Buff_IncreaseDeffense = false;
    public bool Buff_IncreaseMagazine = false;
    public bool Buff_IncreaseAmmo = false;
    //public bool Buff_IncreaseHealEffect = false; ���� ����?
    public bool Buff_IncreaseSpeed = false;
    public bool Buff_IncreaseDamage = false;
    public bool Buff_DecreaseReceivedDamage = false;

    public bool Scout_Drone = false;                    //�þ߳� �� Ȯ��
    public bool Scout_Raider = false;
    //public bool Scout_Flare = false; ���̴��� ��ħ
    //public bool Scout_Telescope = false; ���� �� �Ȱ��� ������ ����� ����
    //public bool Scout_Tower = false; ������а� ��ħ
    //public bool Scout_Sonar = false; ������а� ��ħ
    //public bool Scout_NinjaKit = false; ���� �� �Ȱ��� ������ ����� ����
    public bool Scout_Thruster = false;                 //�ϼҸ� ���� �̵��Ÿ� nĭ ����
    public bool Scout_Jumppad = false;                  //���ʸ� �̵�
    //public bool Scout_Movement = false; ��Ʈ�Ѱ� ��ħ
    public bool Scout_Jetpack = false;                  //�̵��Ÿ� ����
    public bool Scout_Barricade = false;                //�ٸ�����Ʈ��ġ
    //public bool Scout_Anchor = false; ���� �Ⱥ����� ��
    //public bool Scout_Camouflage = false; ���� �Ⱥ����� ��
    public bool Scout_Drill = false;                    //�ٸ�����Ʈ ����

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
            Debug.Log("��� �۵�");
            Drone();
            Scout_Drone = false;
        }
        if (Scout_Raider)
        {
            Debug.Log("���̴� �۵�");
            StartCoroutine("Raider");
            Scout_Raider = false;
        }
        if (Scout_Thruster)
        {
            Debug.Log("������ �۵�");
            StartCoroutine("Thruster");
            Scout_Thruster = false;
        }
        if (Scout_Jumppad)
        {
            Debug.Log("�����е� �۵�");
            StartCoroutine("Jumppad");
            Scout_Jumppad = false;
            Jump = false;
        }
        if (Scout_Jetpack)
        {
            Debug.Log("��Ʈ�� �۵�");
            Jetpack();
            Scout_Jetpack = false;
        }
        if (Scout_Barricade)
        {
            Debug.Log("�ٸ����̵� �۵�");
            StartCoroutine("Barricade");
            Scout_Barricade = false;
        }
        if (Scout_Drill)
        {
            Debug.Log("�帱 �۵�");
            StartCoroutine("Drill");
            Scout_Drill = false;
        }
    }

    public void Heal()
    {
        if (Bool_HealLobby)
        {
            Debug.Log("ü�� ȸ�� - �κ�");
            PlayerData.instance.currentHp = PlayerData.instance.MaxHp;
            Bool_HealLobby = false;
            Consum_Heal = false;
        }

        if (Bool_HealInGame)
        {
            Debug.Log("ü�� ȸ�� - ����");
            PlayerController.instance.currentHp = PlayerData.instance.MaxHp;
            Bool_HealInGame = false;
            Consum_Heal = false;
        }
    }
    public void Grenade()
    {
        //�ִϸ��̼� grenade ����
        //����ź ������Ʈ �÷��̾� �Ӹ� ���� �ڿ��� ���� + ����ź�� �߷� ����
        //����ź ���� ���� �������� �̵�
        //����ź ���� ���� �� ������ ���� ����ź.cs���� ���� �۾�
    }

    public void IncreaseHp()
    {
        Debug.Log("ü�� �ִ�ġ ����");
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
        Debug.Log("������ ����");
        Debug.Log("������ : " + GunController.instance.currentGun.damage);
        GunController.instance.currentGun.damage += 10;
        Debug.Log("������ ������ : " + GunController.instance.currentGun.damage);
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
                            Debug.Log("����Ʈ ���� " + x + ", " + y);
                        }
                        else if (MapData.instance._tile[x, y] == TileType.Enemy_Normal || MapData.instance._tile[x, y] == TileType.Event)//�ӽ÷� �̺�Ʈ�� ����
                        {
                            Debug.Log("�Ϲ� ���� " + x + ", " + y);
                        }
                        else
                        {
                            Debug.Log("�������" + x + ", " + y);
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
            Debug.Log("���̴�");
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("���̴� Ŭ��");
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

                    Debug.Log("�߽�" + x + ", " + y);

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
                                        Debug.Log("����Ʈ ���� " + rx + ", " + ry);
                                    }
                                    else if (MapData.instance._tile[rx, ry] == TileType.Enemy_Normal || MapData.instance._tile[rx, ry] == TileType.Event)//�ӽ÷� �̺�Ʈ�� ����
                                    {
                                        Debug.Log("�Ϲ� ���� " + rx + ", " + ry);
                                    }
                                    else
                                    {
                                        Debug.Log("�������" + rx + ", " + ry);
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
        Debug.Log("������ �غ�");
        int thrust = 0;
        while (true)
        {
            Debug.Log("������");
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("������ Ŭ��");
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
                        Debug.Log("������ �̵�");
                        MapManager.instance.PlayerMove(x, y);

                        thrust++;
                    }
                    else
                    {
                        Debug.Log("������ �̵��Ұ�");
                    }
                }
                if (thrust >= ThrusterTime)
                {
                    Debug.Log("������ ����");
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator Jumppad()
    {
        Debug.Log("�����е� �غ�");
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("�����е� Ŭ��");
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
                        Debug.Log("�����е� ���");
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
                        Debug.Log("�����е� ���-�� �̵�");
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
        Debug.Log("�ٸ����̵� ��ġ �غ�");
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("�ٸ����̵� Ŭ��");
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

                    Debug.Log("�ٸ����̵� ��ġ");
                    //�� ������Ʈ ����
                    MapData.instance._tile[x, y] = TileType.Wall;
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator Drill()
    {
        Debug.Log("�帱 �۵� �غ�");
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("�帱 Ŭ��");
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

                    Debug.Log("�� �ı�");
                    //�� ������Ʈ ����
                    MapData.instance._tile[x, y] = TileType.Empty;
                    break;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}