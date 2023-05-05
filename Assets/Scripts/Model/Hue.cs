using System;
using UnityEngine;

[Serializable]
public class Hue
{
    public float h;
    public float s;
    public float v;

    public Hue(float h, float s, float v)
    {
        this.h = h;
        this.s = s;
        this.v = v;
    }
}
