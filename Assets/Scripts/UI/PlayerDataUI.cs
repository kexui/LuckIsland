using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDataUI : MonoBehaviour
{//玩家数据更新
    [SerializeField]private TextMeshProUGUI diceResultText;
    [SerializeField]private TextMeshProUGUI currentStepText;
    [SerializeField] private TextMeshProUGUI copperText;
    private PlayerData playerData;
    
    public void UpdatePlayerDataUI()
    {
        playerData = PlayerManager.Instance.GetPlayerData(TurnManager.Instance.currentPlayerIndex);
        diceResultText.text = "点数：" + playerData.TotalSteps;
        currentStepText.text = "步数：" + playerData.GetCurrentStep();
        copperText.text = "金币：" + playerData.Copper;
    }
}
