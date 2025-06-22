using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public Transform playerSpawnPoint;//��ɫ����λ��   ������չ

    public List<CharacterData> allCharacters;//��ɫ���ݼ���
    public List<PlayerData> allPlayerDatas = new List<PlayerData>();//������ݼ���

    public int playerCount { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void PreGame(int[] ints)
    {
        CreatePlayers(ints);
        playerCount = allPlayerDatas.Count;
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
        if (isAI)
        {
            playerController = model.AddComponent<AIPlayerController>();
        }
        else
        {
            playerController = model.AddComponent<PlayerController>();
        }
        PlayerData newData = new PlayerData(id, characterData, playerController);

        allPlayerDatas.Add(newData);
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
}
