using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBossEnterEvent : MonoBehaviour
{
    public GameObject powder;
    public GameObject voice;
    public GameObject whiteOut;

    private bool isEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&& !isEnter)
        {
            other.GetComponent<PlayerController>().Shake(10, 2, 2);
            powder.SetActive(true);
            voice.SetActive(true);
            isEnter = true;
            Invoke("SetOnWhiteOut", 9f);
            Invoke("BossScene", 11f);
        }
    }

    private void SetOnWhiteOut()
    {
        whiteOut.SetActive(true);
    }

    private void BossScene()
    {
        PlayerData.instance.currentHp = PlayerController.instance.currentHp;
        LoadingSceneManager.LoadScene("TempleBoss");
    }
}
