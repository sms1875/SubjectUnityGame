using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTileRaycast : MonoBehaviour
{
    //public Transform target;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 p = Input.mousePosition;

            Ray cast = Camera.main.ScreenPointToRay(Input.mousePosition);


            // Mouse�� �������� Ray cast �� ��ȯ



            RaycastHit hit;

            if (Physics.Raycast(cast, out hit))
            {
                
                
            }
        }



        /**
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mos = Input.mousePosition;
            
            Vector3 dir = camera.ScreenToWorldPoint

            RaycastHit hit;
            if(Physics.Raycast())
            {
                Debug.Log(hit.transform.gameObject);
            }
        }**/
    }
}
