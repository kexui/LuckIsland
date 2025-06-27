using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasePlayerController : MonoBehaviour
{
    public PlayerData playerData { get; private set;}
    //protected TextMeshProUGUI playerNameText;
    protected Image playerRing;

    protected bool isMyTurn = false;
    protected bool hasStopRoll = false;
    protected bool isMoving = false;
    protected float moveLerpDuration = 0.5f; //移动插值持续时间
    private bool needsTurn = false;

    public bool HasFinishedTurn { get; private set; }

    protected PlayerAnimator playerAnimator;
    protected AnimationCurve knockbackHeightCurve;

    protected bool isKnockback = false; //是否击飞中
    protected float knockbackHeight = 3f; //击飞高度

    public PlayerFloatingUI playerFloatingUI { get; private set; }

    public void Init(PlayerData data)
    {
        playerData = data;
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        //playerNameText = GetComponentInChildren<TextMeshProUGUI>();
        playerRing = GetComponentInChildren<Image>();
        knockbackHeightCurve = CurveData.Library.knockbackHeightCurve;
        HasFinishedTurn = false;
        playerFloatingUI = GetComponentInChildren<PlayerFloatingUI>();

        if (playerAnimator == null)
        {
            Debug.LogError("PlayerAnimator not found in children of " + gameObject.name);
        }
        //if (playerNameText==null)
        //{
        //    Debug.LogWarning("PlayerController未找到TextMeshProUGUI组件");
        //}
        if (playerRing == null)
        {
            Debug.LogWarning("PlayerController未找到Image组件");
        }
        if (knockbackHeightCurve==null)
        {
            Debug.LogWarning("PlayerController未找到击飞高度曲线");
        }
        if (playerFloatingUI == null)
        {
            Debug.LogWarning("PlayerController未找到PlayerFloatingUI");
        }

        transform.position = TileManager.Instance.Tiles[playerData.CurrentTileIndex].GetTopPosition();
        isMyTurn = true;
        hasStopRoll = false;
        //playerNameText.text = playerData.PlayerName;
        playerRing.color = PlayerColorData.GetColor(playerData.ID); //设置玩家颜色
    }

    public virtual IEnumerator RollDice()
    {
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
        SetMove(true); //开始移动

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
            
            Vector3 finalDirection = endNext - end;
            finalDirection.y = 0;
            if (finalDirection.sqrMagnitude>0.1f)
            transform.rotation = Quaternion.LookRotation(finalDirection);
            

            yield return null;
        }
        SetMove(false); //移动结束
    }
    //public IEnumerator TriggerTileEvent()
    //{
    //    TileManager.Instance.TriggerEvent(playerData.CurrentTileIndex, this);
    //    yield return new WaitForSeconds(0.1f);//换成动画事件控制时间最好
    //}

    public virtual IEnumerator DoTurn()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Doturn Over");
    }

    public void StopRoll()
    { 
        hasStopRoll = true;
    }

    public void StartKnockUP(int moveStep)
    {//击飞移动
        StartCoroutine(KnockUp(moveStep));
    }
    public IEnumerator KnockUp(int moveStep)
    {
        isKnockback = true;
        int tileCount = TileManager.Instance.Tiles.Count;
        int currentTileIndex = playerData.CurrentTileIndex;
        int endIndex = (currentTileIndex + moveStep + tileCount) % tileCount;

        Vector3 start = TileManager.Instance.Tiles[currentTileIndex].GetTopPosition();
        Vector3 end = TileManager.Instance.Tiles[endIndex].GetTopPosition();

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Abs(moveStep);

            Vector3 horizontalPos = Vector3.Lerp(start, end, t); //简单的插值移动
            float height = Mathf.Lerp(0, knockbackHeight, knockbackHeightCurve.Evaluate(t));
            transform.position = horizontalPos + Vector3.up * height; //添加高度偏移
            yield return null;
        }
        transform.position = end; //确保到达终点
        playerData.CurrentTileIndex = endIndex; //更新当前棋子位置
        isKnockback = false; //击飞结束
        AudioManager.Instance.PlayerKnockDownSound(); //播放击飞音效
    }

    private void SetMove(bool isMove)
    { 
        isMoving = isMove;
        playerAnimator.SetWalk(isMoving);
        
        HasFinishedTurn = !isMoving; //如果不在移动，则回合结束
        if (isMove == false)
        {
            PlayerManager.Instance.AllPlayersFinished();
        }
    }
}
