using System.Collections.Generic;
using Core.Systems;
using Game.Data.Player;
using UnityEngine;
using Core.Events;
using Game.Events;
using Game.Logic.Map;


public class PlayerSystem : SystemBase ,IPlayerSystem
{
    private IMapSystem mapSystem;
    public List<PlayerLogic> Players;
    
    public PlayerSystem(IMapSystem mapSystem)
    {
        this.mapSystem = mapSystem; // 依赖注入
    }
    
    // ========== SystemBase ==========
    
    protected override void OnInitialize()
    {
        Players = new List<PlayerLogic>();
        // 创建玩家逻辑
    }
    
    protected override void OnEnable()
    {
        // 订阅事件
        //SubscribeEvent<Events.PlayerMovedEvent>(OnPlayerMoved);
    }
    
    protected override void OnDisable()
    {
        // 取消订阅在基类的ClearSubscriptions中处理
    }
    protected override void OnCleanup()
    {
        ClearSubscriptions(); // 清理事件订阅
        Players.Clear();
    }
    
    // ========== IPlayerSystem ==========
    
    /// <summary>
    /// 创建玩家
    /// </summary>
    public PlayerLogic CreatePlayer(int playerId, string playerName, int startGold = 500, int startTileIndex = 0, string characterName = "Character_Default")
    {
        ValidateSystem();
        
        // 检查玩家ID是否已存在
        if (GetPlayer(playerId) != null)
        {
            Debug.LogWarning($"玩家 {playerId} 已存在");
            return GetPlayer(playerId);
        }
        
        // 验证起始位置
        if (mapSystem != null && mapSystem.GetTile(startTileIndex) == null)
        {
            Debug.LogError($"起始位置 {startTileIndex} 无效");
            startTileIndex = 0; // 默认位置
        }
        
        // 创建玩家逻辑
        PlayerLogic player = new PlayerLogic(playerId, playerName, startGold, startTileIndex, characterName);
        Players.Add(player);

        Debug.Log($"创建玩家: ID={playerId}, Name={playerName}, Gold={startGold}, Tile={startTileIndex}");
        
        return player;
    }
    
    public void LoadPlayer(int playerId, string playerName, int startGold = 500, int startTileIndex = 0, string characterName = "Character_Default")
    {
        PlayerLogic player = CreatePlayer(playerId, playerName, startGold, startTileIndex, characterName);
        
        // 发布玩家创建事件，让View层响应
        EventBus.Instance?.Publish(new PlayerCreatedEvent
        {
            PlayerId = playerId,
            PlayerLogic = player
        });
    }
    
    /// <summary>
    /// 批量加载玩家
    /// </summary>
    public void LoadPlayers(List<PlayerData> playerDataList)
    {
        ValidateSystem();
        
        foreach (var data in playerDataList)
        {
            LoadPlayer(data.playerId, data.playerName, data.startGold, data.startTileIndex, data.characterPrefabName);
        }
        
        Debug.Log($"批量加载了 {playerDataList.Count} 个玩家");
    }
    
    
    // ========== 公共接口 ==========
    
    public PlayerLogic GetPlayer(int playerId)
    {
        ValidateSystem();
        return Players.Find(p => p.GetId() == playerId);
    }
    
    public List<PlayerLogic> GetAllPlayers()
    {
        ValidateSystem();
        return new List<PlayerLogic>(Players);
    }
    
    private void OnPlayerMoved()//Events.PlayerMovedEvent evt)
    {
        // 处理玩家移动事件
    }
}
