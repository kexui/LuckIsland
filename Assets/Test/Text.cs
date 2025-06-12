using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text : MonoBehaviour
{
    public enum TurnStage
    {
        Waiting,
        RollingDice,
        Moving,
        TileEvent,
        TurnEnded
    }

    public PlayerData playerData;

    private PlayerAnimator playerAnimator;

    private TurnStage currentStage = TurnStage.Waiting;
    private bool isMyTurn = false;
    private float timeElapsed = 0f;

    [SerializeField] private float lerpDuration = 1f;

    public event Action OnTurnEnd;

    private void Start()
    {
        GameInput.Instance.OnDiceAction += HandleDiceInput;
        playerAnimator = GetComponent<PlayerAnimator>();
        transform.position = TileManager.Instance.Tiles[playerData.CurrentTileIndex].GetTopPosition();
    }

    public void StartTurn()
    {
        isMyTurn = true;
        currentStage = TurnStage.RollingDice;
        Debug.Log("�غϿ�ʼ���ȴ����Ͷ����");
    }

    private void HandleDiceInput(object sender, EventArgs e)
    {
        if (!isMyTurn || currentStage != TurnStage.RollingDice) return;
        //Ͷ����
        int dice = DiceManager.Instance.GetDiceResult();
        playerData.RemainingSteps = dice;
        Debug.Log($"Ͷ����: {dice}");

        StartCoroutine(MovePlayer());
    }

    private IEnumerator MovePlayer()
    {
        currentStage = TurnStage.Moving;

        while (playerData.RemainingSteps > 0)
        {
            playerAnimator.Jump();

            Vector3 start = TileManager.Instance.Tiles[playerData.CurrentTileIndex].GetTopPosition();
            int nextIndex = (playerData.CurrentTileIndex + 1) % TileManager.Instance.GetRouteTilesCount();
            Vector3 end = TileManager.Instance.Tiles[nextIndex].GetTopPosition();

            timeElapsed = 0f;
            while (timeElapsed < lerpDuration)
            {
                transform.position = Vector3.Lerp(start, end, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = end;
            playerData.CurrentTileIndex = nextIndex;
            playerData.RemainingSteps--;

            //UIManager.Instance.UpdateCurrentStep(playerData.RemainingSteps);
        }

        currentStage = TurnStage.TileEvent;
        //TileManager.Instance.TriggerEvent(playerData.CurrentTileIndex, this);

        yield return new WaitForSeconds(1f); // �ȴ��¼�Ч�����ţ����类С͵͵Ǯ��

        currentStage = TurnStage.TurnEnded;
        isMyTurn = false;
        OnTurnEnd?.Invoke(); // ֪ͨ TurnManager �л����
    }
}
