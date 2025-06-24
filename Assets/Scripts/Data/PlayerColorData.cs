using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerColorData
{
    public static readonly Color[] colors =
    {
        Color.white,
        Color.red,
        Color.blue,
        Color.green
    };
    public static Color GetColor(int index)
    {
        return colors[index % colors.Length];
    }
}
