using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileBase : MonoBehaviour
{
    private float offsetDistance = 2.02f;//ƫ�ƾ���
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
        hasTileEvent = this is NormalTile ? false : true; //�������ͨ���ӣ���û���¼����������¼�
    }

    public Vector3 GetTopPosition()
    { 
        Vector3 topPosition = transform.position + Vector3.up * offsetDistance;
        return topPosition;
    }

    public abstract IEnumerator TriggerEvent(BasePlayerController pc);//�¼�����

    protected T FindNeighbor<T>(Vector3 pos) where T : Component
    {
        foreach (var dir in directions)
        {
            Vector3 origin = pos + Vector3.up * 0.1f;
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

    //��������¼�
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