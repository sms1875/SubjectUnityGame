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

        //���� ����(����,�ּҰ�,�ִ밪)
        mouseY = Mathf.Clamp(mouseY, -55f, 55f);

        //���� ȸ����
        transform.localEulerAngles = new Vector3(-mouseY, 0, 0);

    }
}
