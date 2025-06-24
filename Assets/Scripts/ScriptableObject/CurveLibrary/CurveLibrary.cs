using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CurveLibrary",menuName ="Game/CurveLibrary")]
public class CurveLibrary : ScriptableObject
{
    [Header("��ɫ���ɸ߶�ʱ������")]
    public AnimationCurve knockbackHeightCurve;
    [Header("����ת��ʱ������")]
    public AnimationCurve perFlipCurve;
    [Header("����ֹͣʱ������")]
    public AnimationCurve stopDiceCurve;
}
