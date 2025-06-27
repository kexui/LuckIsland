using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class RandomEventBase : MonoBehaviour
{
    protected string EventName; //事件名称
    protected TextMeshProUGUI eventNameText; //事件名称文本组件
    protected virtual void Start()
    {
        eventNameText = GetComponentInChildren<TextMeshProUGUI>();
        if (eventNameText == null)
        {
            Debug.LogWarning("RandomEvent未找到Text");
        }
        else
        {
            eventNameText.text = EventName;
        }
    }
    public abstract IEnumerator TriggerEvent(BasePlayerController player);
}

