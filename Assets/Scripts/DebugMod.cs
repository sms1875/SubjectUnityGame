using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMod : MonoBehaviour
{
    //�Է��� ������ ���� ����� ����
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
        // '`'Ű�� ������ ��, ����� �г� ���� �ݱ�
        SetDebugMod();
        // ����� ������ ��, Ű �Է� �ޱ�
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
                isDebug = false;
                Time.timeScale = 1;
                transform.GetChild(0).gameObject.SetActive(false);
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
                isDebug = false;
                Time.timeScale = 1;
                transform.GetChild(0).gameObject.SetActive(false);

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
    }
}
