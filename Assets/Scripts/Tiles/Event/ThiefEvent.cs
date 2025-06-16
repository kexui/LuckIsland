using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefEvent : EventBase,IInteractable
{//小偷

    private int stealAmout;
    private void Start()
    {
        stealAmout = Random.Range(1, 4);//1~3
    }


    public void Interact(BasePlayerController player)
    {
        if (player.playerData.SubtractCopper(stealAmout))
        {//PC金币够
            print("打劫成功");
        }
        else
        {
            print("打劫失败");
        }
    }
}
