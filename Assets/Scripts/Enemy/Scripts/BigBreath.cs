using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBreath : MonoBehaviour
{
    public Transform trigger;
    public float holdingTime; // 파티클 duration이랑 맞추거나 0.1~0.2초 길게

    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        StartCoroutine("Contol");
    }

    private IEnumerator Contol()
    {
        StartCoroutine("TriggerLengthWithRay");

        yield return new WaitForSeconds(holdingTime);
        trigger.localScale = new Vector3(0, 0, 0);
        trigger.localPosition = new Vector3(0, -1, 2);

        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private IEnumerator TriggerLengthWithRay()
    {
        RaycastHit hit;

        float currentTime = 0;
        bool isSetUp = false;
        while (true)
        {
            currentTime += Time.deltaTime;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 500f, ~LayerMask.GetMask("Stage1_Boss", "Player", "Breath", "Floor")))
            {
                trigger.localPosition = new Vector3(0, 2, hit.distance / 6f + 2);
                trigger.localScale = new Vector3(3, 3, hit.distance - 9);
                if (hit.transform.CompareTag("Wall"))
                {
                    var main = ps.main;
                    main.startSpeed = hit.distance;
                }
                else
                {
                    var main = ps.main;
                    main.startSpeed = hit.distance / 3;
                }
                if (hit.transform.CompareTag("Pillar") && !isSetUp)
                {
                    isSetUp = true;
                    hit.transform.GetComponent<Pillar>().SetUp(holdingTime - currentTime);
                }
            }

            yield return null;
        }
    }
}
