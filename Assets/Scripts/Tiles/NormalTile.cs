using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTile : TileBase
{//普通格子
    public override void TriggerEvent(BasePlayerController pc)
    {
        print("无事发生");
    }
}
