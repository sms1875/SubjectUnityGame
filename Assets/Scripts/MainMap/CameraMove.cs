using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject Player;
    Vector3 playerPos;

    void Update()
    {
        playerPos = Player.transform.position;
        playerPos.y = transform.position.y;
        transform.position = playerPos;
    }
}
