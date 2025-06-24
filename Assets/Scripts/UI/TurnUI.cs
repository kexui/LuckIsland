using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnStageText;
    [SerializeField] private Image timerBar;
    private float totalTime = 10f; // 总时间
    private float currentTime;

    private void OnEnable()
    {
        TurnManager.OnTurnStageChanged += UpdateTurnStageUI;
        TurnManager.OnPlayerChanged += StartPlayerTurn;
    }
    public void UpdateTurnStageUI(TurnStage turnStage)
    {
        turnStageText.text = turnStage.ToString();
        StartTurn();
    }
    private void StartPlayerTurn(int obj)
    {
        throw new NotImplementedException();
    }


    public void StartTurn()
    { 
        totalTime = TurnManager.Instance.GetTurnTime();
        currentTime = totalTime;
        StartCoroutine(UpdataTimer());
    }
    private IEnumerator UpdataTimer()
    {
        while (currentTime>0)
        {
            if (TurnManager.Instance.OverTurn) break;
            currentTime -= Time.deltaTime;
            timerBar.fillAmount = currentTime / totalTime; // 更新进度条
            yield return null;
        }
    }
}
