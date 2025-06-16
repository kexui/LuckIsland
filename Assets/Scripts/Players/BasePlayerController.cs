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
    public void Move(int steps)
    {
        if (isMoving || steps == 0) return;

        playerData.TotalSteps = steps; //�����ܲ���
        playerData.RemainingSteps = steps; //����ʣ�ಽ��
        StartCoroutine(MoveCoroutine());
    }
    public IEnumerator MoveCoroutine()
    {//Ӧ�ò�����д�ɣ�
        isMoving =true;
        int direction = playerData.RemainingSteps > 0 ? 1 : -1; //��������ƶ�
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
            transform.position = end; //ȷ�������յ�
            playerData.CurrentTileIndex = nextTileIndex; //���µ�ǰ����λ��
            playerData.RemainingSteps -= direction; //����ʣ�ಽ��
            UIManager.Instance.UpdatePlayerDataUI(); //����UI
            yield return null;
        }
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
    public void SetCharacter(CharacterData characterData)
    { 
        playerData.SetCharacter(characterData);
        Instantiate(characterData.modelPrefab, transform);
    }    
}
