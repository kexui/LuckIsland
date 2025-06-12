using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekLinker:MonoBehaviour
{
    private float checkDistance = 2f;//检查距离
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

            Ray ray = new Ray(origin, dir);//方向
            if (Physics.Raycast(ray,out RaycastHit hit , checkDistance+0.1f))//（方向，碰撞体，长度）
            {
                T component = hit.collider.GetComponent<T>();//获取碰撞体上的组件
                if (component!=null)
                {
                    return component;
                }
            }

        }
        return null;
    }
}
