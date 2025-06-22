using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

namespace Test
{
    

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
    }

    public class TextRolling : MonoBehaviour
    {
        private float rollDuration = 8f; // ��������ʱ��
        public float baseFlipTime = 0.1f; // Ĭ��ת����ʱ����
        private float currentFlipTime; // ʵ��ÿ��ת����ʱ����
        private int flipCount; // ÿ��ת���Ĵ���
        public AnimationCurve perFlipCurve; // ÿ��ת��������
        public AnimationCurve stopCurve;// ֹͣ������


        private bool hasStartStop = false;
        private bool stopRolling = false; // �Ƿ�ֹͣ����

        int lastRan = -1;

        private void Start()
        {
            RollingDice();
        }
        public void RollingDice()
        {
            currentFlipTime = baseFlipTime; // ��ʼ����ǰ��תʱ��
            stopRolling = false;
            hasStartStop = false;
            if (stopCurve == null)
            {
                Debug.LogError("�����ü�������");
                return;
            }
            flipCount = Mathf.Max(1, Mathf.RoundToInt(rollDuration / currentFlipTime));

            StartCoroutine(RollDice());
        }
        public IEnumerator RollDice()
        {
            for (int i = 0; i < flipCount; i++)
            {
                if (i == 40)
                {
                    Debug.Log("��ʼ����");
                    StartCoroutine(StopRoll());
                }
                yield return StartRolling();
                if (stopRolling)
                {

                    print(transform.up);
                    Debug.Log("ֹͣ����");
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
        IEnumerator StopRoll()
        {
            float timer = 0f;
            while (timer < 1f)
            {
                timer += Time.deltaTime / 3f;
                currentFlipTime = Mathf.SmoothStep(baseFlipTime, 2f, stopCurve.Evaluate(timer));
                yield return null;
            }
            stopRolling = true; // ����ֹͣ������־
        }
    }
}

