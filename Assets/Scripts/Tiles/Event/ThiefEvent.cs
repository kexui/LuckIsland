using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefEvent : EventBase,IInteractable
{//С͵

    private int stealAmout;
    private void Start()
    {
        stealAmout = Random.Range(1, 4);//1~3
    }


    public void Interact(BasePlayerController player)
    {
        if (player.playerData.SubtractCopper(stealAmout))
        {//PC��ҹ�
            print("��ٳɹ�");
        }
        else
        {
            print("���ʧ��");
        }
    }
}
