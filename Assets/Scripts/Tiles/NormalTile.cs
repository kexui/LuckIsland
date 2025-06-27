using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTile : TileBase
{//普通格子
    public override IEnumerator TriggerEvent(BasePlayerController pc)
    {
        Debug.Log("无事发生");
        yield break;
    }
}
