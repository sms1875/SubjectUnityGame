using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    float mouseY;
    Camera cam;

    private void Awake()
    {
        instance = this;
        cam = gameObject.GetComponentInChildren<Camera>();
    }

    void Update()
    {

        mouseY += Input.GetAxis("Mouse Y") * DataManager.instance.mouseSensitivity * Time.deltaTime;

        //���� ����(����,�ּҰ�,�ִ밪)
        mouseY = Mathf.Clamp(mouseY, -55f, 55f);

        //���� ȸ����
        transform.localEulerAngles = new Vector3(-mouseY, 0, 0);

    }

    public void ReBoundY(float value)
    {
        mouseY += value;
        //cam.fieldOfView += 5;
    }
}
