using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCreashEvent : RandomEventBase
{
    protected override void Start()
    {
        EventName = "车祸"; // 事件名称
        base.Start();
    }
    public override IEnumerator TriggerEvent(BasePlayerController player)
    {
        Debug.Log("随机事件触发成功");
        yield break;
    }
}
