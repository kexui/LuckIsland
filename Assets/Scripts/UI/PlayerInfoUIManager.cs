using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInfoUIManager : MonoBehaviour
{//生成玩家对应的数据对象
    [SerializeField]private GameObject playerInfoPrefab;
    public Transform PlayerInfoUI;

    private List<PlayerInfoItemUI> playerUIs = new List<PlayerInfoItemUI>();

    private void Start()
    {
        GeneratePlayerUI();
    }
    void GeneratePlayerUI()
    {//生成playerUI
        foreach (Transform child in PlayerInfoUI)
        {
            Destroy(child.gameObject);
        }
        playerUIs.Clear();

        foreach (PlayerData playerData in PlayerManager.Instance.allPlayerDatas)
        {
            Debug.Log("Generating UI for player: " + playerData.playerName);
            GameObject go = Instantiate(playerInfoPrefab, PlayerInfoUI);
            PlayerInfoItemUI ui = go.GetComponent<PlayerInfoItemUI>();
            ui.SetData(playerData);
            playerData.OnDataChanged += () =>
            {
                ui.SetData(playerData);
            };
            playerUIs.Add(ui);
            ui.RefreshUI();
        }
    }
}
