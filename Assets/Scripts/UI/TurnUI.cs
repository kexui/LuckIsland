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
    [SerializeField] private TextMeshProUGUI timerText;
    private float totalTime = 10f; // 总时间
    private float currentTime;

    private void OnEnable()
    {
        TurnManager.OnTurnStageChanged += UpdateTurnStageUI;
    }
    public void UpdateTurnStageUI(TurnStage turnStage)
    {
        turnStageText.text = turnStage.ToString();// 更新回合阶段文本
        StartTurn();
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
            if (TurnManager.Instance.OverTurn&&currentTime>1)
            {
                currentTime = 1;
            }
            currentTime -= Time.deltaTime;
            timerText.text = Mathf.CeilToInt(currentTime).ToString(); // 更新计时文本
            timerBar.fillAmount = currentTime / totalTime; // 更新进度条
            yield return null;
        }
        timerText.text = "0"; // 确保计时文本在结束时显示为0
    }
}
