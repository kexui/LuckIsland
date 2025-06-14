using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LandBase : MonoBehaviour
{
    private float offsetDistance = 2f; // ƫ�ƾ���
    public Vector3 GetTopPosition()
    {
        Vector3 topPosition = transform.position + Vector3.up * offsetDistance;
        return topPosition;
    }
}
