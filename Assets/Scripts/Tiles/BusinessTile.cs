using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class BusinessTile : TileBase
{//产地
    int baseCopper = 1;//基础费用
    int purchaseCopper = 5;//购买费用
    BasePlayerController owner;
    bool isOwned = false;//是否被拥有
    public override void TriggerEvent(BasePlayerController pc)
    {
        if (isOwned)
        {//有人拥有
            if (owner == pc)
            {//自己拥有
                print("这是你的产地");
            }
            else
            {//别人拥有
                if (pc.playerData.SubtractCopper(baseCopper))
                {//PC金币够
                    owner.playerData.AddCopper(baseCopper);
                    print("你被收取了" + baseCopper + "铜币");
                }
                else
                {
                    print("你没有足够的铜币，无法支付");
                }
            }
        }
        else
        {//没人拥有
            if (pc.playerData.SubtractCopper(purchaseCopper))
            {//PC金币够
                isOwned = true;
                owner = pc;
                print("你购买了这个产地");
            }
            else
            {
                print("你没有足够的铜币，无法购买");
            }
        }
    }
    public void ChangeLand()
    {//有待思考
        LandBase go = FindLinker<LandBase>();
        //if (go != null) ;

    }
}
