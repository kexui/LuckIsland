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
            moveStep = Random.Range(-3, 4); // 随机生成-3到3之间的整数
        }
        string direction = moveStep > 0 ? "前进" : "后退";
        string richText = TextFormatter.FormatText(Mathf.Abs(moveStep).ToString()); // 格式化步数文本
        EventName = $"{direction} {richText}"; // 事件名称
        base.Start();
    }
    public override void TriggerEvent(BasePlayerController player)
    {
        player.KnockBack(moveStep);
    }
}
