using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefEventTile : TileBase
{

    public override IEnumerator TriggerEvent(BasePlayerController pc)
    {
        print(" 小偷");// 触发事件逻辑
        yield break;
    }
}
