using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            //懒加载LazyInitialization
            if (_instance == null)
            {//如果实例不存在
             //尝试在场景中查找GameManager实例
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {//如果场景中没有GameManager实例，则创建一个新的
                    var obj = new GameObject("GameManager");
                    _instance = obj.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    public GameState currentGameState { get; private set; } = GameState.PreGame; //初始状态为开始游戏

    enum GameMode
    {//游戏模式 暂时写在这  未来：模式1 【1（玩家），3（AI）】 这样的数据类型
        AI,
        Player
    }

    private void Awake()
    {
        //确保GameManager是单例模式
        if (_instance != null && _instance != this)
        {//如果已经有一个实例存在，销毁当前实例
            Destroy(gameObject);
            return;
        }
        _instance = this;
        //DontDestroyOnLoad(gameObject);//确保GameManager在场景切换时不会被销毁
    }
    private void Start()
    {
        currentGameState = GameState.PreGame;
        ChangeGameState();
    }

    void ChangeGameState()
    {
        switch (currentGameState)
        {
            case GameState.PreGame:
                PreGame();
                break;
            case GameState.InitGame:
                InitGame();
                break;
            case GameState.StartGame:
                StartGame();
                break;
            case GameState.PlayerTurn:
                PlayerTurn();
                break;
            case GameState.EndGame:
                EndGame();
                break;
            default:
                break;
        }
    }
    private void SetGameState(GameState newState)
    {
        if (newState == currentGameState) return;
        currentGameState = newState;
        ChangeGameState();
    }
    private void PreGame()
    {
        PlayerManager.Instance.PreGame(((int)GameMode.AI), new int[] {1,1 });
        SetGameState(GameState.InitGame);
    }
    private void InitGame()
    {
        SetGameState(GameState.StartGame);
    }
    private void StartGame()
    {

        SetGameState(GameState.PlayerTurn);
    }
    private void PlayerTurn()
    {
        TurnManager.Instance.PlayerTurn();
        //SetGameState(GameState.EndGame);
    }
    private void EndGame()
    { 
    
    }
}
