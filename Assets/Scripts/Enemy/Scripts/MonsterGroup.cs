using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGroup : MonoBehaviour
{
    private EnemyFSM[] enemys;
    private int length;

    private void Awake()
    {
        length = transform.childCount;
        enemys = GetComponentsInChildren<EnemyFSM>();
    }

    private void LateUpdate()
    {
        ChangeState();
    }

    private void ChangeState()
    {
        bool isRecognize = false;

        for(int i = 0; i < length; i++)
        {
            if(enemys[i].enemyState==EnemyState.Hit|| enemys[i].enemyState == EnemyState.Pursuit || enemys[i].enemyState == EnemyState.Attack)
            {
                isRecognize = true;
                break;
            }
        }

        if (isRecognize)
        {
            for (int i = 0; i < length; i++)
            {
                if (enemys[i].enemyState == EnemyState.Idle || enemys[i].enemyState == EnemyState.Wander)
                {
                    enemys[i].ChangeState(EnemyState.Pursuit);
                }
            }
        }
    }
}
