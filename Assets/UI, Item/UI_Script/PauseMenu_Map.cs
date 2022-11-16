using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu_Map : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject hud;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
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
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        hud.SetActive(true);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        hud.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
    }
}
