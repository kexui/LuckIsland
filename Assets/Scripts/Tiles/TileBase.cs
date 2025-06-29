using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileBase : MonoBehaviour
{
    private float offsetDistance = 2.02f;//偏移距离
    private Vector3[] directions = new Vector3[]
    {
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right
    };
    protected bool hasTileEvent;
    public bool HasTileEvent => hasTileEvent;

    protected bool hasRandomEvent;
    public bool HasRandomEvent => hasRandomEvent;

    protected RandomEventBase randomEvent;
    public RandomEventBase RandomEvent => randomEvent;

    protected virtual void Start()
    {
        Init();
    }
    protected virtual void Init()
    {
        hasTileEvent = this is NormalTile ? false : true; //如果是普通格子，则没有事件，否则有事件
    }

    public Vector3 GetTopPosition()
    { 
        Vector3 topPosition = transform.position + Vector3.up * offsetDistance;
        return topPosition;
    }

    public abstract IEnumerator TriggerEvent(BasePlayerController pc);//事件触发

    protected T FindNeighbor<T>(Vector3 pos) where T : Component
    {
        foreach (var dir in directions)
        {
            Vector3 origin = pos + Vector3.up * 0.1f;
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

    //设置随机事件
    public void SetRandomEvent(RandomEventBase randomEvent)
    {
        if (randomEvent == null)
        {
            Debug.LogError("RandomEventBase is null");
            return;
        }
        this.randomEvent = randomEvent;
        hasRandomEvent = true;
    }
    public void ClearRandomEvent()
    { 
        hasRandomEvent = false;
        randomEvent = null;
    }
}