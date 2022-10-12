using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimatorController : MonoBehaviour
{
    public GameObject kickAttackCollision;
    public GameObject punchAttackCollision_r;
    public GameObject punchAttackCollision_l;
    public GameObject shockWave;
    public GameObject rushEffect;
    public GameObject rushAttackCollision;
    public GameObject poisionGas;

    public void OnKick()
    {
        kickAttackCollision.SetActive(true);
    }

    public void OnPunch()
    {
        if(punchAttackCollision_r != null)punchAttackCollision_r.SetActive(true);
        punchAttackCollision_l.SetActive(true);
    }

    public void OnMoveForPunchForce()
    {
        transform.parent.position += transform.parent.forward * 0.3f;
    }

    public void OnShockWave()
    {
        shockWave.SetActive(true);
    }

    public void OnRushEffect()
    {
        rushEffect.SetActive(true);
    }

    public void OnRushAttackCollision()
    {
        if (rushAttackCollision.activeSelf == false)
        {
            rushAttackCollision.SetActive(true);
        }
    }

    public void OnPoisionGas()
    {
        poisionGas.SetActive(true);
    }
}
