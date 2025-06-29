using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// ��������¼��Ļ���
/// </summary>
public abstract class RandomEventBase : MonoBehaviour
{
    protected string EventName; //�¼�����
    protected TextMeshProUGUI eventNameText; //�¼������ı����
    protected int tileIndex;//����Tile���±�

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

    public void SetTileIndex(int index)
    { 
        tileIndex = index;
    }
    public abstract IEnumerator TriggerEvent(BasePlayerController player);
}

