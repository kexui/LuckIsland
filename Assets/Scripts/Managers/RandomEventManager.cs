using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
    public static RandomEventManager Instance { get; private set; }

    public List<RandomEventBase> allEvent;//��������¼���Ԥ����
    [Range(0,10)]
    [SerializeField] private int eventCount = 3; // ���ɵ�����¼�����
    private Dictionary<int,RandomEventBase> tileEventMap = new Dictionary<int, RandomEventBase>();

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        GameManager.OnLoadResources += GameManager_OnLoadResources;
    }

    private void GameManager_OnLoadResources()
    {
        SpawnRandomEvent();
    }

    void SpawnRandomEvent()
    {
        for (int i = 0; i < eventCount; i++)
        {
            int tileIndex = Random.Range(0, TileManager.Instance.Tiles.Count);
            while (tileEventMap.ContainsKey(tileIndex))
            {//�����ص�
                tileIndex = Random.Range(0, TileManager.Instance.Tiles.Count);
            }
            int eventIndex = Random.Range(0, allEvent.Count);
            SpawnEvent(tileIndex, allEvent[eventIndex]);
        }
        Debug.Log($"����{eventCount}������¼�");
    }

    public void SpawnEvent(int tileIndex,RandomEventBase randomEventBase)
    {
        TileBase tile = TileManager.Instance.Tiles[tileIndex];
        GameObject go = Instantiate(randomEventBase.gameObject, tile.GetTopPosition(), Quaternion.identity, tile.transform);
        RandomEventBase randomEvent = go.GetComponent<RandomEventBase>();
        randomEvent.SetTileIndex(tileIndex);
        tile.SetRandomEvent(randomEvent);
        tileEventMap.Add(tileIndex, randomEvent);
    }
    public void RemoveRandomEvent(int index)
    {
        if (tileEventMap.ContainsKey(index))
        {
            Destroy(tileEventMap[index].gameObject);
            tileEventMap.Remove(index);
            TileManager.Instance.Tiles[index].ClearRandomEvent();
            Debug.Log("����¼�ɾ���ɹ�");
        }
        else
        {
            Debug.LogWarning("����¼�ɾ��ʧ�ܣ�����RandomEventManager��RemoveMap");
        }
    }
}
