using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCreashEvent : RandomEventBase
{
    protected override void Start()
    {
        EventName = "����"; // �¼�����
        base.Start();
    }
    public override void TriggerEvent(BasePlayerController player)
    {
        Debug.Log("����¼������ɹ�");
    }
}
