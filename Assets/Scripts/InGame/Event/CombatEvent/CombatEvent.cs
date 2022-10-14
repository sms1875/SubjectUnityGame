using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEvent : MonoBehaviour
{
    protected void Clear()
    {
        AllEnemyDie();
        //UI �ٿ�� ������ üũ�ϸ� �� �Ѿ�� �ؾ���
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
    }
}
