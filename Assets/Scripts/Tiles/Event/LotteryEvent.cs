using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotteryEvent : EventBase, IInteractable
{//²ÊÆ±
    int Amout; // ²ÊÆ±ÖÐ½±½ð¶î
    private void Start()
    {
        Amout = Random.Range(1, 4); // 1~3
    }
    public void Interact(BasePlayerController player)
    {
        
        player.playerData.AddCopper(Amout);
    }
}
