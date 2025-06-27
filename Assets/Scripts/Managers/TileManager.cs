using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{//棋格管理
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
    private void SpawnRoad()
    {
        foreach (TileBase Tile in Tiles)
        {
            Vector3 position = Tile.GetTopPosition() + Vector3.up * 0.01f;
            //Instantiate(roadPrefabToSpawn, Tile.GetTopPosition(), Quaternion.identity);
            Instantiate(TilePrefabToSpawn, position, Quaternion.identity, Tile.transform);
        }
    }
    public IEnumerator TriggerEvent(int index,BasePlayerController pc)
    {//Tile触发事件
        //先执行完随机事件 再执行本地事件
        if (Tiles[index].HasRandomEvent)
        {            
            yield return Tiles[index].RandomEvent.TriggerEvent(pc);
            Tiles[index].DestroyRandomEvent();
        }
        yield return Tiles[pc.playerData.CurrentTileIndex].TriggerEvent(pc);

        TurnManager.Instance.SetOverTurn();
    }
}
