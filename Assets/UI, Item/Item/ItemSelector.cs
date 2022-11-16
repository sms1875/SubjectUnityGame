using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    [SerializeField] public GameObject go_SelectorParent;
    public int howmany_pickups_in_this_scene;
    private int howmany_items_selected;
    public Item[] itemObj;
    public GameObject SelectorUI;
    public GameObject OK_button;
    public Inventorys theInventory;

    private void Start()
    {
        howmany_items_selected = 0;
    }

    public void ItemSelect(int index)
    {
        if (OK_button.activeInHierarchy)
        {
            howmany_items_selected++;
            theInventory.AcquireItem(itemObj[index]);
            Debug.Log("Item Selected");
        }
        
        Debug.Log(howmany_items_selected);
        if (howmany_items_selected == howmany_pickups_in_this_scene)
        {
            SelectorEnd();
        }
    }

    public void SelectorEnd()
    {
            SelectorUI.SetActive(false);
    }
}
