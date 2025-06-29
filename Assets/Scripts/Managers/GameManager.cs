using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;

enum GameMode
{//游戏模式 暂时写在这  未来：模式1 【1（玩家），3（AI）】 这样的数据类型
    AI,
    Player
}

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

    public int LocalPlayerIndex { get; private set; } //本地玩家索引，默认为0
    public BasePlayerController LocalPlayer { get; private set; }

    public GameState currentGameState { get; private set; } = GameState.LoadResources; //初始状态为开始游戏
    public int TurnCount { get; private set; }
    public static event Action<PlayerData> OnLocalPlayerSet; //本地玩家设置事件

    public static event Action OnLoadResources;
    public static event Action<int[]> OnInitPlayers;
    public static event Action OnInitUI;
    [SerializeField] private GameOverUI gameOverUI;

    private void Awake()
    {
        //确保GameManager是单例模式
        if (_instance != null && _instance != this)
        {//如果已经有一个实例存在，销毁当前实例
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);//确保GameManager在场景切换时不会被销毁
    }
    private void Start()
    {
        GameStart();
    }
    void GameStart()
    {
        currentGameState = GameState.LoadResources;
        TurnCount = 1;
        ChangeGameState();
    }
    void ChangeGameState()
    {//是否需要改成协程
        switch (currentGameState)
        {
            case GameState.LoadResources:
                LoadResources();
                break;
            case GameState.InitPlayers:
                InitPlayers();
                break;
            case GameState.InitUI:
                InitUI();
                break;
            case GameState.WaitForReady:
                WaitForReady();
                break;
            case GameState.StartGame:
                StartGame();
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
        //if (newState == currentGameState) return;//!!!
        currentGameState = newState;
        ChangeGameState();
    }
    private void LoadResources()
    {
        Debug.Log("LoadingResources");
        OnLoadResources?.Invoke();
        SetGameState(GameState.InitPlayers);
    }
    private void InitPlayers()
    {
        Debug.Log("InitPlayers");
        OnInitPlayers?.Invoke(new int[] { 1, 1});

        //设置本地玩家索引和本地玩家控制器
        LocalPlayerIndex = 1;//假设本地玩家索引为1
        OnLocalPlayerSet?.Invoke(PlayerManager.Instance.allPlayerDatas[LocalPlayerIndex]);
        LocalPlayer = PlayerManager.Instance.allPlayerDatas[LocalPlayerIndex].playerController;

        SetGameState(GameState.InitUI);
    }
    private void InitUI()
    {
        Debug.Log("InitUI");
        OnInitUI?.Invoke();
        SetGameState(GameState.WaitForReady);
    }
    private void WaitForReady()
    {
        Debug.Log("WaitForReady");
        SetGameState(GameState.StartGame);
    }
    private void StartGame()
    {
        Debug.Log("Game Started");
        TurnManager.Instance.StartGame();
        //SetGameState(GameState.EndGame);
    }
    public void EndGame()
    {
        TurnManager.Instance.StopTurn();
        List<PlayerData> playerResult = PlayerManager.Instance.GetRankPlayers();
        gameOverUI.Show(playerResult);
    }
}
