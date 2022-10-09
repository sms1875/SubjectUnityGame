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


            // Mouse의 포지션을 Ray cast 로 변환



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
