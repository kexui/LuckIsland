using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{//������
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
    public void TriggerEvent(int index,BasePlayerController pc)
    {//Tile�����¼�
        foreach (Transform child in Tiles[index].transform)
        {//�������࣬�����Ƿ��нӿ�
            var interactable = child.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(pc);
            }
        }
        Tiles[index].TriggerEvent(pc);

    }

}
