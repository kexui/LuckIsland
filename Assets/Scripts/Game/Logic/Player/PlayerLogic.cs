using Core.Logic;
using UnityEngine;

public class PlayerLogic : LogicBase
{
    private int playerId;
    private string playerName;
    private int gold;
    private int currentTileIndex;
    private string characterPrefabName; // 角色预制体名称
    
    public PlayerLogic(int id, string name, int startGold, int startTileIndex, string characterName)
    {
        playerId = id;
        playerName = name;
        gold = startGold;
        currentTileIndex = startTileIndex;
        characterPrefabName = characterName;
    }
    
    public override int GetId() => playerId;
    public string GetName() => playerName;
    public int GetGold() => gold;
    public int GetCurrentTileIndex() => currentTileIndex;
    public string GetCharacterPrefabName() => characterPrefabName;
    
    public void SetGold(int amount) => gold = amount;
    public void AddGold(int amount) => gold += amount;
    public void SetCurrentTileIndex(int index) => currentTileIndex = index;
}