using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOffMesh : MonoBehaviour
{
    [SerializeField]
    private int dropArea = 3;

    [SerializeField]
    private float dropSpeed = 1.5f;
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitUntil(()=> IsDrop());

            yield return StartCoroutine("Drop");
        }
    }

    private bool IsDrop()
    {
        if (navMeshAgent.isOnOffMeshLink)
        {
            OffMeshLinkData linkData = navMeshAgent.currentOffMeshLinkData;

            if(linkData.offMeshLink != null && linkData.offMeshLink.area == dropArea)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator Drop()
    {
        navMeshAgent.isStopped = true;

        OffMeshLinkData linkData = navMeshAgent.currentOffMeshLinkData;
        Vector3 start = linkData.startPos;
        Vector3 end = linkData.endPos;

        float dropTime = Mathf.Abs(end.y - start.y) / dropSpeed;
        float currentTime = 0f;
        float percent = 0;

        while (percent < 1)
        {
            transform.rotation = linkData.offMeshLink.startTransform.rotation;
            currentTime += Time.deltaTime;
            percent = currentTime / dropTime;

            transform.position = Vector3.Lerp(start, end, percent);

            yield return null;
        }

        navMeshAgent.CompleteOffMeshLink();

        navMeshAgent.isStopped = false;

        navMeshAgent.velocity = Vector3.zero;
    }
}
