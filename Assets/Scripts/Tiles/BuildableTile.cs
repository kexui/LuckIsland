using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class BuildableTile : TileBase
{//产地
    int baseCopper = 1;//基础费用
    int purchaseCopper = 5;//购买费用
    int UpgradeCopper = 3;//升级费用
    BasePlayerController owner;
    BasePlayerController currentPlayer;
    bool isOwned = false;//是否被拥有
    bool isUpgraded = false;//是否升级

    BuildableLand neighborLand;

    protected override void Start()
    {
        base.Start();
        BuildableLand land = FindNeighbor<BuildableLand>(transform.position);
        if (land==null)
        {
            Debug.Log("BuildableTile：FindLinker为空");
        }
        else
        {
            neighborLand = land;
        }
    }
    public override IEnumerator TriggerEvent(BasePlayerController pc)
    {
        currentPlayer = pc;
        if (isOwned)
        {//有人拥有
            if (owner == pc)
            {//自己拥有
                if (isUpgraded) yield break;
                if (GameManager.Instance.LocalPlayer != pc) yield break;
                WorldSpaceUI.Instance.buildPromptUI.Show(transform.position, UpgradeBusiness);
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
                    //！
                }
            }
        }
        else
        {//没人拥有
            if (GameManager.Instance.LocalPlayer != pc) yield break;
            WorldSpaceUI.Instance.buildPromptUI.Show(transform.position,SpawnBusiness); //显示购买UI
        }
        yield return null;
    }
    void SpawnBusiness()
    { //建造
        if (currentPlayer.playerData.SubtractCopper(purchaseCopper))
        {//钱够
            owner = currentPlayer;
            isOwned = true;
            print("你购买了"  + "，花费了" + purchaseCopper + "铜币");
            neighborLand.SpawmBusiness();
        }
        else
        {//钱不够
            print("你没有足够的铜币，无法购买");
            return;
        }
    }
    void UpgradeBusiness()
    {
        if (currentPlayer.playerData.SubtractCopper(UpgradeCopper))
        {
            print("你升级了建造" + "，花费了" + UpgradeCopper + "铜币");
            isUpgraded = true;
            neighborLand.UpgradeBusiness();
        }
        else
        {
            print("你没有足够的铜币，无法升级");
            return;
        }
    }
}
