using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Caller : MonoBehaviour
{
    public static bool InventoryIsOpened = false;
    public GameObject inventoryUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (InventoryIsOpened)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }
    void OpenInventory()
    {
        inventoryUI.SetActive(true);
        InventoryIsOpened = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void CloseInventory()
    {
        inventoryUI.SetActive(false);
        InventoryIsOpened = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
