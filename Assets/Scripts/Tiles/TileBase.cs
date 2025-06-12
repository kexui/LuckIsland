using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileBase : MonoBehaviour
{
    private float offsetDistance = 2f;//������
    private Vector3[] directions = new Vector3[]
    {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
    };

    public Vector3 GetTopPosition()
    { 
        Vector3 topPosition = transform.position + Vector3.up * offsetDistance;
        return topPosition;
    }

    public abstract void TriggerEvent(BasePlayerController pc);

    protected T FindLinker<T>() where T : Component
    {
        foreach (var dir in directions)
        {
            Vector3 origin = transform.position;

            Ray ray = new Ray(origin, dir);//����
            if (Physics.Raycast(ray, out RaycastHit hit, offsetDistance + 0.1f))//��������ײ�壬���ȣ�
            {
                T component = hit.collider.GetComponent<T>();//��ȡ��ײ���ϵ����
                if (component != null)
                {
                    return component;
                }
            }

        }
        return null;
    }

}