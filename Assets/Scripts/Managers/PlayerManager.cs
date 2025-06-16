using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public Transform playerSpawnPoint;//角色生成位置   后期拓展

    public List<CharacterData> allCharacters;

    public List<PlayerData> allPlayerDatas = new List<PlayerData>();//角色数据集合

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        
    }
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }


    public void PreGame(int gameMode, int[] ints)
    {
        switch (gameMode)
        {
            case 0 :
                CreatePlayers(ints);
                break;
            case 1:
                CreatePlayers(ints);
                break;
            default:
                break;
        }
    }
    private void CreatePlayers(int[] ints)
    {
        int id = 0;
        for (int i = 0; i < ints[0]; i++,id++)
        {
            CreatePlayer(id, allCharacters[0]);
        }
        for (int i = 0; i < ints[1]; i++,id++)
        {
            CreateAIPlayer(id, allCharacters[1]);
        }
    }
    private void CreatePlayer(int id,CharacterData characterData)
    {
        PlayerData newData = new PlayerData(id);
        GameObject model = Instantiate(characterData.modelPrefab);
        PlayerController playerController = model.AddComponent<PlayerController>();
        playerController.playerData = newData;
        newData.playerController = playerController;
        allPlayerDatas.Add(newData);
        Debug.Log("ID: " + id);
    }
    private void CreateAIPlayer(int id,CharacterData characterData)
    {
        PlayerData newData = new PlayerData(id);
        GameObject model = Instantiate(characterData.modelPrefab);
        AIPlayerController playerController = model.AddComponent<AIPlayerController>();
        playerController.playerData = newData;
        newData.playerController = playerController;
        allPlayerDatas.Add(newData);
        Debug.Log("ID: " + id);
    }
    public PlayerData GetPlayerData(int currentPlayerIndex)
    {
        return allPlayerDatas[currentPlayerIndex];
    }
    public int GetPlayersCount()
    {
        return allPlayerDatas.Count;
    }
    public PlayerData GetCurrentPlayerData()
    {
        return allPlayerDatas[TurnManager.Instance.currentPlayerIndex];
    }
}
