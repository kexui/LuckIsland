using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    [SerializeField]private float slideSpeed = 10;
    [SerializeField] private GameInput gameInput;

    //private float currentTime = 0;//�Ѿ�����ʱ��
    //private float totalTime = 1;//�ܹ�ʱ��
    private void Update()
    {
        Slide();
    }
    private void Slide()
    {
        Vector3 direction =  gameInput.GetMoveMentDirectionNormalized();
        if (direction != Vector3.zero)
        {
            Vector3 tagetPos = transform.position + direction;
            transform.position = Vector3.Slerp(transform.position, tagetPos, Time.deltaTime * slideSpeed);
        }
    }
}
