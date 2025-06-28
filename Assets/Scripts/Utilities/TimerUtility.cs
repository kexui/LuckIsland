using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUtility: MonoBehaviour
{//Э����Ҫ��������
    private static TimerUtility _instance;
    public static TimerUtility Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject("TimerUtility");
                _instance = obj.AddComponent<TimerUtility>();
                DontDestroyOnLoad(obj); //ȷ���ڳ����л�ʱ���ᱻ����
            }
            return _instance;
        }
    }
    public void StartTimer(float duration,Action onComplete,Func<bool> cancelCondition = null)
    { 
        StartCoroutine(TimerCoroutine(duration, onComplete, cancelCondition));
    }
    public IEnumerator TimerCoroutine(float duration,Action onComplete,Func<bool>canelCondition)
    {
        float timer = 0f;
        while (timer<duration)
        {
            if (canelCondition != null && canelCondition()) yield break;
            timer += Time.deltaTime;
            yield return null;
        }
        onComplete?.Invoke(); //������ɻص�
    }
}
