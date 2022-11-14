using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float mouseY;

    void Update()
    {
        if (PlayerController.instance.IsStunned || DebugMod.isDebug)
        {
            return;
        }

        mouseY += Input.GetAxis("Mouse Y") * DataManager.instance.mouseSensitivity * Time.deltaTime;

        //범위 제한(변수,최소값,최대값)
        mouseY = Mathf.Clamp(mouseY, -55f, 55f);

        //로컬 회전값
        transform.localEulerAngles = new Vector3(-mouseY, 0, 0);

    }
}
