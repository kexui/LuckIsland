using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public void Interact(BasePlayerController player)
    {
        player.playerData.AddCopper(5);
        Debug.Log("��Ҽ�5");
    }
}
