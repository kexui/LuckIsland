using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTile : TileBase
{//ÆÕÍ¨¸ñ×Ó
    public override void TriggerEvent(BasePlayerController pc)
    {
        foreach (Transform child in transform)
        {
            var interactable = child.GetComponent<IInteractable>();
            if (interactable!=null)
            {
                interactable.Interact(pc);
            }
        }
    }
}
