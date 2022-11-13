using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMod : MonoBehaviour
{
    //입력을 나누기 위해 디버그 변수
    public static bool isDebug;

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
            Item();
            Event();
        }
    }

    private void SetDebugMod()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (isDebug)
            {
                isDebug = false;
                Time.timeScale = 1;
                transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                isDebug = true;
                Time.timeScale = 0;
                transform.GetChild(0).gameObject.SetActive(true);
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
                isDebug = false;
                Time.timeScale = 1;
                transform.GetChild(0).gameObject.SetActive(false);
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
                isDebug = false;
                Time.timeScale = 1;
                transform.GetChild(0).gameObject.SetActive(false);

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
    }
}
