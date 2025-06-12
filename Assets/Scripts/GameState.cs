using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{//游戏状态
    PreGame,//准备    角色准备
    InitGame,//初始化
    StartGame,//开始游戏   投个骰子谁先开始？
    PlayerTurn,//玩家回合
    EndGame
}
