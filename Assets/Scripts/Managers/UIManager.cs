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

    //�غ�״̬&��ǰ���
    private void UpdateStageText(TurnStage turnStage)
    {
        turnUI.UpdateTurnStageUI(turnStage);
    }
    private void UpdateCurrentPlayerID(int playerID)
    {
        turnUI.UpdateCurrentPlayerIDUI(playerID);
    }

    public void UpdatePlayerDataUI()
    {//�����Ϣ����  Ͷ����/����һ��
        playerDataUI.UpdatePlayerDataUI();
    }


}
