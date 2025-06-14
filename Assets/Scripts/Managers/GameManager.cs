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

    public GameState currentGameState { get; private set; } = GameState.PreGame; //��ʼ״̬Ϊ��ʼ��Ϸ

    enum GameMode
    {//��Ϸģʽ ��ʱд����  δ����ģʽ1 ��1����ң���3��AI���� ��������������
        AI,
        Player
    }

    private void Awake()
    {
        //ȷ��GameManager�ǵ���ģʽ
        if (_instance != null && _instance != this)
        {//����Ѿ���һ��ʵ�����ڣ����ٵ�ǰʵ��
            Destroy(gameObject);
            return;
        }
        _instance = this;
        //DontDestroyOnLoad(gameObject);//ȷ��GameManager�ڳ����л�ʱ���ᱻ����
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
