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
    public void Move(int steps)
    {
        if (isMoving || steps == 0) return;

        playerData.TotalSteps = steps; //设置总步数
        playerData.RemainingSteps = steps; //设置剩余步数
        StartCoroutine(MoveCoroutine());
    }
    public IEnumerator MoveCoroutine()
    {//应该不用重写吧？
        isMoving =true;
        int direction = playerData.RemainingSteps > 0 ? 1 : -1; //正向或反向移动
        int absSteps = Mathf.Abs(playerData.RemainingSteps);

        for (int i = 0; i < absSteps; i++)
        {
            //playerAnimator.Jump();

            int tileCount = TileManager.Instance.Tiles.Count;
            int currentTileIndex = playerData.CurrentTileIndex;
            int nextTileIndex = (currentTileIndex + direction + tileCount) % tileCount;

            Vector3 start = TileManager.Instance.Tiles[currentTileIndex].GetTopPosition();
            Vector3 end = TileManager.Instance.Tiles[nextTileIndex].GetTopPosition();

            float timeElapsed = 0f;
            while (timeElapsed < moveLerpDuration)
            {
                timeElapsed+= Time.deltaTime;
                transform.position = Vector3.Lerp(start, end, timeElapsed/moveLerpDuration);
                yield return null;
            }
            transform.position = end; //确保到达终点
            playerData.CurrentTileIndex = nextTileIndex; //更新当前棋子位置
            playerData.RemainingSteps -= direction; //更新剩余步数
            UIManager.Instance.UpdatePlayerDataUI(); //更新UI
            yield return null;
        }
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
    public void SetCharacter(CharacterData characterData)
    { 
        playerData.SetCharacter(characterData);
        Instantiate(characterData.modelPrefab, transform);
    }    
}
