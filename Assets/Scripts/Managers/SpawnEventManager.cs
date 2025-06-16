using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEventManager : MonoBehaviour
{
    public List<GameObject> allEvent;



    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            int tileIndex = Random.Range(0, TileManager.Instance.Tiles.Count);
            int eventIndex = Random.Range(0, allEvent.Count);
            SpawnEvent(TileManager.Instance.Tiles[tileIndex], allEvent[eventIndex]);
        }
    }

    public void SpawnEvent(TileBase tile,GameObject eventPrefab)
    {
        GameObject go = Instantiate(eventPrefab, tile.GetTopPosition(), Quaternion.identity, tile.transform);
        
    }
}
