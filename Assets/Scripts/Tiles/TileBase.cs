using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileBase : MonoBehaviour
{
    private float offsetDistance = 2f;//检查距离
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

            Ray ray = new Ray(origin, dir);//方向
            if (Physics.Raycast(ray, out RaycastHit hit, offsetDistance + 0.1f))//（方向，碰撞体，长度）
            {
                T component = hit.collider.GetComponent<T>();//获取碰撞体上的组件
                if (component != null)
                {
                    return component;
                }
            }

        }
        return null;
    }

}