using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public Transform playerSpawnPoint;//角色生成位置   后期拓展
    [SerializeField] public GameObject characterPrefab;//角色预设体
    [SerializeField] public GameObject AIplayerPrefab;//AI角色预设体
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
            CreateAIPlayer("AI"+i, id);
        }
        for (int i = 0; i < ints[1]; i++,id++)
        {
            CreatePlayer("Player" + i, id);
        }
    }
    private void CreatePlayer(string name, int id)
    {
        PlayerData newPlayer = new PlayerData(name, id, characterPrefab);
        allPlayerDatas.Add(newPlayer);
        Debug.Log("Player Created: " + name + " with ID: " + id);
    }
    private void CreateAIPlayer(string name,int id)
    { 
        PlayerData newPlayer = new PlayerData(name, id,AIplayerPrefab);
        allPlayerDatas.Add(newPlayer);
        Debug.Log("AI Player Created: " + name + " with ID: " + id);
    }
    public PlayerData GetPlayerData(int currentPlayerIndex)
    {
        return allPlayerDatas[currentPlayerIndex];
    }
    public int GetPlayersCount()
    {
        return allPlayerDatas.Count;
    }   
}
