using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_PlayerMoving : MonoBehaviour
{
    static public GameObject slice;

    // Start is called before the first frame update
    void Start()
    {
        
        slice = GetComponent<GameObject>();
        ViewSlice();
        Debug.Log("t");
    }

 

    static void ViewSlice()
    {
        slice.gameObject.SetActive(true);
    }


}
