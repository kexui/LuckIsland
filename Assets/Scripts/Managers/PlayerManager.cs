using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public Transform playerSpawnPoint;//角色生成位置   后期拓展

    public List<CharacterData> allCharacters;//角色数据集合
    public List<PlayerData> allPlayerDatas = new List<PlayerData>();//玩家数据集合

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
