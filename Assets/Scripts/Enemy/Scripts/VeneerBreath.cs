using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeneerBreath : MonoBehaviour
{
    public GameObject veneerProjectileSpawner;
    public Transform trigger;

    private void OnEnable()
    {
        StartCoroutine("Control");
    }

    private IEnumerator Control()
    {
        RaycastHit hit;

        Vector3 tf = new Vector3(transform.position.x, transform.position.y + 20, transform.position.z);

        if (Physics.Raycast(tf, transform.up, out hit, 300f, ~LayerMask.GetMask("Stage1_Boss")))
        {   
            veneerProjectileSpawner.transform.position = hit.point;
            if (veneerProjectileSpawner.activeSelf == false) veneerProjectileSpawner.SetActive(true);
            else veneerProjectileSpawner.GetComponent<VeneerProjectileSpawner>().SetUp();
            trigger.localPosition = new Vector3(0, -1, hit.distance / 2f + 2);
            trigger.localScale = new Vector3(3, 3, hit.distance - 4);
        }

        yield return new WaitForSeconds(2.2f);
        trigger.localPosition = new Vector3(0, 0, 0);
        trigger.localScale = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(0.8f);

        gameObject.SetActive(false);
    }
}
