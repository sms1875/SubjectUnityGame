using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotShotEffect : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void SetUp(MemoryPool memoryPool)
    {
        StartCoroutine("AutoDisable", memoryPool);
    }

    private IEnumerator AutoDisable(MemoryPool memoryPool)
    {
        while (ps.isPlaying)
        {
            yield return null;
        }

        memoryPool.DeactivatePoolItem(gameObject);
    }
}
