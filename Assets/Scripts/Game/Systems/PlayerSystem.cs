using System.Collections.Generic;
using Core.Systems;
using Game.Data.Player;
using UnityEngine;
using Core.Events;
using Game.Events;
using Game.Systems;
using Game.Utils;


public class PlayerSystem : SystemBase ,IPlayerSystem
{
    private IMapSystem mapSystem;
    public Dictionary<int, PlayerLogic> players = new();
    
    public PlayerSystem(IMapSystem mapSystem)
    {
        this.mapSystem = mapSystem; // 依赖注入
    }
    
    // ========== SystemBase ==========
    
    protected override void OnInitialize()
    {
        // 创建玩家逻辑
    }
    
    protected override void OnEnable()
    {
        // 订阅事件
        SubscribeEvent<DiceRolledEvent>(OnGetDiceResult);
        //SubscribeEvent<Events.PlayerMovedEvent>(OnPlayerMoved);
    }
    
    protected override void OnDisable()
    {
        // 取消订阅在基类的ClearSubscriptions中处理
    }
    protected override void OnCleanup()
    {
        ClearSubscriptions(); // 清理事件订阅
        players.Clear();
    }
    
    // ========== Events ==========
    private void OnGetDiceResult(DiceRolledEvent evt)
    {
        SetPlayerDiceResult(evt.PlayerId, evt.Result);
    }

    // ========== IPlayerSystem ==========

    public void CreatePlayer(PlayerData data)
    {
        CreatePlayer(data.playerId,data.playerName,data.startGold,data.startTileIndex,data.characterPrefabName);
    }

    /// <summary>
    /// 创建玩家
    /// </summary>
    public void CreatePlayer(int playerId, string playerName, int startGold = 500, int startTileIndex = 0, string characterName = "Character_Default")
    {
        ValidateSystem();
        
        // 检查玩家ID是否已存在
        if (HasPlayer(playerId))
        {
            Debug.LogWarning($"玩家 {playerId} 已存在");
            return;
        }
        
        // 验证起始位置
        if (mapSystem != null && mapSystem.GetTile(startTileIndex) == null)
        {
            Debug.LogError($"起始位置 {startTileIndex} 无效");
            startTileIndex = 0; // 默认位置
        }
        
        // 创建玩家逻辑
        PlayerLogic logic = new PlayerLogic(playerId, playerName, startGold, startTileIndex, characterName);
        players.Add(playerId,logic);
        
        // 发布玩家创建事件，让View层响应
        EventBus.Instance?.Publish(new PlayerCreatedEvent
        {
            PlayerId = playerId,
            PlayerLogic = logic
        });

        Debug.Log($"创建玩家: ID={playerId}, Name={playerName}, Gold={startGold}, Tile={startTileIndex}");
    }
    
    
    /// <summary>
    /// 批量加载玩家
    /// </summary>
    public void LoadPlayers(List<PlayerData> playerDataList)
    {
        ValidateSystem();
        
        foreach (var data in playerDataList)
        {
            CreatePlayer(data);
        }
    }

    public bool HasPlayer(int playerId)
    {
        if (!players.ContainsKey(playerId))
        {
            return false;
        }
        return true;
    }

    public bool TryGetPlayer(int playerId, out PlayerLogic player)
    {
        if (!players.TryGetValue(playerId,out player))
        {
            Debug.LogError($"PlayerSystem: 未找到{playerId}");
            return false;
        }
        return true;
    }

    public PlayerLogic GetPlayer(int playerId)
    {
        ValidateSystem();
        PlayerLogic player;
        if (!players.TryGetValue(playerId,out player))
        {
            return null;
        }
        return player;
    }
    
    public List<PlayerLogic> GetAllPlayers()
    {
        ValidateSystem();
        return new List<PlayerLogic>(players.Values);
    }
    
    private void OnPlayerMoved()//Events.PlayerMovedEvent evt)
    {
        // 处理玩家移动事件
    }

    private void SetPlayerDiceResult(int playerId,int result)
    {
        GetPlayer(playerId).SetRollResult(result);
        //GetPlayer(playerId).SetRemainingSteps(result);
    }
}
