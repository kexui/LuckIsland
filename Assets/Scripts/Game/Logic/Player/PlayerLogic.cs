using Core.Logic;
using Game.Data.Player;
using UnityEngine;

public class PlayerLogic : LogicBase
{
    private int playerId;
    private string playerName;
    private int gold;
    private int currentTileIndex;
    private string characterPrefabName; // 角色预制体名称
    
    
    private int rollResult;
    //private int remainingSteps;
    
    public PlayerLogic(int id, string name, int startGold, int startTileIndex, string characterName)
    {
        playerId = id;
        playerName = name;
        gold = startGold;
        currentTileIndex = startTileIndex;
        characterPrefabName = characterName;
    }

    public PlayerLogic(PlayerData data)
    {
        playerId = data.playerId;
        playerName = data.playerName;
        gold = data.startGold;
        currentTileIndex = data.startTileIndex;
        characterPrefabName = data.characterPrefabName;
    }

    public override int GetId() => playerId;
    public string GetName() => playerName;
    public int GetGold() => gold;
    public void SetGold(int amount) => gold = amount;
    public void AddGold(int amount) => gold += amount;
    public int GetCurrentTileIndex() => currentTileIndex;
    public void SetCurrentTileIndex(int index) => currentTileIndex = index;
    public string GetCharacterPrefabName() => characterPrefabName;
    public int GetRollResult() => rollResult;
    public void SetRollResult(int result) => rollResult = result;
    //public  int GetRemainingSteps() => remainingSteps;
    //public void SetRemainingSteps(int remaining) => remainingSteps = remaining;
}