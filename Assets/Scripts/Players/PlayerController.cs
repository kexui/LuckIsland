using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//����ƶ�����
public class PlayerController : BasePlayerController
{
    
    private void OnEnable()
    {
        GameInput.Instance.OnDiceAction += GameInput_OnDiceAction;
    }

    private void GameInput_OnDiceAction(object sender, System.EventArgs e)
    {
        if (TurnManager.Instance.CurrentStage != TurnStage.RollDice) return;
        //ҡ
        hasStopRoll = true;

        Debug.Log("PlayerController: Dice Rolled");
    }

    public override IEnumerator RollDice()
    {
        if (playerData.autoRollDIce)
        {//�Զ�ҡ����
            yield return new WaitForSeconds(1f);
            playerData.dice.StartStopRollDice(2f);
            hasStopRoll = true;
            playerData.dice.StartStopRollDice(2f);
            yield return new WaitForSeconds(0.1f);
        }
        else
        {//�ֶ�ҡ����
            float timer = 0.0f;
            while (!hasStopRoll)
            {//ûҡ
                timer += Time.deltaTime;
                if (timer > 5f)
                {//�Զ�ҡ����
                    Debug.Log("PlayerController: Auto Rolling Dice");
                    hasStopRoll = true;
                    playerData.dice.StartStopRollDice(2f);
                    yield break;
                }
                yield return null;//Ϊʲô��Ҫ��
            }
            playerData.dice.StartStopRollDice(3f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
