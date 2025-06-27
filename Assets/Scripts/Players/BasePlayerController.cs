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
    protected float moveLerpDuration = 0.5f; //�ƶ���ֵ����ʱ��
    private bool needsTurn = false;

    public bool HasFinishedTurn { get; private set; }

    protected PlayerAnimator playerAnimator;
    protected AnimationCurve knockbackHeightCurve;

    protected bool isKnockback = false; //�Ƿ������
    protected float knockbackHeight = 3f; //���ɸ߶�

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
        //    Debug.LogWarning("PlayerControllerδ�ҵ�TextMeshProUGUI���");
        //}
        if (playerRing == null)
        {
            Debug.LogWarning("PlayerControllerδ�ҵ�Image���");
        }
        if (knockbackHeightCurve==null)
        {
            Debug.LogWarning("PlayerControllerδ�ҵ����ɸ߶�����");
        }
        if (playerFloatingUI == null)
        {
            Debug.LogWarning("PlayerControllerδ�ҵ�PlayerFloatingUI");
        }

        transform.position = TileManager.Instance.Tiles[playerData.CurrentTileIndex].GetTopPosition();
        isMyTurn = true;
        hasStopRoll = false;
        //playerNameText.text = playerData.PlayerName;
        playerRing.color = PlayerColorData.GetColor(playerData.ID); //���������ɫ
    }

    public virtual IEnumerator RollDice()
    {
        Debug.LogWarning("Э��RollDiceδ��д");
        yield return null;
    }
    public void Move(int steps)
    {
        if (isMoving || steps == 0) return;

        playerData.TotalSteps = steps; //�����ܲ���
        playerData.RemainingSteps = steps; //����ʣ�ಽ��
        StartCoroutine(MoveCoroutine());
    }
    public IEnumerator MoveCoroutine()
    {
        SetMove(true); //��ʼ�ƶ�

        int direction = playerData.RemainingSteps > 0 ? 1 : -1; //��������ƶ�
        int absSteps = Mathf.Abs(playerData.RemainingSteps);

        for (int i = 0; i < absSteps; i++)
        {//ÿһ��
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
            {//һ��
                timeElapsed += Time.deltaTime;
                float t = Mathf.Clamp01(timeElapsed / moveLerpDuration);
                transform.position = Vector3.Lerp(start, end, t);

                //ת��
                float distance = Vector3.Distance(transform.position, end);//����һ�����ӵľ���
                if (needsTurn)
                {//��Ҫת���ҵ���ת�����
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

            transform.position = end; //ȷ�������յ�
            playerData.CurrentTileIndex = nextTileIndex; //���µ�ǰ����λ��
            playerData.RemainingSteps -= direction; //����ʣ�ಽ��
            
            Vector3 finalDirection = endNext - end;
            finalDirection.y = 0;
            if (finalDirection.sqrMagnitude>0.1f)
            transform.rotation = Quaternion.LookRotation(finalDirection);
            

            yield return null;
        }
        SetMove(false); //�ƶ�����
    }
    //public IEnumerator TriggerTileEvent()
    //{
    //    TileManager.Instance.TriggerEvent(playerData.CurrentTileIndex, this);
    //    yield return new WaitForSeconds(0.1f);//���ɶ����¼�����ʱ�����
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
    {//�����ƶ�
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

            Vector3 horizontalPos = Vector3.Lerp(start, end, t); //�򵥵Ĳ�ֵ�ƶ�
            float height = Mathf.Lerp(0, knockbackHeight, knockbackHeightCurve.Evaluate(t));
            transform.position = horizontalPos + Vector3.up * height; //��Ӹ߶�ƫ��
            yield return null;
        }
        transform.position = end; //ȷ�������յ�
        playerData.CurrentTileIndex = endIndex; //���µ�ǰ����λ��
        isKnockback = false; //���ɽ���
        AudioManager.Instance.PlayerKnockDownSound(); //���Ż�����Ч
    }

    private void SetMove(bool isMove)
    { 
        isMoving = isMove;
        playerAnimator.SetWalk(isMoving);
        
        HasFinishedTurn = !isMoving; //��������ƶ�����غϽ���
        if (isMove == false)
        {
            PlayerManager.Instance.AllPlayersFinished();
        }
    }
}
