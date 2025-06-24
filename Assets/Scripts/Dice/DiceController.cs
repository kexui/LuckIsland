using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public static class FlipData
{
    public static readonly Quaternion[] All =
        {
            Quaternion.AngleAxis(90f,Vector3.forward),
            Quaternion.AngleAxis(90f,Vector3.right),
            Quaternion.AngleAxis(90f,Vector3.up),
            Quaternion.AngleAxis(-90f,Vector3.forward),
            Quaternion.AngleAxis(-90f,Vector3.right),
            Quaternion.AngleAxis(-90f,Vector3.up),
        };
    public static readonly Vector3[] faceNormals = 
        {
            Vector3.forward, //1点
            Vector3.up, //2点
            Vector3.left, //3点
            Vector3.right, //4点
            Vector3.down, //5点
            Vector3.back //6点
        };
}

public class DiceController : MonoBehaviour
{
    public int id { get; private set; }

    private float rollDuration = 7f; // 滚动持续时间
    private float StopRollDuration = 2f; // 停止滚动持续时间
    [SerializeField] private float baseFlipTime = 0.15f; // 默认转动的时间间隔
    private float currentFlipTime; // 实际每次转动的时间间隔
    private int flipCount; // 每次转动的次数
    public AnimationCurve perFlipCurve; // 每次转动的曲线
    public AnimationCurve stopCurve;// 停止的曲线


    private bool hasStartStop = false;
    private bool stopRolling = false; // 是否停止滚动

    int lastRan = -1;

    public void StartRollingDice()
    {
        currentFlipTime = baseFlipTime; // 初始化当前翻转时间
        stopRolling = false;
        hasStartStop = false;
        if (stopCurve == null||perFlipCurve == null)
        {
            Debug.LogError("请设置减速曲线");
            return;
        }
        flipCount = Mathf.Max(1, Mathf.RoundToInt(rollDuration / currentFlipTime));

        StartCoroutine(RollDice());
    }
    public void StartStopRollDice(float stopRollTime)
    {
        if (hasStartStop) return;
        StartCoroutine(StopRoll(stopRollTime));
    }
    public IEnumerator RollDice()
    {
        int stopFlipCount = flipCount - Mathf.RoundToInt(StopRollDuration / baseFlipTime);
        for (int i = 0; i < flipCount; i++)
        {
            if (i == stopFlipCount && !hasStartStop)
            {
                Debug.Log("开始减速");
                StartCoroutine(StopRoll(StopRollDuration));
            }
            yield return StartRolling();
            if (stopRolling)
            {
                Debug.Log("停止滚动");
                DiceManager.Instance.DiceResult(id, GetDicePoint());
                yield break;
            }
        }
    }
    IEnumerator StartRolling()
    {
        Quaternion startRot = transform.rotation; // 初始旋转
        Quaternion endRot = startRot * GetAngleAxis();
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / currentFlipTime;
            transform.rotation = Quaternion.Slerp(startRot, endRot, perFlipCurve.Evaluate(t));
            yield return null;
        }
    }
    Quaternion GetAngleAxis()
    {
        int ran = Random.Range(0, FlipData.All.Length);
        int lastRanNeg = (lastRan + 3) % FlipData.All.Length;
        while (ran == lastRanNeg)
        {
            ran = Random.Range(0, FlipData.All.Length);
        }
        lastRan = ran;
        return FlipData.All[ran];
    }
    IEnumerator StopRoll(float stopRollTime)
    {
        hasStartStop = true;
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime / stopRollTime;
            currentFlipTime = Mathf.SmoothStep(baseFlipTime, 2f, stopCurve.Evaluate(timer));
            yield return null;
        }
        stopRolling = true; // 设置停止滚动标志
    }
    
    public int GetDicePoint()
    {
        float maxDot = -1f;
        int result = -1;
        for (int i = 0; i < FlipData.faceNormals.Length; i++)
        {
            Vector3 wordNormal = transform.rotation * FlipData.faceNormals[i];
            float dot = Vector3.Dot(wordNormal, Vector3.up);
            if (dot > maxDot)
            {
                maxDot = dot;
                result = i + 1; // 点数从1开始
            }
        }
        return result;
    }
    public void SetId(int id)
    {
        this.id = id;
    }
}

