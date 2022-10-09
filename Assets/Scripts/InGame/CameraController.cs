using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float mouseSpeed = 300;
    float mouseY;

    void Update()
    {

        mouseY += Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;

        //���� ����(����,�ּҰ�,�ִ밪)
        mouseY = Mathf.Clamp(mouseY, -55f, 55f);

        //���� ȸ����
        transform.localEulerAngles = new Vector3(-mouseY, 0, 0);

    }
}
