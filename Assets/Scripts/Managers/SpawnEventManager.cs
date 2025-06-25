using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEventManager : MonoBehaviour
{
    public List<RandomEventBase> allEvent;
    [Range(0,10)]
    [SerializeField] private int eventCount = 3; // ���ɵ�����¼�����

    private void Start()
    {
        for (int i = 0; i < eventCount; i++)
        {
            int tileIndex = Random.Range(0, TileManager.Instance.Tiles.Count);
            int eventIndex = Random.Range(0, allEvent.Count);
            SpawnEvent(TileManager.Instance.Tiles[tileIndex], allEvent[2]);
        }
        Debug.Log($"����{eventCount}������¼�");
    }

    public void SpawnEvent(TileBase tile,RandomEventBase randomEvent)
    {
        GameObject go = Instantiate(randomEvent.gameObject, tile.GetTopPosition(), Quaternion.identity, tile.transform);
        tile.SetRandomEvent(go.GetComponent<RandomEventBase>());
    }
}
