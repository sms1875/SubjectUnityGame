using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMod : MonoBehaviour
{
    //�Է��� ������ ���� ����� ����
    public static bool isDebug;

    public Gun[] Guns;

    private void Awake()
    {
        var obj = FindObjectsOfType<DebugMod>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // '`'Ű�� ������ ��, ����� �г� ���� �ݱ�
        SetDebugMod();
        // ����� ������ ��, Ű �Է� �ޱ�
        if (isDebug)
        {
            if(Input.GetKey(KeyCode.LeftAlt)|| Input.GetKey(KeyCode.LeftShift))
            {
                SetGun();
                return;
            }
            Item();
            Event();
            Ability();
        }
    }
    void SetGun()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (PlayerData.instance.currentGunList[1] == Guns[0]) return;

            PlayerData.instance.currentGunList[0] = Guns[0];
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (PlayerData.instance.currentGunList[1] == Guns[1]) return;

            PlayerData.instance.currentGunList[0] = Guns[1];
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (PlayerData.instance.currentGunList[1] == Guns[2]) return;

            PlayerData.instance.currentGunList[0] = Guns[2];
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (PlayerData.instance.currentGunList[1] == Guns[3]) return;

            PlayerData.instance.currentGunList[0] = Guns[3];
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (PlayerData.instance.currentGunList[1] == Guns[4]) return;

            PlayerData.instance.currentGunList[0] = Guns[4];
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (PlayerData.instance.currentGunList[1] == Guns[5]) return;

            PlayerData.instance.currentGunList[0] = Guns[5];
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (PlayerData.instance.currentGunList[1] == Guns[6]) return;

            PlayerData.instance.currentGunList[0] = Guns[6];
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (PlayerData.instance.currentGunList[1] == Guns[7]) return;

            PlayerData.instance.currentGunList[0] = Guns[7];
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (PlayerData.instance.currentGunList[1] == Guns[8]) return;

            PlayerData.instance.currentGunList[0] = Guns[8];
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (PlayerData.instance.currentGunList[1] == Guns[9]) return;

            PlayerData.instance.currentGunList[0] = Guns[9];
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (PlayerData.instance.currentGunList[0] == Guns[0]) return;

            PlayerData.instance.currentGunList[1] = Guns[0];
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (PlayerData.instance.currentGunList[0] == Guns[1]) return;

            PlayerData.instance.currentGunList[1] = Guns[1];
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (PlayerData.instance.currentGunList[0] == Guns[2]) return;

            PlayerData.instance.currentGunList[1] = Guns[2];
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (PlayerData.instance.currentGunList[0] == Guns[3]) return;

            PlayerData.instance.currentGunList[1] = Guns[3];
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (PlayerData.instance.currentGunList[0] == Guns[4]) return;

            PlayerData.instance.currentGunList[1] = Guns[4];
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (PlayerData.instance.currentGunList[0] == Guns[5]) return;

            PlayerData.instance.currentGunList[1] = Guns[5];
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (PlayerData.instance.currentGunList[0] == Guns[6]) return;

            PlayerData.instance.currentGunList[1] = Guns[6];
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (PlayerData.instance.currentGunList[0] == Guns[7]) return;

            PlayerData.instance.currentGunList[1] = Guns[7];
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (PlayerData.instance.currentGunList[0] == Guns[8]) return;

            PlayerData.instance.currentGunList[1] = Guns[8];
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (PlayerData.instance.currentGunList[0] == Guns[9]) return;

            PlayerData.instance.currentGunList[1] = Guns[9];
        }
    }

    private void SetDebugMod()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (isDebug)
            {
                ClosePannel();
            }
            else
            {
                OpenPannel();
            }
        }
    }

    private void Item()
    {
        if (Input.GetKeyDown("k"))//����׿� ������ ���⿡ ������ ������� ���� Ʈ���Ű� �ð�
        {
            Debug.Log("������ ����� �غ�");
            ItemData.instance.Scout_Thruster = true;
        }

        if (Input.GetKeyDown("j"))//����׿� ������ ���⿡ ������ ������� ���� Ʈ���Ű� �ð�
        {
            Debug.Log("��Ʈ�� �����");
            ItemData.instance.Scout_Jetpack = true;
        }

        if (Input.GetKeyDown("h"))//����׿� ������ ���⿡ ������ ������� ���� Ʈ���Ű� �ð�
        {
            Debug.Log("�ٸ����̵� �����");
            ItemData.instance.Scout_Barricade = true;
        }
        if (Input.GetKeyDown("y"))//����׿� ������ ���⿡ ������ ������� ���� Ʈ���Ű� �ð�
        {
            Debug.Log("�帱 �����");
            ItemData.instance.Scout_Drill = true;
        }
        if (Input.GetKeyDown("u"))//����׿� ������ ���⿡ ������ ������� ���� Ʈ���Ű� �ð�
        {
            Debug.Log("�����е� �����");
            ItemData.instance.Scout_Jumppad = true;
        }
        if (Input.GetKeyDown("i"))//����׿� ������ ���⿡ ������ ������� ���� Ʈ���Ű� �ð�
        {
            Debug.Log("��� �����");
            ItemData.instance.Scout_Drone = true;
        }
        if (Input.GetKeyDown("o"))//����׿� ������ ���⿡ ������ ������� ���� Ʈ���Ű� �ð�
        {
            Debug.Log("���̴� �����");
            ItemData.instance.Scout_Raider = true;
        }
    }

    private void Event()
    {
        if (Input.GetKeyDown(KeyCode.C)) //�̺�Ʈ Ŭ���� 
        {
            if (GameObject.FindGameObjectWithTag("Event") || GameObject.FindGameObjectWithTag("Stage1_Boss")) // �̺�Ʈ �������� Ȯ��
            {
                Debug.Log("�̺�Ʈ Ŭ����");
                ClosePannel();
                GameObject.FindGameObjectWithTag("Event").GetComponent<CombatEvent>().Clear();
            }
            else
            {
                Debug.Log("�̺�Ʈ�� �ƴ�");
            }
        }
        if (Input.GetKeyDown(KeyCode.F)) //�̺�Ʈ ����
        {
            if (GameObject.FindGameObjectWithTag("Event") || GameObject.FindGameObjectWithTag("Stage1_Boss")) // �̺�Ʈ �������� Ȯ��
            {
                Debug.Log("�̺�Ʈ ����");
                ClosePannel();

                PlayerData.instance.currentHp = PlayerController.instance.currentHp;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                LoadingSceneManager.LoadScene("MainMap_Chess"); // ��ȸ �پ��� �ؾ���
            }
            else
            {
                Debug.Log("�̺�Ʈ�� �ƴ�");
            }
        }

        // ���̵�
        // 2*2 ���� �̺�Ʈ
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ClosePannel();
            MapData.instance.playerStartingPoint_X = MapManager.instance.playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = MapManager.instance.playerNowPoint_Y;

            LoadingSceneManager.LoadScene("Forest");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ClosePannel();
            MapData.instance.playerStartingPoint_X = MapManager.instance.playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = MapManager.instance.playerNowPoint_Y;

            LoadingSceneManager.LoadScene("Wasteland");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ClosePannel();
            MapData.instance.playerStartingPoint_X = MapManager.instance.playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = MapManager.instance.playerNowPoint_Y;

            LoadingSceneManager.LoadScene("Temple1");
        }

        // 3*3 ���� �̺�Ʈ
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ClosePannel();
            MapData.instance.playerStartingPoint_X = MapManager.instance.playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = MapManager.instance.playerNowPoint_Y;

            LoadingSceneManager.LoadScene("Forest2");
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ClosePannel();
            MapData.instance.playerStartingPoint_X = MapManager.instance.playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = MapManager.instance.playerNowPoint_Y;

            LoadingSceneManager.LoadScene("Wasteland2");
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ClosePannel();
            MapData.instance.playerStartingPoint_X = MapManager.instance.playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = MapManager.instance.playerNowPoint_Y;

            LoadingSceneManager.LoadScene("Temple2");
        }

        // ����Ʈ ���� �̺�Ʈ
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ClosePannel();
            MapData.instance.playerStartingPoint_X = MapManager.instance.playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = MapManager.instance.playerNowPoint_Y;

            LoadingSceneManager.LoadScene("ForestBoss");
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ClosePannel();
            MapData.instance.playerStartingPoint_X = MapManager.instance.playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = MapManager.instance.playerNowPoint_Y;

            LoadingSceneManager.LoadScene("SnowBoss");
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ClosePannel();
            MapData.instance.playerStartingPoint_X = MapManager.instance.playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = MapManager.instance.playerNowPoint_Y;

            LoadingSceneManager.LoadScene("RobotBoss");
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ClosePannel();
            MapData.instance.playerStartingPoint_X = MapManager.instance.playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = MapManager.instance.playerNowPoint_Y;

            LoadingSceneManager.LoadScene("MutantBoss");
        }

        // ���� �̺�Ʈ
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            ClosePannel();
            MapData.instance.playerStartingPoint_X = MapManager.instance.playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = MapManager.instance.playerNowPoint_Y;

            LoadingSceneManager.LoadScene("Temple3");
        }

        // ������ �̺�Ʈ  LoadingSceneManager.LoadScene("Temple3"); << ��ġ�Ǵ� ������ ���� �ʿ�
        if (Input.GetKeyDown(KeyCode.F1)) 
        {
            ClosePannel();
            MapData.instance.playerStartingPoint_X = MapManager.instance.playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = MapManager.instance.playerNowPoint_Y;

            LoadingSceneManager.LoadScene("Temple3");
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            ClosePannel();
            MapData.instance.playerStartingPoint_X = MapManager.instance.playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = MapManager.instance.playerNowPoint_Y;

            LoadingSceneManager.LoadScene("Temple3");
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            ClosePannel();
            MapData.instance.playerStartingPoint_X = MapManager.instance.playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = MapManager.instance.playerNowPoint_Y;

            LoadingSceneManager.LoadScene("Temple3");
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            ClosePannel();
            MapData.instance.playerStartingPoint_X = MapManager.instance.playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = MapManager.instance.playerNowPoint_Y;

            LoadingSceneManager.LoadScene("Temple3");
        }

        // ���� ��
        if (Input.GetKeyDown(KeyCode.Equals) && !GameObject.FindGameObjectWithTag("Main"))
        {
            ClosePannel();
            PlayerData.instance.currentHp = PlayerController.instance.currentHp;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            LoadingSceneManager.LoadScene("MainMap_Chess");
        }

        // �׽�Ʈ ��
        if (Input.GetKeyDown(KeyCode.F12))
        {
            ClosePannel();
            MapData.instance.playerStartingPoint_X = MapManager.instance.playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = MapManager.instance.playerNowPoint_Y;

            LoadingSceneManager.LoadScene("EnemyTest");
        }

    }

    private void Ability()
    {
        if (Input.GetKeyDown(KeyCode.R)) // ȸ��
        {
            PlayerData.instance.currentHp = PlayerData.instance.MaxHp;
            PlayerController.instance.currentHp = PlayerData.instance.MaxHp;
            PlayerController.instance.currentDp = PlayerData.instance.MaxDp;
        }

        if (Input.GetKeyDown(KeyCode.A)) // �Ϲ� ���� ��� ���̱�
        {
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject gameObject in enemys)
            {
                gameObject.GetComponent<EnemyFSM>().TakeDamage(10000);
            }
        }
    }

    private void ClosePannel()
    {
        isDebug = false;
        Time.timeScale = 1;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OpenPannel()
    {
        isDebug = true;
        Time.timeScale = 0;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
