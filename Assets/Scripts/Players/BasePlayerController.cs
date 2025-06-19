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
    private bool hasTurned = false;//�Ƿ���ת��
    private float turnThreshold = 1;//ת�����

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
    {
        
        isMoving =true;
        playerAnimator.SetWalk(isMoving);

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
            UIManager.Instance.UpdatePlayerDataUI(); //����UI
            
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
    public void SetPlayerData(PlayerData playerData)
    {
        this.playerData = playerData;
        playerData.playerController = this;
    }
}
