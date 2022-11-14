using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    //public Gun gun;

    [Header("Fire Effects")]
    [SerializeField]
    private GameObject muzzleFlashEffect; // 총구화염 이펙트

    [Header("Spawn Points")]
    [SerializeField]
    public Transform bulletSpawnPoint; // 총구위치

    public string gunName; // 총의 이름.
    public float range; // 사정 거리
    public float accuracy; // 정확도
    public float fireRate; // 연사속도.
    public float reloadTime; // 재장전 속도.
    public float getWeaponTime; // 무기 꺼내기 속도
    public float outWeaponTime; // 무기 넣기 속도

    // public bool isAutomaic; // 연사총인가

    public float damage; // 총의 데미지.

    public int reloadBulletCount; // 총알 재정전 개수.
    public int currentBulletCount; // 현재 탄알집에 남아있는 총알의 개수.
    public int maxBulletCount = 99999; // 최대 소유 가능 총알 개수.
    public int carryBulletCount; // 현재 소유하고 있는 총알 개수.

    public float retroActionForce; // 반동 세기
    public float retroActionFineSightForce; // 정조준시의 반동 세기.

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

    public void MuzzleFlash() // 총구화염 출력
    {
        StartCoroutine(OnMuzzleFlashEffect());
    }

    private IEnumerator OnMuzzleFlashEffect() // 총구화염 출력용 Coroutine
    {
        muzzleFlashEffect.SetActive(true);
        yield return new WaitForSeconds(fireRate * 0.3f);
        muzzleFlashEffect.SetActive(false);
    }
}
