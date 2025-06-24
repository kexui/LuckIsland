using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurveData
{
    private static CurveLibrary _Library;
    public static CurveLibrary Library
    {
        get
        {
            if (_Library == null)
            {
#if UNITY_EDITOR
                _Library = UnityEditor.AssetDatabase.LoadAssetAtPath<CurveLibrary>("Assets/ScriptableObject/CurveLibrary/CurveLibrary.asset");
#endif
            }
            return _Library;
        }
    }
}
