using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotteryEvent : RandomEventBase
{//��Ʊ
    int Amout; // ��Ʊ�н����
    protected override void Start()
    {
        Amout = Random.Range(1, 4); // 1~3
        string richText = TextFormatter.FormatText(Amout.ToString()); // ��ʽ���н�����ı�
        EventName = $"��� {richText}"; // �¼�����
        base.Start();
    }

    public override void TriggerEvent(BasePlayerController player)
    {
        player.playerData.AddCopper(Amout);
        Debug.Log("����¼������ɹ�");
    }
}
