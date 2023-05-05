using System;
using UnityEngine;

[Serializable]
public class Hue
{
    public float h;
    public float s;
    public float v;

    private Color color;

    public float H
    {
        get
        {
            return h;
        }

        set
        {
            h = value;
            color = Color.HSVToRGB(h, s, v);
        }
    }

    public float S
    {
        get
        {
            return s;
        }

        set
        {
            s = value;
            color = Color.HSVToRGB(h, s, v);
        }
    }

    public float V
    {
        get
        {
            return v;
        }

        set
        {
            v = value;
            color = Color.HSVToRGB(h, s, v);
        }
    }

    public Color toRGB
    {
        get
        {
            return color;
        }
    }

    public Hue(float h, float s, float v)
    {
        this.h = h;
        this.s = s;
        this.v = v;
        color = color = Color.HSVToRGB(h, s, v);
    }

    //Used when loading hue data from save file. Color classes are not saved.
    public Hue(Hue hue)
    {
        h = hue.h;
        s = hue.s;
        v = hue.v;
        color = color = Color.HSVToRGB(h, s, v);
    }
}
