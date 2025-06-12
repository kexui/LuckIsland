using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefTile : TileBase
{//小偷

    private int stealAmout;
    private void Start()
    {
        stealAmout = Random.Range(1, 4);//1~3
    }
    public override void TriggerEvent(BasePlayerController pc)
    {
        if (pc.playerData.SubtractCopper(stealAmout))
        {//PC金币够
            print("打劫成功");
        }
        else
        {
            print("打劫失败");
        }
        
    }
}
