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
                SetCurrentPanel(playerTurnPanel);
                break;
            case TurnStage.EndTurn:
                SetCurrentPanel(null);
                break;
            default:
                break;
        }
    }
    private void Awake()
    {
        rollDicePanel = transform.Find("RollDice").gameObject;
        playerTurnPanel = transform.Find("PlayerTurn").gameObject;
        if (rollDicePanel == null&&playerTurnPanel==null)
        {
            Debug.LogError("RollDice button not found in ButtonPanel.");
        }
        rollDicePanel.SetActive(false);
        playerTurnPanel.SetActive(false);
    }
    void SetCurrentPanel(GameObject panel)
    {
        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
        }
        currentPanel = panel;
        if (currentPanel != null)
        {
            currentPanel.SetActive(true);
        }
        else
        {
            
        }
    }

    public void OnRollClick()
    { 
        GameManager.Instance.localPlayer.StopRoll();
        TimerUtility.Instance.StartTimer(0.25f, () => SetCurrentPanel(null));
    }
    public void OnPlayerTurnClick()
    {
        //Âß¼­
        SetCurrentPanel(null);
    }
}
