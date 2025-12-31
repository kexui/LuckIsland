using System;
using Game.Managers;
using UnityEngine;
using UnityEngine.UI;

public class RollButton : MonoBehaviour
{
    private Button button;
    private bool canRoll = false;

    private void Start()
    {
        if (TryGetComponent(out button))
        {
            button.onClick.AddListener(OnRollButtonClicked);
        }
    }

    private void OnRollButtonClicked()
    {
        TryRollDice();
    }

    private void TryRollDice()
    {
        if (!canRoll)
        {
            Debug.LogWarning("当前不在投骰子阶段，无法投掷");
            return;
        }

        if (GameManager.Instance?.DiceSystem != null)
        {
            GameManager.Instance.DiceSystem.RequestRollDice(GameManager.Instance.LocalPlayerId);
        }
    }
}
