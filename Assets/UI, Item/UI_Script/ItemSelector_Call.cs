using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelector_Call: MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject selectorUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        selectorUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        selectorUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

}
