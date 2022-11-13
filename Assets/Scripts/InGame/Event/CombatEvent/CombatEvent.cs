using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEvent : MonoBehaviour
{
    private bool isClear;

    public void Clear()
    {
        if (isClear) return;
        isClear = true;
        AllEnemyDie();
        //UI 뛰우고 아이템 체크하면 씬 넘어가게 해야함
        Invoke("LoadMainScene", 5f);
    }

    private void LoadMainScene()
    {
        PlayerData.instance.currentHp = PlayerController.instance.currentHp;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        LoadingSceneManager.LoadScene("MainMap_Chess");
    }

    private void AllEnemyDie()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
        foreach (GameObject gameObject in spawners)
        {
            gameObject.GetComponent<SpawnerController>().StopAllCoroutines();
        }
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject gameObject in enemys)
        {
            gameObject.GetComponent<EnemyFSM>().TakeDamage(10000);
        }
        GameObject[] baby = GameObject.FindGameObjectsWithTag("Baby");
        foreach (GameObject gameObject in baby)
        {
            gameObject.GetComponent<MidBoss1_Baby>().TakeDamage(10000);
        }
        GameObject[] midBoss1 = GameObject.FindGameObjectsWithTag("MidBoss1");
        foreach (GameObject gameObject in midBoss1)
        {
            gameObject.GetComponent<MidBossFSM_1>().TakeDamage(10000);
        }
        GameObject[] midBoss2 = GameObject.FindGameObjectsWithTag("MidBoss2");
        foreach (GameObject gameObject in midBoss2)
        {
            gameObject.GetComponent<MidBossFSM_2>().TakeDamage(10000);
        }
        GameObject[] midBoss3 = GameObject.FindGameObjectsWithTag("MidBoss3");
        foreach (GameObject gameObject in midBoss3)
        {
            gameObject.GetComponent<MidBoss3>().TakeDamage(10000);
        }
        GameObject[] midBoss4 = GameObject.FindGameObjectsWithTag("Stage1_Boss");
        foreach (GameObject gameObject in midBoss4)
        {
            gameObject.GetComponent<BossFSM>().TakeDamage(10000);
        }
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
        foreach (GameObject gameObject in turrets)
        {
            gameObject.GetComponent<Turret>().TakeDamage(10000);
        }
    }
}
