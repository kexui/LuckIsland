using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefTile : TileBase
{//С͵

    private int stealAmout;
    private void Start()
    {
        stealAmout = Random.Range(1, 4);//1~3
    }
    public override void TriggerEvent(BasePlayerController pc)
    {
        if (pc.playerData.SubtractCopper(stealAmout))
        {//PC��ҹ�
            print("��ٳɹ�");
        }
        else
        {
            print("���ʧ��");
        }
        
    }
}
