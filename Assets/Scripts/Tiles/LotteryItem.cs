using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotteryItem : MonoBehaviour, IInteractable
{//��Ʊ
    public void Interact(BasePlayerController player)
    {
        Debug.Log("LotteryItem Interacted with by " + player.name);
    }
}
