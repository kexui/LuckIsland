using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*思路
 初始化确定本地玩家并绑定玩家卡牌数改变方法
 
*/
public class HandPanelUI : MonoBehaviour
{
    [SerializeField] private CardUI cardUI;
    private PlayerData localplayerData;


    private void OnEnable()
    {
        GameManager.OnLocalPlayerSet += SetLocalPlayer;
    }
    public void SetLocalPlayer(PlayerData data)
    {//确定本地玩家
        localplayerData = data;
        data.OnCardChanged += RefreshUI;
        RefreshUI();
    }
    private void OnDestroy()
    {
        if (localplayerData != null)
        {
            foreach (CardDataBase cardData in localplayerData.HandCards)
            {
                localplayerData.OnCardChanged -= RefreshUI;
            }
        }
    }
    private void RefreshUI()
    {//刷新手牌UI
        foreach (Transform child in transform)
        {//删除手牌
            Destroy(child.gameObject);
        }
        foreach (var card in localplayerData.HandCards)
        {//遍历手牌 生成新手牌并绑定数据
            GameObject go = Instantiate(cardUI.gameObject, transform);
            CardUI newCardUI = go.GetComponent<CardUI>();
            if (newCardUI != null)
            {
                newCardUI.SetData(card);
            }
            else
            {
                Debug.LogError("卡牌未挂载脚本");
            }
        }
    }
    public void AddCardUI(CardDataBase card)
    {
        GameObject go = Instantiate(cardUI.gameObject, transform);
        CardUI newCardUI = go.GetComponent<CardUI>();
        if (newCardUI != null)
        {
            newCardUI.SetData(card);
        }
        else
        {
            Debug.LogError("卡牌未挂载脚本");
        }
    }
}
