using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTile : TileBase
{
    ShopLand neighborLand;
    private void Awake()
    {
        
    }
    protected override void Start()
    {
        base.Start();
        ShopLand land = FindNeighbor<ShopLand>(transform.position);
        if (land == null)
        {
            Debug.Log("ShopTile£ºFindLinkerÎª¿Õ");
        }
        else
        {
            neighborLand = land;
        }
    }

    public override IEnumerator TriggerEvent(BasePlayerController pc)
    {
        print("Çë¹ºÂò¿¨ÅÆ");
        yield break;
    }

    
}
