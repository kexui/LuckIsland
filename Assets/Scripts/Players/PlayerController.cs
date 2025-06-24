using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//玩家移动管理
public class PlayerController : BasePlayerController
{
    
    private void OnEnable()
    {
        GameInput.Instance.OnDiceAction += GameInput_OnDiceAction;
    }

    private void GameInput_OnDiceAction(object sender, System.EventArgs e)
    {
        if (TurnManager.Instance.CurrentStage != TurnStage.RollDice) return;
        //摇
        hasStopRoll = true;

        Debug.Log("PlayerController: Dice Rolled");
    }

    public override IEnumerator RollDice()
    {
        if (playerData.autoRollDIce)
        {//自动摇骰子
            yield return new WaitForSeconds(1f);
            playerData.dice.StartStopRollDice(2f);
            hasStopRoll = true;
            playerData.dice.StartStopRollDice(2f);
            yield return new WaitForSeconds(0.1f);
        }
        else
        {//手动摇骰子
            float timer = 0.0f;
            while (!hasStopRoll)
            {//没摇
                timer += Time.deltaTime;
                if (timer > 5f)
                {//自动摇骰子
                    Debug.Log("PlayerController: Auto Rolling Dice");
                    hasStopRoll = true;
                    playerData.dice.StartStopRollDice(2f);
                    yield break;
                }
                yield return null;//为什么需要？
            }
            playerData.dice.StartStopRollDice(3f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
