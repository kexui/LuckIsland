using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotteryEvent : EventBase, IInteractable
{//��Ʊ
    int Amout; // ��Ʊ�н����
    private void Start()
    {
        Amout = Random.Range(1, 4); // 1~3
    }
    public void Interact(BasePlayerController player)
    {
        
        player.playerData.AddCopper(Amout);
    }
}
