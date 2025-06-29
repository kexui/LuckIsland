using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;

enum GameMode
{//��Ϸģʽ ��ʱд����  δ����ģʽ1 ��1����ң���3��AI���� ��������������
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
            //������LazyInitialization
            if (_instance == null)
            {//���ʵ��������
             //�����ڳ����в���GameManagerʵ��
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {//���������û��GameManagerʵ�����򴴽�һ���µ�
                    var obj = new GameObject("GameManager");
                    _instance = obj.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    public int LocalPlayerIndex { get; private set; } //�������������Ĭ��Ϊ0
    public BasePlayerController LocalPlayer { get; private set; }

    public GameState currentGameState { get; private set; } = GameState.LoadResources; //��ʼ״̬Ϊ��ʼ��Ϸ
    public int TurnCount { get; private set; }
    public static event Action<PlayerData> OnLocalPlayerSet; //������������¼�

    public static event Action OnLoadResources;
    public static event Action<int[]> OnInitPlayers;
    public static event Action OnInitUI;
    [SerializeField] private GameOverUI gameOverUI;

    private void Awake()
    {
        //ȷ��GameManager�ǵ���ģʽ
        if (_instance != null && _instance != this)
        {//����Ѿ���һ��ʵ�����ڣ����ٵ�ǰʵ��
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);//ȷ��GameManager�ڳ����л�ʱ���ᱻ����
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
    {//�Ƿ���Ҫ�ĳ�Э��
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

        //���ñ�����������ͱ�����ҿ�����
        LocalPlayerIndex = 1;//���豾���������Ϊ1
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
