using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInfoUIManager : MonoBehaviour
{//������Ҷ�Ӧ�����ݶ���
    [SerializeField]private GameObject playerInfoPrefab;
    public Transform contentParent;
    private List<PlayerInfoItemUI> playerUIs = new List<PlayerInfoItemUI>();

    private void Start()
    {
        GeneratePlayerUI();
    }
    void GeneratePlayerUI()
    {//����playerUI
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        playerUIs.Clear();
        foreach (PlayerData playerData in PlayerManager.Instance.allPlayerDatas)
        {
            GameObject go = Instantiate(playerInfoPrefab, contentParent);
            PlayerInfoItemUI ui = go.GetComponent<PlayerInfoItemUI>();
            ui.SetData(playerData);
            playerData.OnDataChanged += () =>
            {
                ui.SetData(playerData);
            };
            playerUIs.Add(ui);
        }
    }

}
