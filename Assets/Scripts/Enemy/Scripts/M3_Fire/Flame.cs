using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    public GameObject fire;

    public void OnShoot()
    {
        fire.SetActive(true);
    }
}