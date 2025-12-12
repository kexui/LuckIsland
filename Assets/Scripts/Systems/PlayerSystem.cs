using System.Collections;
using System.Collections.Generic;
using Core;
using Core.Systems;
using UnityEngine;

public class PlayerSystem : SystemBace
{
    private MapSystem mapSystem;
    public List<PlayerLogic> Players;
    
    public PlayerSystem(MapSystem mapSystem)
    {
        this.mapSystem = mapSystem; // 依赖注入
    }
    
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
    
    // ========== 公共接口 ==========
    public PlayerLogic GetPlayer(int playerId)
    {
        ValidateSystem();
        return Players.Find(p => p.playerId == playerId);
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
