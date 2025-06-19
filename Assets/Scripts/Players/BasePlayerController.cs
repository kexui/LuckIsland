using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerController : MonoBehaviour
{
    public PlayerData playerData { get; private set;}
    protected bool isMyTurn = false;
    protected bool hasDiceRolled = false;
    protected bool isMoving = false;
    [SerializeField] protected float moveLerpDuration = 1;

    [SerializeField] private float turnSpeed = 10f;
    private bool needsTurn = false;
    private bool hasTurned = false;//是否在转向
    private float turnThreshold = 1;//转向距离

    protected PlayerAnimator playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        if (playerAnimator == null)
        {
            Debug.LogError("PlayerAnimator not found in children of " + gameObject.name);
        }
    }
    protected virtual void Start()
    {
        transform.position = TileManager.Instance.Tiles[playerData.CurrentTileIndex].GetTopPosition();
    }
    protected virtual void OnEnable()
    {
        DiceManager.Instance.OnDiceRolled += DiceManager_OnDiceRolled;
    }
    protected virtual void OnDisable()
    {
        DiceManager.Instance.OnDiceRolled -= DiceManager_OnDiceRolled;
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
    {
        
        isMoving =true;
        playerAnimator.SetWalk(isMoving);

        int direction = playerData.RemainingSteps > 0 ? 1 : -1; //正向或反向移动
        int absSteps = Mathf.Abs(playerData.RemainingSteps);

        for (int i = 0; i < absSteps; i++)
        {//每一步
            int tileCount = TileManager.Instance.Tiles.Count;
            int currentTileIndex = playerData.CurrentTileIndex;
            int nextTileIndex = (currentTileIndex + direction + tileCount) % tileCount;
            int nextNextTileIndex = (currentTileIndex + direction * 2 + tileCount) % tileCount;

            Vector3 start = TileManager.Instance.Tiles[currentTileIndex].GetTopPosition();
            Vector3 end = TileManager.Instance.Tiles[nextTileIndex].GetTopPosition();

            Vector3 endNext = TileManager.Instance.Tiles[nextNextTileIndex].GetTopPosition();
            needsTurn = Vector3.Distance(start, endNext) < 3.9f;

            float timeElapsed = 0f;
            while (timeElapsed < moveLerpDuration)
            {//一步
                timeElapsed += Time.deltaTime;
                float t = Mathf.Clamp01(timeElapsed / moveLerpDuration);
                transform.position = Vector3.Lerp(start, end, t);

                //转向
                float distance = Vector3.Distance(transform.position, end);//与下一个格子的距离
                if (needsTurn)
                {//需要转向并且到达转向距离
                    Vector3 directionToNext = endNext - transform.position;
                    directionToNext.y = 0;
                    if (directionToNext.sqrMagnitude>0.01f)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(directionToNext);
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
                    }
                }
                yield return null;
            }

            transform.position = end; //确保到达终点
            playerData.CurrentTileIndex = nextTileIndex; //更新当前棋子位置
            playerData.RemainingSteps -= direction; //更新剩余步数
            UIManager.Instance.UpdatePlayerDataUI(); //更新UI
            
            Vector3 finalDirection = endNext - end;
            finalDirection.y = 0;
            if (finalDirection.sqrMagnitude>0.1f)
            transform.rotation = Quaternion.LookRotation(finalDirection);
            

            yield return null;
        }
        isMoving = false;
        playerAnimator.SetWalk(isMoving);
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
    public void SetPlayerData(PlayerData playerData)
    {
        this.playerData = playerData;
        playerData.playerController = this;
    }
}
