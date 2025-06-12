using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerController : MonoBehaviour
{
    public PlayerData playerData;
    protected bool isMyTurn = false;
    protected bool hasDiceRolled = false;
    protected bool isMoving = false;
    [SerializeField] protected float moveLerpDuration = 1;

    protected PlayerAnimator playerAnimator;

    protected virtual void Start()
    {
        transform.position = TileManager.Instance.Tiles[playerData.CurrentTileIndex].GetTopPosition();
        playerAnimator = GetComponent<PlayerAnimator>();
    }
    protected virtual void OnEnable()
    {
        DiceManager.Instance.OnDiceRolled += DiceManager_OnDiceRolled;
    }

    private void DiceManager_OnDiceRolled(int riceResult)
    {
        playerData.TotalSteps = riceResult; //设置总步数
        playerData.RemainingSteps = riceResult; //设置剩余步数
    }

    public abstract void StartTurn();//抽象方法     开始回合
    public abstract void EndTurn();
    public virtual IEnumerator Wait()
    {
        Debug.LogWarning("协程Wait未重写");
        yield return null;
    }
    public virtual IEnumerator RollDice()
    {
        /*
        是否开启自动摇骰子 
        1.开启，执行自动摇骰子（随机动画）
        2.没，循环检测是否摇骰子，到指定时间自动摇骰子
        */
        Debug.LogWarning("协程RollDice未重写");
        yield return null;
    }
    public IEnumerator Move()
    {//应该不用重写吧？
        isMoving = true;
        while (playerData.RemainingSteps > 0)
        {//每一步
            playerAnimator.Jump();
            Vector3 start = TileManager.Instance.Tiles[playerData.CurrentTileIndex].GetTopPosition();//起始位置
            int nextIndex = (playerData.CurrentTileIndex + 1) % TileManager.Instance.GetRouteTilesCount();//目标index
            Vector3 end = TileManager.Instance.Tiles[nextIndex].GetTopPosition();//目标位置
            float timeElapsed = 0;//计时器

            while (timeElapsed < moveLerpDuration)
            {
                timeElapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(start, end, timeElapsed / moveLerpDuration);//插值
                yield return null;
            }

            transform.position = end;
            playerData.CurrentTileIndex = nextIndex;
            playerData.RemainingSteps--;
            UIManager.Instance.UpdatePlayerDataUI();//更新UI
            yield return new WaitForSeconds(0.1f);
        }
        //移动完
        isMoving = false;
    }
    public virtual IEnumerator TriggerTileEvent()
    {
        Debug.LogWarning("协程TriggerTileEvent未重写");
        yield return null;
    }
    protected void Roll()
    { //摇
        DiceManager.Instance.RollDice();
    }
}
