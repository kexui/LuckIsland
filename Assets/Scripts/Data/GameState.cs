using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{//游戏状态
    LoadResources,//加载资源
    InitPlayers,//初始化玩家数据
    InitUI,//初始化
    WaitForReady,//玩家回合
    StartGame,//开始游戏   投个骰子谁先开始？
    EndGame
}
