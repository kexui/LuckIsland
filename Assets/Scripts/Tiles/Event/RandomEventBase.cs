using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 所有随机事件的基类
/// </summary>
public abstract class RandomEventBase : MonoBehaviour
{
    protected string EventName; //事件名称
    protected TextMeshProUGUI eventNameText; //事件名称文本组件
    protected int tileIndex;//所在Tile的下标

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

    public void SetTileIndex(int index)
    { 
        tileIndex = index;
    }
    public abstract IEnumerator TriggerEvent(BasePlayerController player);
}

