using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekLinker:MonoBehaviour
{
    private float checkDistance = 2f;//������
    private Vector3[] directions =new Vector3[] 
    {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
    };

    public T FindLinker<T>() where T : Component
    { 
        foreach(var dir in directions)
        { 
            Vector3 origin = transform.position;

            Ray ray = new Ray(origin, dir);//����
            if (Physics.Raycast(ray,out RaycastHit hit , checkDistance+0.1f))//��������ײ�壬���ȣ�
            {
                T component = hit.collider.GetComponent<T>();//��ȡ��ײ���ϵ����
                if (component!=null)
                {
                    return component;
                }
            }

        }
        return null;
    }
}
