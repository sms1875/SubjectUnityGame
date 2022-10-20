using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType { Bullet = 0, GuidedMissile, VeneerMissile }

public class RobotMemoryPool : MonoBehaviour
{
    public GameObject bullet;
    public GameObject shotEffect;

    public BulletType bulletType;
    public GameObject cap;
    public Material[] mats;

    private MemoryPool[] memoryPools;
    private int index = -1;

    private void Awake()
    {
        memoryPools = new MemoryPool[2];
        memoryPools[0] = new MemoryPool(bullet, false, 10);
        memoryPools[1] = new MemoryPool(shotEffect, false, 10);
    }

    public void Shoot(int index = -1, float x = 999, float y = 999, float z = 999)
    {
        if(bulletType == BulletType.Bullet)
        {
            GameObject bulletClone = memoryPools[0].ActivatePoolItem();
            bulletClone.transform.position = transform.position;
            bulletClone.transform.rotation = transform.rotation;
            bulletClone.GetComponent<RobotBullet>().SetUp(memoryPools[0]);
        }
        else if(bulletType == BulletType.GuidedMissile)
        {
            this.index = index;
            cap.SetActive(false);
            GameObject bulletClone = memoryPools[0].ActivatePoolItem();
            bulletClone.transform.position = transform.position;
            bulletClone.GetComponent<GuidedMissile>().SetUp(memoryPools[0], transform.position.y);

            Invoke("CapActive", 1f);
        }
        else if (bulletType == BulletType.VeneerMissile)
        {
            this.index = index;
            cap.SetActive(false);
            GameObject bulletClone = memoryPools[0].ActivatePoolItem();
            bulletClone.transform.position = transform.position;
            bulletClone.GetComponent<VeneerMissile>().SetUp(memoryPools[0], new Vector3(x, y, z));

            Invoke("CapActive", 1f);
        }



        GameObject shotEffectClone = memoryPools[1].ActivatePoolItem();
        shotEffectClone.transform.position = transform.position;
        shotEffectClone.transform.rotation = transform.rotation;
        shotEffectClone.GetComponent<RobotShotEffect>().SetUp(memoryPools[1]);
    }

    private void CapActive()
    {
        if (index != -1)
        {
            cap.GetComponentInChildren<MeshRenderer>().material = mats[index];
        }
        cap.SetActive(true);
        index = -1;
    }
}
