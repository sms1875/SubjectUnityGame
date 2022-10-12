using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flak : MonoBehaviour
{
    public RobotMemoryPool pistol1;
    public RobotMemoryPool pistol2;

    public void OnShoot1()
    {
        pistol1.Shoot();
    }

    public void OnShoot2()
    {
        pistol2.Shoot();
    }
}
