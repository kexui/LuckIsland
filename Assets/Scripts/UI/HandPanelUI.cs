using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*˼·
 ��ʼ��ȷ��������Ҳ�����ҿ������ı䷽��
 
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
    {//ȷ���������
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
    {//ˢ������UI
        foreach (Transform child in transform)
        {//ɾ������
            Destroy(child.gameObject);
        }
        foreach (var card in localplayerData.HandCards)
        {//�������� ���������Ʋ�������
            GameObject go = Instantiate(cardUI.gameObject, transform);
            CardUI newCardUI = go.GetComponent<CardUI>();
            if (newCardUI != null)
            {
                newCardUI.SetData(card);
            }
            else
            {
                Debug.LogError("����δ���ؽű�");
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
            Debug.LogError("����δ���ؽű�");
        }
    }
}
