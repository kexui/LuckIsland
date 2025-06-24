using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefEvent : RandomEventBase
{//С͵

    private int stealAmout;
    protected override void Start()
    {
        stealAmout = Random.Range(1, 4);//1~3
        string richText = TextFormatter.FormatText(stealAmout.ToString()); // ��ʽ��͵�Խ���ı�
        EventName = $"С͵ {richText}"; // �¼�����
        base.Start();
    }
    public override void TriggerEvent(BasePlayerController player)
    {
        if (player.playerData.SubtractCopper(stealAmout))
        {//PC��ҹ�
            print("��ٳɹ�");
        }
        else
        {
            print("���ʧ��");
        }
        Debug.Log("����¼������ɹ�");
    }
}
