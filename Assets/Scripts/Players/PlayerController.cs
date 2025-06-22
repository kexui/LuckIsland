using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//玩家移动管理
public class PlayerController : BasePlayerController
{
    protected override void Start()
    {
        base.Start();
        isMyTurn = true;
        hasDiceRolled = false;
    }
    private void OnEnable()
    {
        GameInput.Instance.OnDiceAction += GameInput_OnDiceAction;
    }

    private void GameInput_OnDiceAction(object sender, System.EventArgs e)
    {
        if (TurnManager.Instance.CurrentStage != TurnStage.RollDice) return;
        //摇
        hasDiceRolled = true;
        Debug.Log("PlayerController: Dice Rolled");
    }

    public override IEnumerator RollDice()
    {
        if (playerData.autoRollDIce)
        {//自动摇骰子
            hasDiceRolled = true;
            Roll();
            yield return new WaitForSeconds(0.1f);
        }
        else
        {//手动摇骰子
            float timer = 0.0f;
            while (!hasDiceRolled)
            {//没摇
                timer += Time.deltaTime;
                if (timer > 5f)
                {//自动摇骰子
                    Debug.Log("PlayerController: Auto Rolling Dice");
                    hasDiceRolled = true;
                }
                yield return null;//为什么需要？
            }
            Roll();
            yield return new WaitForSeconds(0.1f);
        }
    }
    public override IEnumerator TriggerTileEvent()
    { //引发Tile事件
        TileManager.Instance.TriggerEvent(playerData.CurrentTileIndex, this);
        yield return new WaitForSeconds(0.1f);//换成动画事件控制时间最好
    }
}
