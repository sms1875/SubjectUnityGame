using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMod : MonoBehaviour
{
    //입력을 나누기 위해 디버그 변수
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
        // '`'키를 눌렀을 때, 디버그 패널 열고 닫기
        SetDebugMod();
        // 디버그 상태일 시, 키 입력 받기
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
        if (Input.GetKeyDown("k"))//디버그용 실제는 여기에 아이템 사용으로 인한 트리거가 올것
        {
            Debug.Log("추진기 디버그 준비");
            ItemData.instance.Scout_Thruster = true;
        }

        if (Input.GetKeyDown("j"))//디버그용 실제는 여기에 아이템 사용으로 인한 트리거가 올것
        {
            Debug.Log("제트팩 디버그");
            ItemData.instance.Scout_Jetpack = true;
        }

        if (Input.GetKeyDown("h"))//디버그용 실제는 여기에 아이템 사용으로 인한 트리거가 올것
        {
            Debug.Log("바리케이드 디버그");
            ItemData.instance.Scout_Barricade = true;
        }
        if (Input.GetKeyDown("y"))//디버그용 실제는 여기에 아이템 사용으로 인한 트리거가 올것
        {
            Debug.Log("드릴 디버그");
            ItemData.instance.Scout_Drill = true;
        }
        if (Input.GetKeyDown("u"))//디버그용 실제는 여기에 아이템 사용으로 인한 트리거가 올것
        {
            Debug.Log("점프패드 디버그");
            ItemData.instance.Scout_Jumppad = true;
        }
        if (Input.GetKeyDown("i"))//디버그용 실제는 여기에 아이템 사용으로 인한 트리거가 올것
        {
            Debug.Log("드론 디버그");
            ItemData.instance.Scout_Drone = true;
        }
        if (Input.GetKeyDown("o"))//디버그용 실제는 여기에 아이템 사용으로 인한 트리거가 올것
        {
            Debug.Log("레이더 디버그");
            ItemData.instance.Scout_Raider = true;
        }
    }

    private void Event()
    {
        if (Input.GetKeyDown(KeyCode.C)) //이벤트 클리어 
        {
            if (GameObject.FindGameObjectWithTag("Event") || GameObject.FindGameObjectWithTag("Stage1_Boss")) // 이벤트 내부인지 확인
            {
                Debug.Log("이벤트 클리어");
                ClosePannel();
                GameObject.FindGameObjectWithTag("Event").GetComponent<CombatEvent>().Clear();
            }
            else
            {
                Debug.Log("이벤트가 아님");
            }
        }
        if (Input.GetKeyDown(KeyCode.F)) //이벤트 실패
        {
            if (GameObject.FindGameObjectWithTag("Event") || GameObject.FindGameObjectWithTag("Stage1_Boss")) // 이벤트 내부인지 확인
            {
                Debug.Log("이벤트 실패");
                ClosePannel();

                PlayerData.instance.currentHp = PlayerController.instance.currentHp;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                LoadingSceneManager.LoadScene("MainMap_Chess"); // 기회 줄어들게 해야함
            }
            else
            {
                Debug.Log("이벤트가 아님");
            }
        }

        // 맵이동
        // 2*2 전투 이벤트
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

        // 3*3 전투 이벤트
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

        // 엘리트 몬스터 이벤트
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

        // 보스 이벤트
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            ClosePannel();
            MapData.instance.playerStartingPoint_X = MapManager.instance.playerNowPoint_X;
            MapData.instance.playerStartingPoint_Y = MapManager.instance.playerNowPoint_Y;

            LoadingSceneManager.LoadScene("Temple3");
        }

        // 비전투 이벤트  LoadingSceneManager.LoadScene("Temple3"); << 매치되는 씬으로 수정 필요
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

        // 메인 맵
        if (Input.GetKeyDown(KeyCode.Equals) && !GameObject.FindGameObjectWithTag("Main"))
        {
            ClosePannel();
            PlayerData.instance.currentHp = PlayerController.instance.currentHp;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            LoadingSceneManager.LoadScene("MainMap_Chess");
        }

        // 테스트 맵
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
        if (Input.GetKeyDown(KeyCode.R)) // 회복
        {
            PlayerData.instance.currentHp = PlayerData.instance.MaxHp;
            PlayerController.instance.currentHp = PlayerData.instance.MaxHp;
            PlayerController.instance.currentDp = PlayerData.instance.MaxDp;
        }

        if (Input.GetKeyDown(KeyCode.A)) // 일반 몬스터 모두 죽이기
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
