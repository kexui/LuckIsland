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
    private float totalTime = 10f; // ��ʱ��
    private float currentTime;

    private void OnEnable()
    {
        TurnManager.OnTurnStageChanged += UpdateTurnStageUI;
    }
    public void UpdateTurnStageUI(TurnStage turnStage)
    {
        turnStageText.text = turnStage.ToString();// ���»غϽ׶��ı�
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
            timerText.text = Mathf.CeilToInt(currentTime).ToString(); // ���¼�ʱ�ı�
            timerBar.fillAmount = currentTime / totalTime; // ���½�����
            yield return null;
        }
        timerText.text = "0"; // ȷ����ʱ�ı��ڽ���ʱ��ʾΪ0
    }
}
