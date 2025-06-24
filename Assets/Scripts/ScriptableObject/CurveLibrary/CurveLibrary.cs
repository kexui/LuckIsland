using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CurveLibrary",menuName ="Game/CurveLibrary")]
public class CurveLibrary : ScriptableObject
{
    [Header("角色击飞高度时间曲线")]
    public AnimationCurve knockbackHeightCurve;
    [Header("骰子转面时间曲线")]
    public AnimationCurve perFlipCurve;
    [Header("骰子停止时间曲线")]
    public AnimationCurve stopDiceCurve;
}
