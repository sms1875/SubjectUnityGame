using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public float MaxHp, MaxDp;
    public float currentHp, currentDp;
    public float DashGauge = 60;

    public Gun[] currentGunList; // ÇöÀç ÀåÂøµÈ ÃÑ

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentHp = MaxHp;
        currentDp = MaxDp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
