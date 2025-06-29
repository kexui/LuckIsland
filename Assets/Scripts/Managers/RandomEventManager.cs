using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
    public static RandomEventManager Instance { get; private set; }

    public List<RandomEventBase> allEvent;//所有随机事件的预设体
    [Range(0,10)]
    [SerializeField] private int eventCount = 3; // 生成的随机事件数量
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
            {//不能重叠
                tileIndex = Random.Range(0, TileManager.Instance.Tiles.Count);
            }
            int eventIndex = Random.Range(0, allEvent.Count);
            SpawnEvent(tileIndex, allEvent[eventIndex]);
        }
        Debug.Log($"生成{eventCount}个随机事件");
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
            Debug.Log("随机事件删除成功");
        }
        else
        {
            Debug.LogWarning("随机事件删除失败！！！RandomEventManager：RemoveMap");
        }
    }
}
