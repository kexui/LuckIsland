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
        private float rollDuration = 8f; // 滚动持续时间
        public float baseFlipTime = 0.1f; // 默认转动的时间间隔
        private float currentFlipTime; // 实际每次转动的时间间隔
        private int flipCount; // 每次转动的次数
        public AnimationCurve perFlipCurve; // 每次转动的曲线
        public AnimationCurve stopCurve;// 停止的曲线


        private bool hasStartStop = false;
        private bool stopRolling = false; // 是否停止滚动

        int lastRan = -1;

        private void Start()
        {
            RollingDice();
        }
        public void RollingDice()
        {
            currentFlipTime = baseFlipTime; // 初始化当前翻转时间
            stopRolling = false;
            hasStartStop = false;
            if (stopCurve == null)
            {
                Debug.LogError("请设置减速曲线");
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
                    Debug.Log("开始减速");
                    StartCoroutine(StopRoll());
                }
                yield return StartRolling();
                if (stopRolling)
                {

                    print(transform.up);
                    Debug.Log("停止滚动");
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
        IEnumerator StopRoll()
        {
            float timer = 0f;
            while (timer < 1f)
            {
                timer += Time.deltaTime / 3f;
                currentFlipTime = Mathf.SmoothStep(baseFlipTime, 2f, stopCurve.Evaluate(timer));
                yield return null;
            }
            stopRolling = true; // 设置停止滚动标志
        }
    }
}

