using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventorys : MonoBehaviour
{
    [SerializeField] public GameObject go_SlotsParent_Map_Weapon;
    [SerializeField] public GameObject go_SlotsParent_Map_Heal;
    [SerializeField] public GameObject go_SlotsParent_Map_Tactical;
    [SerializeField] public GameObject go_SlotsParent_InGame_Weapon;
    [SerializeField] public GameObject go_SlotsParent_InGame_Heal;
    [SerializeField] public GameObject go_SlotsParent_InGame_Tactical;

    public Slots[] slots_Map_Weapon;
    public Slots[] slots_Map_Heal;
    public Slots[] slots_Map_Tactical;
    public Slots[] slots_InGame_Weapon;
    public Slots[] slots_InGame_Heal;
    public Slots[] slots_InGame_Tactical;

    private void Start()
    {
        slots_Map_Weapon = go_SlotsParent_Map_Weapon.GetComponentsInChildren<Slots>();
        slots_Map_Heal = go_SlotsParent_Map_Heal.GetComponentsInChildren<Slots>();
        slots_Map_Tactical = go_SlotsParent_Map_Tactical.GetComponentsInChildren<Slots>();
        slots_InGame_Weapon = go_SlotsParent_InGame_Weapon.GetComponentsInChildren<Slots>();
        slots_InGame_Heal = go_SlotsParent_InGame_Heal.GetComponentsInChildren<Slots>();
        slots_InGame_Tactical = go_SlotsParent_InGame_Tactical.GetComponentsInChildren<Slots>();
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if(Item.ItemType.Weapon == _item.itemType)
        {
            for(int i = 0; i < slots_Map_Weapon.Length; i++)
            {
                if(slots_Map_Weapon[i].item == null)
                {
                    slots_Map_Weapon[i].AddItem(_item, _count);
                    return;
                }
            }
        }
        else if(Item.ItemType.Heal == _item.itemType)
        {
            for(int i = 0; i < slots_Map_Heal.Length; i++)
            {
                if (slots_Map_Heal[i].item == null)
                {
                    slots_Map_Heal[i].AddItem(_item, _count);
                    return;
                }
            }
        }
        else if(Item.ItemType.Heal == _item.itemType)
        {
            for(int i = 0; i < slots_Map_Tactical.Length; i++)
            {
                if (slots_Map_Tactical[i].item == null)
                {
                    slots_Map_Tactical[i].AddItem(_item, _count);
                    return;
                }
            }
        }
    }
}
