using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice : MonoBehaviour
{
    public Camera UI_Camera;
    Vector3 m_vecMouseDownPos;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_vecMouseDownPos = Input.mousePosition;

            Vector2 pos = UI_Camera.ScreenToWorldPoint(m_vecMouseDownPos);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.name);

                if (hit.collider.name == "Image(1)") Debug.Log("Select Item1");
                else if (hit.collider.name == "Image(2)") Debug.Log("Select Item2");
                else if (hit.collider.name == "Image(3)") Debug.Log("Select Item3");
                else if (hit.collider.name == "Image(4)") Debug.Log("Select Item4");
                else if (hit.collider.name == "Image(5)") Debug.Log("Select Item5");
            }
        }        
    }
}
