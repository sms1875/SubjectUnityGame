using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeneerProjectileSpawner : MonoBehaviour
{
    public GameObject veneerProjectile;
    public int maxVeneer = 8;

    private int veneerCnt = 0;


    public void SetUp()
    {
        StartCoroutine("Control");
    }

    private IEnumerator Control()
    {
        yield return new WaitForSeconds(0.5f);

        int randomNum = Random.Range(3, 5);
        for(int i = 0; i < randomNum; i++)
        {
            if (veneerCnt >= maxVeneer) break;
            GameObject instant = Instantiate(veneerProjectile, transform.position, transform.rotation);
            veneerCnt++;
            StartCoroutine(AutoDestroy(instant));
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator AutoDestroy(GameObject instant)
    {
        yield return new WaitForSeconds(10f);

        Destroy(instant);
        veneerCnt--;
    }
}