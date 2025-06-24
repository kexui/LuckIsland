using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEvent : RandomEventBase
{
    private int moveStep = 0;
    protected override void Start()
    {
        while (moveStep == 0)
        {
            moveStep = Random.Range(-3, 4); // �������-3��3֮�������
        }
        string direction = moveStep > 0 ? "ǰ��" : "����";
        string richText = TextFormatter.FormatText(Mathf.Abs(moveStep).ToString()); // ��ʽ�������ı�
        EventName = $"{direction} {richText}"; // �¼�����
        base.Start();
    }
    public override void TriggerEvent(BasePlayerController player)
    {
        player.KnockBack(moveStep);
    }
}
