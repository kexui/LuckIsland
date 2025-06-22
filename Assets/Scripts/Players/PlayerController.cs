using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//����ƶ�����
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
        //ҡ
        hasDiceRolled = true;
        Debug.Log("PlayerController: Dice Rolled");
    }

    public override IEnumerator RollDice()
    {
        if (playerData.autoRollDIce)
        {//�Զ�ҡ����
            hasDiceRolled = true;
            Roll();
            yield return new WaitForSeconds(0.1f);
        }
        else
        {//�ֶ�ҡ����
            float timer = 0.0f;
            while (!hasDiceRolled)
            {//ûҡ
                timer += Time.deltaTime;
                if (timer > 5f)
                {//�Զ�ҡ����
                    Debug.Log("PlayerController: Auto Rolling Dice");
                    hasDiceRolled = true;
                }
                yield return null;//Ϊʲô��Ҫ��
            }
            Roll();
            yield return new WaitForSeconds(0.1f);
        }
    }
    public override IEnumerator TriggerTileEvent()
    { //����Tile�¼�
        TileManager.Instance.TriggerEvent(playerData.CurrentTileIndex, this);
        yield return new WaitForSeconds(0.1f);//���ɶ����¼�����ʱ�����
    }
}
