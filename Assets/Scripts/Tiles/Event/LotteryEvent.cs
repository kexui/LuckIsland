using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotteryEvent : RandomEventBase
{//彩票
    int Amout; // 彩票中奖金额
    protected override void Start()
    {
        Amout = Random.Range(1, 4); // 1~3
        string richText = TextFormatter.FormatText(Amout.ToString()); // 格式化中奖金额文本
        EventName = $"金币 {richText}"; // 事件名称
        base.Start();
    }

    public override void TriggerEvent(BasePlayerController player)
    {
        player.playerData.AddCopper(Amout);
        Debug.Log("随机事件触发成功");
    }
}
