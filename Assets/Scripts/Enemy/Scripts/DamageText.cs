using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    private Color color;
    private TextMeshPro damageText;

    private void Awake()
    {
        damageText = GetComponent<TextMeshPro>();
        color = damageText.color;
    }

    public void SetUp(float damage)
    {
        StartCoroutine("DMGText", damage);
    }

    private IEnumerator DMGText(float damage)
    {
        float upSpeed = 2f;
        float alphaSpeed = 2f;
        float lifeTime = 2f;
        float startTime = Time.time;
        while (true)
        {
            damageText.text = damage.ToString();

            this.transform.position += new Vector3(0, upSpeed * Time.deltaTime, 0);
            color.a = Mathf.Lerp(color.a, 0, Time.deltaTime*alphaSpeed);
            damageText.color = color;

            if (Time.time - startTime>=lifeTime)
            {
                Destroy(gameObject);
                yield break;
            }

            yield return null; 
        }
    }
}
