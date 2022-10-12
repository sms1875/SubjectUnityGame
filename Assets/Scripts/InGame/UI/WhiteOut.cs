using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhiteOut : MonoBehaviour
{
    private Image image;
    private Color color;

    private void Awake()
    {
        color = Color.white;
        color.a = 0;
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (color.a <= 0.99)
        {
            color.a += 0.004f;
            image.color = color;
        }
    }
    private void OnDisable()
    {
        color.a = 0;
    }
}
