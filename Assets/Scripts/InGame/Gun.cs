using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    //public Gun gun;

    [Header("Fire Effects")]
    [SerializeField]
    private GameObject muzzleFlashEffect; // �ѱ�ȭ�� ����Ʈ

    [Header("Spawn Points")]
    [SerializeField]
    public Transform bulletSpawnPoint; // �ѱ���ġ

    public string gunName; // ���� �̸�.
    public float range; // ���� �Ÿ�
    public float accuracy; // ��Ȯ��
    public float fireRate; // ����ӵ�.
    public float reloadTime; // ������ �ӵ�.
    public float getWeaponTime; // ���� ������ �ӵ�
    public float outWeaponTime; // ���� �ֱ� �ӵ�

    // public bool isAutomaic; // �������ΰ�

    public float damage; // ���� ������.

    public int reloadBulletCount; // �Ѿ� ������ ����.
    public int currentBulletCount; // ���� ź������ �����ִ� �Ѿ��� ����.
    public int maxBulletCount = 99999; // �ִ� ���� ���� �Ѿ� ����.
    public int carryBulletCount; // ���� �����ϰ� �ִ� �Ѿ� ����.

    public float retroActionForce; // �ݵ� ����
    public float retroActionFineSightForce; // �����ؽ��� �ݵ� ����.

    private ImpactMemoryPool impactMemoryPool;

    private void Awake()
    {
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
        muzzleFlashEffect.SetActive(false);
    }

    public void init(Gun origin)
    {
        reloadBulletCount = origin.reloadBulletCount;
        currentBulletCount = origin.currentBulletCount;
        maxBulletCount = origin.maxBulletCount;
        carryBulletCount = origin.carryBulletCount;
    }

    public void MuzzleFlash() // �ѱ�ȭ�� ���
    {
        StartCoroutine(OnMuzzleFlashEffect());
    }

    private IEnumerator OnMuzzleFlashEffect() // �ѱ�ȭ�� ��¿� Coroutine
    {
        muzzleFlashEffect.SetActive(true);
        yield return new WaitForSeconds(fireRate * 0.3f);
        muzzleFlashEffect.SetActive(false);
    }
}
