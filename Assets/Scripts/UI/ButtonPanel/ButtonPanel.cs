using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPanel : MonoBehaviour
{
    GameObject rollDicePanel;
    GameObject playerTurnPanel;

    GameObject currentPanel;

    private void OnEnable()
    {
        TurnManager.OnTurnStageChanged += OnTurnStageChanged;
    }
    private void Awake()
    {
        rollDicePanel = transform.Find("RollDice").gameObject;
        playerTurnPanel = transform.Find("PlayerTurn").gameObject;

        if (rollDicePanel == null || playerTurnPanel == null)
        {
            Debug.LogError("RollDice button not found in ButtonPanel.");
            return;
        }
        rollDicePanel.SetActive(false);
        playerTurnPanel.SetActive(false);
    }
    private void OnTurnStageChanged(TurnStage stage)
    {
        switch (stage)
        {
            case TurnStage.StartTurn:
                SetCurrentPanel(null);
                break;
            case TurnStage.Wait:
                SetCurrentPanel(null);
                break;
            case TurnStage.RollDice:
                SetCurrentPanel(rollDicePanel);
                break;
            case TurnStage.Move:
                SetCurrentPanel(null);
                break;
            case TurnStage.TriggerTileEvent:
                SetCurrentPanel(null);
                break;
            case TurnStage.PlayerTurn:
                if (TurnManager.Instance.currentPlayerIndex != GameManager.Instance.LocalPlayerIndex) return;
                //如果不是本地玩家的回合，则不显示玩家操作面板
                SetCurrentPanel(playerTurnPanel);
                break;
            case TurnStage.EndTurn:
                SetCurrentPanel(null);
                break;
            default:
                break;
        }
    }

    void SetCurrentPanel(GameObject panel)
    {
        if (currentPanel != null)
        {//隐藏当前panel
            currentPanel.SetActive(false);
        }
        currentPanel = panel;
        if (currentPanel != null)
        {//显示新panel
            currentPanel.SetActive(true);
        }
    }

    public void OnRollClick()
    {//摇骰子按钮点击事件
        GameManager.Instance.LocalPlayer.StopRoll();
        TimerUtility.Instance.StartTimer(0.25f, () => SetCurrentPanel(null));
    }
    public void OnOverPlayerTurnClick()
    {
        TurnManager.Instance.SetOverTurn();
        //SetCurrentPanel(null);
    }
}
