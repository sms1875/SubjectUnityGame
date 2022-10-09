using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // �ʿ��� ������Ʈ
    [SerializeField]
    private GunController theGunController;
    private Gun currentGun;

    // �ʿ��ϸ� HUD ȣ��, �ʿ� ������ HUD ��Ȱ��ȭ.
    [SerializeField]
    private GameObject go_BulletHUD;

    // �Ѿ� ���� �ؽ�Ʈ�� �ݿ�
    [SerializeField]
    private Text[] text_Bullet;

    private void Awake()
    {
        theGunController = FindObjectOfType<GunController>();
    }
    // Update is called once per frame
    void Update()
    {
        CheckBullet();
    }

    private void CheckBullet()
    {
        currentGun = GunController.instance.currentGun;
        text_Bullet[0].text = currentGun.reloadBulletCount.ToString();//������ ������ �Ѿ� ����
        text_Bullet[1].text = currentGun.currentBulletCount.ToString();//���� �Ѿ� ����
    }
}
