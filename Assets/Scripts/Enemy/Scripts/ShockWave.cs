using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    public Transform trigger;

    private void OnEnable()
    {
        StartCoroutine("Control");
    }

    private IEnumerator Control()
    {
        Vector3 upScale = Vector3.one;
        while(trigger.localScale.x < 23)
        {
            upScale.x += 0.29f;
            upScale.z += 0.29f;
            trigger.localScale = upScale;

            yield return null;
        }

        trigger.localScale = Vector3.zero;

        yield return new WaitForSeconds(0.6f);

        gameObject.SetActive(false);
    }
}
