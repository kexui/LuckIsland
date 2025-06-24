using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefEvent : RandomEventBase
{//小偷

    private int stealAmout;
    protected override void Start()
    {
        stealAmout = Random.Range(1, 4);//1~3
        string richText = TextFormatter.FormatText(stealAmout.ToString()); // 格式化偷窃金额文本
        EventName = $"小偷 {richText}"; // 事件名称
        base.Start();
    }
    public override void TriggerEvent(BasePlayerController player)
    {
        if (player.playerData.SubtractCopper(stealAmout))
        {//PC金币够
            print("打劫成功");
        }
        else
        {
            print("打劫失败");
        }
        Debug.Log("随机事件触发成功");
    }
}
