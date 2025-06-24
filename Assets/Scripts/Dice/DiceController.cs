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
            Vector3.forward, //1��
            Vector3.up, //2��
            Vector3.left, //3��
            Vector3.right, //4��
            Vector3.down, //5��
            Vector3.back //6��
        };
}

public class DiceController : MonoBehaviour
{
    public int id { get; private set; }

    private float rollDuration = 7f; // ��������ʱ��
    private float StopRollDuration = 2f; // ֹͣ��������ʱ��
    [SerializeField] private float baseFlipTime = 0.15f; // Ĭ��ת����ʱ����
    private float currentFlipTime; // ʵ��ÿ��ת����ʱ����
    private int flipCount; // ÿ��ת���Ĵ���
    public AnimationCurve perFlipCurve; // ÿ��ת��������
    public AnimationCurve stopCurve;// ֹͣ������


    private bool hasStartStop = false;
    private bool stopRolling = false; // �Ƿ�ֹͣ����

    int lastRan = -1;

    public void StartRollingDice()
    {
        currentFlipTime = baseFlipTime; // ��ʼ����ǰ��תʱ��
        stopRolling = false;
        hasStartStop = false;
        if (stopCurve == null||perFlipCurve == null)
        {
            Debug.LogError("�����ü�������");
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
                Debug.Log("��ʼ����");
                StartCoroutine(StopRoll(StopRollDuration));
            }
            yield return StartRolling();
            if (stopRolling)
            {
                Debug.Log("ֹͣ����");
                DiceManager.Instance.DiceResult(id, GetDicePoint());
                yield break;
            }
        }
    }
    IEnumerator StartRolling()
    {
        Quaternion startRot = transform.rotation; // ��ʼ��ת
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
        stopRolling = true; // ����ֹͣ������־
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
                result = i + 1; // ������1��ʼ
            }
        }
        return result;
    }
    public void SetId(int id)
    {
        this.id = id;
    }
}

