using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceEvent : CombatEvent
{
    public float limitTime = 180;
    public GameObject smallShip;

    private void Update()
    {
        ClearCheck();
    }

    private void ClearCheck()
    {
        if (limitTime <= 0)
        {
            return;
        }
        limitTime -= Time.deltaTime;
        if (limitTime <= 0)
        {
            limitTime = 0;

            Clear();
        }
        if (!smallShip.activeSelf)
        {
            Invoke("Fail", 3f);
        }
    }

    private void Fail()
    {
        PlayerData.instance.currentHp = PlayerController.instance.currentHp;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        LoadingSceneManager.LoadScene("MainMap_Chess"); // 기회 줄어들게 해야함
    }
}
