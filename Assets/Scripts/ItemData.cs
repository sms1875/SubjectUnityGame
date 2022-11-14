using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public static ItemData instance;
    public bool Consum_Heal_1 = false;                  //��� ȸ�� ������ ���
    public bool Consum_Heal_2 = false;                  //���� ȸ�� ������ ���
    //public bool Consum_Grenade = false;
    //public bool Consum_Mine = false; ����Ʈ ����
    //public bool Consum_PoisonMIne = false; ����Ʈ ����
    //public bool Consum_FlashGrenade = false;
    //public bool Consum_FrozenGrenade = false;
    //public bool Consum_ParalysGrenade = false;
    //public bool Consum_Indicator = false; �ִϸ��̼� ����

    public bool Buff_IncreaseHp = false;                //�ִ� ü�� ����
    public bool Buff_IncreaseDeffense = false;          //���� ����
    public bool Buff_IncreaseSpeed = false;             //�̵��ӵ� ����
    public bool Buff_DecreaseReceivedDamage = false;    //�޴µ����� ����
    public bool Buff_IncreaseMagazine = false;          //źâ �뷮 ����
    public bool Buff_IncreaseAmmo = false;              //ź�� ����
    public bool Buff_IncreaseDamage = false;            //������ ����
    //public bool Buff_IncreaseHealEffect = false; ���� ����?


    public bool Scout_Drone = false;                    //�þ߳� �� Ȯ��
    public bool Scout_Raider = false;                   //���� ���� �� �� Ȯ��
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
            Debug.Log("ü�� ��� ȸ��");
            Heal_1();
            Consum_Heal_1 = false;
        }
        if (Consum_Heal_2)
        {
            Debug.Log("ü�� ���� ȸ��");
            StartCoroutine("Heal_2");
            Consum_Heal_2 = false;
        }



        if (Buff_IncreaseHp)
        {
            Debug.Log("���� ü�� ���� �۵�");
            IncreaseHp();
            Buff_IncreaseHp = false;
        }
        if (Buff_IncreaseDeffense)
        {
            Debug.Log("���� ���� ���� �۵�");
            IncreaseDeffense();
            Buff_IncreaseDeffense = false;
        }
        if (Buff_IncreaseSpeed)
        {
            Debug.Log("�̵��ӵ� ���� �۵�");
            IncreaseSpeed();
            Buff_IncreaseSpeed = false;
        }
        if (Buff_DecreaseReceivedDamage)
        {
            Debug.Log("�޴µ����� ���� �۵�");
            DecreaseReceivedDamage();
            Buff_DecreaseReceivedDamage = false;
        }
        if (Buff_IncreaseMagazine)
        {
            Debug.Log("źâ �뷮 ����");
            IncreaseMagazine();
            Buff_IncreaseMagazine = false;
        }
        if (Buff_IncreaseAmmo)
        {
            Debug.Log("ź�� ����");
            IncreaseAmmo();
            Buff_IncreaseAmmo = false;
        }
        if (Buff_IncreaseDamage)
        {
            Debug.Log("������ ����");
            IncreaseDamage();
            Buff_IncreaseDamage = false;
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

    public void Heal_1()
    {
        Debug.Log("ü�� ��� ȸ��");
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
            Debug.Log("ü�� ���� ȸ��");
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
        Debug.Log("ü�� �ִ�ġ ����");
        float beforeHP = PlayerData.instance.MaxHp;
        PlayerData.instance.MaxHp = beforeHP * 1.2f;
        PlayerData.instance.currentHp += beforeHP * 0.2f;
        upHP = beforeHP * 0.2f;
    }
    public void AfterHP()
    {
        Debug.Log("ü�� �ִ�ġ ���� ����");
        PlayerData.instance.MaxHp -= upHP;
        if (PlayerData.instance.MaxHp < PlayerData.instance.currentHp)
        {
            PlayerData.instance.currentHp = PlayerData.instance.MaxHp;
        }
        upHP = 0;
    }

    public void IncreaseDeffense()
    {
        Debug.Log("���� �ִ�ġ ����");
        float beforeDP = PlayerData.instance.MaxDp;
        PlayerData.instance.MaxDp = beforeDP * 1.1f;
        PlayerData.instance.currentDp += beforeDP * 0.1f;
        upDP = beforeDP * 0.2f;
    }
    public void AfterDeffense()
    {
        Debug.Log("���� �ִ�ġ ���� ����");
        PlayerData.instance.MaxDp -= upDP;
        if (PlayerData.instance.MaxDp < PlayerData.instance.currentDp)
        {
            PlayerData.instance.currentDp = PlayerData.instance.MaxDp;
        }
        upDP = 0;
    }


    public void IncreaseSpeed()
    {
        Debug.Log("�̵��ӵ� ����");
        upSP = 1.2f;
    }
    public void AfterSpeed()
    {
        Debug.Log("�̵��ӵ� ���� ����");
        upSP = 1.0f;
    }

    public void DecreaseReceivedDamage()
    {
        Debug.Log("����������");
        PlayerController.instance.damagedown = 0.9f;
    }
    public void AfterDRD()
    {
        Debug.Log("����������");
        PlayerController.instance.damagedown = 1.0f;
    }

    public void IncreaseDamage()
    {
        Debug.Log("������ ����");
        damage = true;
    }
    public void AfterDamage()
    {
        Debug.Log("������ ���� ����");
        damage = false;
    }

    public void IncreaseMagazine()
    {
        Debug.Log("źâ ����");
        magazine = true;
    }
    public void AfterMagazine()
    {
        Debug.Log("źâ ���� ����");
        magazine = false;
    }

    public void IncreaseAmmo()
    {
        Debug.Log("ź�� ����");
        ammo = true;
    }
    public void AfterAmmo()
    {
        Debug.Log("ź�� ���� ����");
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
        Debug.Log("���̴�");
        while (true)
        {
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