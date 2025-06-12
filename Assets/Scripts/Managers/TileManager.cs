using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{//∆Â∏Òπ‹¿Ì
    public static TileManager Instance;

    [SerializeField] private Transform TileParent;
    [SerializeField] private GameObject TilePrefabToSpawn;

    public List<TileBase> Tiles = new List<TileBase>();

    
    private void Awake()
    {
        Instance = this;
        BuildRouteFromChlidren();
    }
    private void Start()
    {
        //SpawnRoad();
    }
    void BuildRouteFromChlidren()
    {
        Tiles.Clear();
        foreach (Transform child in TileParent)
        {
            TileBase routeTileBace = child.GetComponent<TileBase>();
            if (routeTileBace != null)
            {
                Tiles.Add(routeTileBace);
            }
        }
    }
    public int GetRouteTilesCount()
    {
        return Tiles.Count;
    }
    private void SpawnRoad()
    {
        foreach (TileBase Tile in Tiles)
        {
            Vector3 position = Tile.GetTopPosition() + Vector3.up * 0.01f;
            //Instantiate(roadPrefabToSpawn, Tile.GetTopPosition(), Quaternion.identity);
            Instantiate(TilePrefabToSpawn, position, Quaternion.identity, Tile.transform);
        }
    }
    public void TriggerEvent(int index,BasePlayerController pc)
    {
        Tiles[index].TriggerEvent(pc);
    }

}
