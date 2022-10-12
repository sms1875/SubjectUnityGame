using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 5f;

    private void OnEnable()
    {
        Invoke("WaitForSecondsToDestroy", lifeTime);
    }

    private void WaitForSecondsToDestroy()
    {
        Destroy(gameObject);
    }
}
