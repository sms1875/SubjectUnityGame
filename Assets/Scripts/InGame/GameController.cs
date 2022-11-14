using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Result()
    {
        PlayerData.instance.currentHp = PlayerController.instance.currentHp;
        StartCoroutine(tttt());
    }

    IEnumerator tttt()
    {
        yield return new WaitForSeconds(100f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        LoadingSceneManager.LoadScene("MainMap_Chess");
    }
}
