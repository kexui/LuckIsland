using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnStageText;
    [SerializeField] private TextMeshProUGUI playerIDText;

    public void UpdateTurnStageUI(TurnStage turnStage)
    {
        turnStageText.text = turnStage.ToString();
    }
    public void UpdateCurrentPlayerIDUI(int PlayerID)
    {
        playerIDText.text = (PlayerID).ToString();
    }
}
