using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class RandomEventBase : MonoBehaviour
{
    protected string EventName; //�¼�����
    protected TextMeshProUGUI eventNameText; //�¼������ı����
    protected virtual void Start()
    {
        eventNameText = GetComponentInChildren<TextMeshProUGUI>();
        if (eventNameText == null)
        {
            Debug.LogWarning("RandomEventδ�ҵ�Text");
        }
        else
        {
            eventNameText.text = EventName;
        }
    }
    public abstract IEnumerator TriggerEvent(BasePlayerController player);
}

