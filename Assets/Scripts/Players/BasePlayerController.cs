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
        playerData.TotalSteps = riceResult; //�����ܲ���
        playerData.RemainingSteps = riceResult; //����ʣ�ಽ��
    }

    public abstract void StartTurn();//���󷽷�     ��ʼ�غ�
    public abstract void EndTurn();
    public virtual IEnumerator Wait()
    {
        Debug.LogWarning("Э��Waitδ��д");
        yield return null;
    }
    public virtual IEnumerator RollDice()
    {
        /*
        �Ƿ����Զ�ҡ���� 
        1.������ִ���Զ�ҡ���ӣ����������
        2.û��ѭ������Ƿ�ҡ���ӣ���ָ��ʱ���Զ�ҡ����
        */
        Debug.LogWarning("Э��RollDiceδ��д");
        yield return null;
    }
    public IEnumerator Move()
    {//Ӧ�ò�����д�ɣ�
        isMoving = true;
        while (playerData.RemainingSteps > 0)
        {//ÿһ��
            playerAnimator.Jump();
            Vector3 start = TileManager.Instance.Tiles[playerData.CurrentTileIndex].GetTopPosition();//��ʼλ��
            int nextIndex = (playerData.CurrentTileIndex + 1) % TileManager.Instance.GetRouteTilesCount();//Ŀ��index
            Vector3 end = TileManager.Instance.Tiles[nextIndex].GetTopPosition();//Ŀ��λ��
            float timeElapsed = 0;//��ʱ��

            while (timeElapsed < moveLerpDuration)
            {
                timeElapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(start, end, timeElapsed / moveLerpDuration);//��ֵ
                yield return null;
            }

            transform.position = end;
            playerData.CurrentTileIndex = nextIndex;
            playerData.RemainingSteps--;
            UIManager.Instance.UpdatePlayerDataUI();//����UI
            yield return new WaitForSeconds(0.1f);
        }
        //�ƶ���
        isMoving = false;
    }
    public virtual IEnumerator TriggerTileEvent()
    {
        Debug.LogWarning("Э��TriggerTileEventδ��д");
        yield return null;
    }
    protected void Roll()
    { //ҡ
        DiceManager.Instance.RollDice();
    }
}
