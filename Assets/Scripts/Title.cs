using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string sceneName = "MainMap_Chess";

    public void ClickStart()
    {
        LoadingSceneManager.LoadScene("MainMap_Chess");
    }

    public void ClickExit()
    {
        
    }
}
