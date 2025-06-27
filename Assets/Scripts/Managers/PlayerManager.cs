using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private Transform playerSpawnPoint;//��ɫ����λ��   ������չ

    [SerializeField]private List<CharacterData> allCharacters;//��ɫ���ݼ���
    public List<PlayerData> allPlayerDatas = new List<PlayerData>();//������ݼ���
    private List<BasePlayerController> allPlayerControllers = new List<BasePlayerController>();
    

    public int playerCount { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        GameManager.OnInitPlayers += PreGame; //ע����Ϸ��ʼǰ�¼�
    }
    public void PreGame(int[] ints)
    {
        CreatePlayers(ints);
        playerCount = allPlayerDatas.Count;
        foreach (PlayerData playerData in allPlayerDatas)
        {
            allPlayerControllers.Add(playerData.playerController);//��������ҿ�������ӵ��б���
        }
    }
    private void CreatePlayers(int[] ints)
    {
        int id = 0;
        for (int i = 0; i < ints[0]; i++,id++)
        {
            CreatePlayer(id, allCharacters[0],true);
        }
        for (int i = 0; i < ints[1]; i++,id++)
        {
            CreatePlayer(id, allCharacters[1],false);
        }
    }
    private void CreatePlayer(int id,CharacterData characterData,bool isAI)
    {
        
        GameObject model = Instantiate(characterData.modelPrefab, transform.position, Quaternion.identity);
        BasePlayerController playerController;
        PlayerData newPlayerData;
        if (isAI)
        {
            playerController = model.AddComponent<AIPlayerController>();
            newPlayerData = new PlayerData(id, characterData, playerController);
        }
        else
        {
            playerController = model.AddComponent<PlayerController>();
            newPlayerData = new PlayerData(id, characterData, playerController);
        }
        
        allPlayerDatas.Add(newPlayerData);
        Debug.Log("ID: " + id);
    }

    public PlayerData GetPlayerData(int currentPlayerIndex)
    {
        return allPlayerDatas[currentPlayerIndex];
    }
    public PlayerData GetCurrentPlayerData()
    {
        return allPlayerDatas[TurnManager.Instance.currentPlayerIndex];
    }
    public void StartAllPlayersMove()
    {//��ʼ������ҵ��ƶ�Э��
        AudioManager.Instance.StartPlayerWalkSound();
        foreach (PlayerData playerData in allPlayerDatas)
        {
            StartCoroutine(playerData.playerController.MoveCoroutine());
        }
    }
    public bool AllPlayersFinished()
    {
        foreach (BasePlayerController player in allPlayerControllers)
        {
            if (!player.HasFinishedTurn)return false;
        }
        AudioManager.Instance.StopPlayerWalkSound();
        TurnManager.Instance.SetOverTurn();//������Ҷ�����˻غ�
        return true;
    }
    public void StartAllPlayersTriggerTileEvent()
    {
        foreach (PlayerData playerData in allPlayerDatas)
        {
            StartCoroutine(TileManager.Instance.TriggerEvent(playerData.CurrentTileIndex, playerData.playerController));
        }
    }
}
