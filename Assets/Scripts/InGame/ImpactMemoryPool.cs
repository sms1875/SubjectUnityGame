using UnityEngine;

public enum ImpactType { Enemy = 0, Enemy_Head, /*Drone, MidBoss1, MidBoss1_Head, MidBoss2, MidBoss2_Head, Stage1_Boss,*/ }

public class ImpactMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] impactPrefab;
    private MemoryPool[] memoryPool;

    private void Awake()
    {
        memoryPool = new MemoryPool[impactPrefab.Length];
        for (int i = 0; i < impactPrefab.Length; ++i)
        {
            memoryPool[i] = new MemoryPool(impactPrefab[i]);
        }
    }

    public void SpawnImpact(RaycastHit hit)
    {
        //if (hit.transform.CompareTag("Enemy"))
        {
            OnSpawnImpact(ImpactType.Enemy, hit.point, Quaternion.LookRotation(hit.normal));
        } 

        /*
        else if (hit.transform.CompareTag("Enemy_Head"))
        {
            OnSpawnImpact(ImpactType.Enemy_Head, hit.point, Quaternion.LookRotation(hit.normal));
            Debug.Log("�� �Ӹ� ���� ����Ʈ");
        }*/

        /*
        else if (hit.transform.CompareTag("ImpactDrone"))
        {
            OnSpawnImpact(ImpactType.Drone, hit.point, Quaternion.LookRotation(hit.normal));
            Debug.Log("��� ���� ����Ʈ");
        }
        else if (hit.transform.CompareTag("ImpactMidBoss1"))
        {
            OnSpawnImpact(ImpactType.MidBoss1, hit.point, Quaternion.LookRotation(hit.normal));
            Debug.Log("�߰�����1 ���� ����Ʈ");
        }
        else if (hit.transform.CompareTag("ImpactMidBoss1_Head"))
        {
            OnSpawnImpact(ImpactType.MidBoss1_Head, hit.point, Quaternion.LookRotation(hit.normal));
            Debug.Log("�߰�����1 �Ӹ� ���� ����Ʈ");
        }
        else if (hit.transform.CompareTag("ImpactMidBoss2"))
        {
            OnSpawnImpact(ImpactType.MidBoss2, hit.point, Quaternion.LookRotation(hit.normal));
            Debug.Log("�߰�����2 ���� ����Ʈ");
        }
        else if (hit.transform.CompareTag("ImpactMidBoss2_Head"))
        {
            OnSpawnImpact(ImpactType.MidBoss2_Head, hit.point, Quaternion.LookRotation(hit.normal));
            Debug.Log("�߰�����2 �Ӹ� ���� ����Ʈ");
        }
        else if (hit.transform.CompareTag("ImpactStage1_Boss"))
        {
            OnSpawnImpact(ImpactType.Stage1_Boss, hit.point, Quaternion.LookRotation(hit.normal));
            Debug.Log("�������� ���� ���� ����Ʈ");
        }
        */
    }

    public void OnSpawnImpact(ImpactType type, Vector3 position, Quaternion rotation)
    {
        GameObject item = memoryPool[(int)type].ActivatePoolItem();
        item.transform.position = position;
        item.transform.rotation = rotation;
        item.GetComponent<Impact>().Setup(memoryPool[(int)type]);
    }
}
