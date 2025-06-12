using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileBase : MonoBehaviour
{
    private float offset = 2;
    public Vector3 GetTopPosition()
    { 
        Vector3 topPosition = transform.position + Vector3.up * offset;
        return topPosition;
    }

    public abstract void TriggerEvent(BasePlayerController pc);
}
