using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{//��Ϸ״̬
    LoadResources,//������Դ
    InitPlayers,//��ʼ���������
    InitUI,//��ʼ��
    WaitForReady,//��һغ�
    StartGame,//��ʼ��Ϸ   Ͷ������˭�ȿ�ʼ��
    EndGame
}
