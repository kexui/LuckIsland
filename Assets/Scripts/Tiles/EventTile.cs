using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTile : TileBase
{
    public override void TriggerEvent(BasePlayerController pc)
    {
        pc.playerData.AddCopper(1); // 触发事件，增加玩家铜币
    }

}
