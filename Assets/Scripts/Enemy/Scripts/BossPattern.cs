using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossPattern
{
    public string patternName;
    public float weight;

    public BossPattern(string patternName, float weight)
    {
        this.patternName = patternName;
        this.weight = weight;
    }
}
