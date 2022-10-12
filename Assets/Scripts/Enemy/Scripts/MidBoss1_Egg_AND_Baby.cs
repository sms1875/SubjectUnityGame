using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidBoss1_Egg_AND_Baby : MonoBehaviour
{
    private MidBoss1_Egg egg;
    private SphereCollider eggColl;

    private void Awake()
    {
        egg = GetComponentInChildren<MidBoss1_Egg>();
        eggColl = GetComponentInChildren<SphereCollider>();
    }

    public void SetUp()
    {
        gameObject.layer = 0;
        egg.enabled = true;
        eggColl.enabled = true;
    }
}
