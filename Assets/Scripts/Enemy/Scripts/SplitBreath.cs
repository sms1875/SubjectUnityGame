using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBreath : MonoBehaviour
{
    public ParticleSystem centerPs;
    public Transform centerTrigger;

    public ParticleSystem leftPs;
    public Transform leftTrigger;

    public ParticleSystem rightPs;
    public Transform rightTrigger;

    private void OnEnable()
    {
        StartCoroutine("Control");
    }

    private IEnumerator Control()
    {
        TriggerLengthWithRay();

        yield return new WaitForSeconds(1.9f);

        centerTrigger.localScale = new Vector3(0, 0, 0);
        centerTrigger.localPosition = new Vector3(0, 0.8f, 2);
        leftTrigger.localScale = new Vector3(0, 0, 0);
        leftTrigger.localPosition = new Vector3(0, 0.8f, 2);
        rightTrigger.localScale = new Vector3(0, 0, 0);
        rightTrigger.localPosition = new Vector3(0, 0.8f, 2);

        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }

    private void TriggerLengthWithRay()
    {
        RaycastHit hit;

        if(Physics.Raycast(centerTrigger.position, centerTrigger.forward, out hit, 20f, ~LayerMask.GetMask("Stage1_Boss", "Player")))
        {
            Debug.Log(hit.transform.name);

            centerTrigger.localPosition = new Vector3(0, 0, hit.distance / 2f + 1);
            centerTrigger.localScale = new Vector3(1, 1, hit.distance + 2);

            centerPs.startSpeed = hit.distance * 1.9f;
        }
        else
        {
            centerTrigger.localPosition = new Vector3(0, 0, 9);
            centerTrigger.localScale = new Vector3(1, 1, 17);

            centerPs.startSpeed = 35f;
        }

        if (Physics.Raycast(leftTrigger.position, leftTrigger.forward, out hit, 20f, ~LayerMask.GetMask("Stage1_Boss", "Player")))
        {
            Debug.Log(hit.transform.name);

            leftTrigger.localPosition = new Vector3(0, 0, hit.distance / 2f + 2);
            leftTrigger.localScale = new Vector3(1, 1, hit.distance + 3);

            leftPs.startSpeed = hit.distance * 1.9f;
        }
        else
        {
            leftTrigger.localPosition = new Vector3(0, 0, 9);
            leftTrigger.localScale = new Vector3(1, 1, 17);

            leftPs.startSpeed = 35f;
        }

        if (Physics.Raycast(rightTrigger.position, rightTrigger.forward, out hit, 20f, ~LayerMask.GetMask("Stage1_Boss", "Player")))
        {
            Debug.Log(hit.transform.name);
            rightTrigger.localPosition = new Vector3(0, 0, hit.distance / 2f + 2);
            rightTrigger.localScale = new Vector3(1, 1, hit.distance + 3);

            rightPs.startSpeed = hit.distance * 1.9f;
        }
        else
        {
            rightTrigger.localPosition = new Vector3(0, 0, 9);
            rightTrigger.localScale = new Vector3(1, 1, 17);

            rightPs.startSpeed = 35f;
        }
    }
}
