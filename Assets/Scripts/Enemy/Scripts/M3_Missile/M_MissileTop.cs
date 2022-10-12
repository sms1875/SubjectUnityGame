using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class M_MissileTop : MonoBehaviour
{
    public RobotMemoryPool[] guidedMissiles;
    public RobotMemoryPool[] iceMissiles;
    public RobotMemoryPool[] fireMissiles;

    public bool isDead;

    private Transform target;

    public List<BossPattern> missilePatterns;
    private float patternTotalWeight = 0f;

    private int nextIndex = -1;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < missilePatterns.Count; i++)
        {
            patternTotalWeight += missilePatterns[i].weight;
        }
    }

    private void OnEnable()
    {
        StartCoroutine("OnGuidedMissile");
    }

    private void LateUpdate()
    {
        if (isDead)
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator Think()
    {
        yield return new WaitForSeconds(5f);

        switch (nextIndex)
        {
            case 0:
                StartCoroutine("OnGuidedMissile");
                break;
            case 1:
                StartCoroutine("OnIceMissiles");
                break;
            case 2:
                StartCoroutine("OnFireMissiles");
                break;
        }
    }

    private IEnumerator OnGuidedMissile()
    {
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine("SetNextPattern");

        for (int i = 0; i < guidedMissiles.Length; i++)
        {
            guidedMissiles[i].Shoot(nextIndex);
            yield return new WaitForSeconds(1f);
        }

        StartCoroutine("Think");
    }

    private IEnumerator OnIceMissiles()
    {
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine("SetNextPattern");

        for (int i = 0; i < iceMissiles.Length; i++)
        {
            iceMissiles[i].Shoot(nextIndex, target.position.x, target.position.y, target.position.z);
            yield return new WaitForSeconds(1f);
        }

        StartCoroutine("Think");
    }

    private IEnumerator OnFireMissiles()
    {
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine("SetNextPattern");

        for (int i = 0; i < fireMissiles.Length; i++)
        {
            fireMissiles[i].Shoot(nextIndex, target.position.x, target.position.y, target.position.z);
            yield return new WaitForSeconds(1f);
        }

        StartCoroutine("Think");
    }

    private IEnumerator SetNextPattern()
    {
        List<BossPattern> bossMeleePatternWeightSortList = missilePatterns.OrderBy(x => x.weight).ToList();
        string patternName = "";
        float weight = 0;
        float selectNum = 0;
        selectNum = patternTotalWeight * Random.Range(0.0f, 1.0f); // 0.0 ~ 1.0

        for (int i = 0; i < bossMeleePatternWeightSortList.Count; i++)
        {
            weight += bossMeleePatternWeightSortList[i].weight;
            if (selectNum <= weight)
            {
                patternName = bossMeleePatternWeightSortList[i].patternName;

                break;
            }
        }

        switch (patternName)
        {
            case "GuidedMissile":
                nextIndex = 0;
                break;
            case "IceMissile":
                nextIndex = 1;
                break;
            case "FireMissile":
                nextIndex = 2;
                break;
        }

        yield return null;
    }
}
