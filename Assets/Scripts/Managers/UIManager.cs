using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]private TurnUI turnUI;
    [SerializeField]private PlayerDataUI playerDataUI;

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        TurnManager.OnTurnStageChanged += UpdateStageText;
        TurnManager.OnPlayerChanged += UpdateCurrentPlayerID;
    }

    //回合状态&当前玩家
    private void UpdateStageText(TurnStage turnStage)
    {
        turnUI.UpdateTurnStageUI(turnStage);
    }
    private void UpdateCurrentPlayerID(int playerID)
    {
        turnUI.UpdateCurrentPlayerIDUI(playerID);
    }

    public void UpdatePlayerDataUI()
    {//左侧信息更新  投骰子/走完一步
        playerDataUI.UpdatePlayerDataUI();
    }


}
