using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTile : TileBase
{

    public override IEnumerator TriggerEvent(BasePlayerController pc)
    {
        
        Debug.Log("��ʼTile");
        yield break;
    }
}
