using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFormatter : MonoBehaviour
{
    public static string FormatText(string text)
    {
        return $"<color=yellow><b>{text}</b></color>";
    }
}
