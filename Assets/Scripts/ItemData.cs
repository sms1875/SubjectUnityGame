using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public static ItemData instance;
    public bool HealLobby = false;
    public bool HealInGame = false;
    public bool IncreaseHp = false;
    public bool IncreaseDamage = false;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (IncreaseHp)
        {
            PlayerData.instance.MaxHp += 10;
            PlayerData.instance.currentHp += 10;
            IncreaseHp = false;
        }

        if (HealLobby)
        {
            PlayerData.instance.currentHp = PlayerData.instance.MaxHp;
            HealLobby = false;
        }


        if (HealInGame)
        {
            PlayerController.instance.currentHp= PlayerData.instance.MaxHp;
            HealInGame = false;
        }

    }

    public void DamageUp()
    {
        Debug.Log("데미지증가");
        Debug.Log("데미지 : " + GunController.instance.currentGun.damage);
        GunController.instance.currentGun.damage += 10;
        Debug.Log("증가된 데미지 : " + GunController.instance.currentGun.damage);
        ItemData.instance.IncreaseDamage = false;
    }
}
