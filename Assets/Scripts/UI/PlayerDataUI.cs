using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDataUI : MonoBehaviour
{//������ݸ���
    [SerializeField]private TextMeshProUGUI diceResultText;
    [SerializeField]private TextMeshProUGUI currentStepText;
    [SerializeField] private TextMeshProUGUI copperText;
    private PlayerData playerData;

    private void Start()
    {
        DefaultPlayerDataUI();
    }
    public void UpdatePlayerDataUI()
    {
        if (TurnManager.Instance.currentPlayerIndex!=-1)
        {
            playerData = PlayerManager.Instance.GetPlayerData(TurnManager.Instance.currentPlayerIndex);
            diceResultText.text = "������" + playerData.TotalSteps;
            currentStepText.text = "������" + playerData.GetCurrentStep();
            copperText.text = "��ң�" + playerData.Copper;
        }
        DefaultPlayerDataUI();
    }
    public void DefaultPlayerDataUI()
    {
        diceResultText.text = "������";
        currentStepText.text = "������";
        copperText.text = "��ң�";
    }
}
