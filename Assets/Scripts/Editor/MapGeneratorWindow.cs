using Game.Data.Config;
using Game.Data.Map;
using Game.Managers;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class MapGeneratorWindow : EditorWindow
    {
        private MapTemplateConfig mapTemplateConfig;//
        private GridManager grid;
        private GameObject tilePrefab;
        private GameObject landPrefab;
        private Transform mapRoot;

        [MenuItem("Tools/Map/Map Generator")]
        public static void ShowWindow()
        {
            GetWindow<MapGeneratorWindow>("Map Generator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Map Generator", EditorStyles.boldLabel);

            mapTemplateConfig =
                (MapTemplateConfig)EditorGUILayout.ObjectField("Map Template", mapTemplateConfig, typeof(MapTemplateConfig), false);
            grid = (GridManager)EditorGUILayout.ObjectField("Grid Manager", grid, typeof(GridManager), true);
            mapRoot = (Transform)EditorGUILayout.ObjectField("Map Root", mapRoot, typeof(Transform), true);

            tilePrefab = (GameObject)EditorGUILayout.ObjectField("Tile Prefab", tilePrefab, typeof(GameObject), false);
            landPrefab = (GameObject)EditorGUILayout.ObjectField("Land Prefab", landPrefab, typeof(GameObject), false);

            EditorGUILayout.Space();

            if (GUILayout.Button("Clear Map"))
            {
                if (mapRoot)
                    ClearMap();
                else
                    Debug.LogWarning("请先设置 MapRoot");
            }

            if (GUILayout.Button("Generate Map"))
            {
                if (mapTemplateConfig && grid && mapRoot)
                    GenerateMap();
                else
                    Debug.LogWarning("请先设置 MapData, GridManager 和 MapRoot");
            }
        }

        /// <summary>
        /// 生成地图
        /// </summary>
        private void GenerateMap()
        {
            GameObject TilesRoot = new GameObject("Tiles");
            TilesRoot.transform.parent = mapRoot;
            foreach (var cell in mapTemplateConfig.TileCells)
            {
                Vector3 worldPos = grid.GetWorldPos(cell);
                if (tilePrefab != null)
                {
                    GameObject tileObj = (GameObject)PrefabUtility.InstantiatePrefab(tilePrefab, TilesRoot.transform);
                    tileObj.transform.position = worldPos;
                }
            }

            GameObject LandsRoot = new GameObject("Lands");
            LandsRoot.transform.parent = mapRoot;
            foreach (var cell in mapTemplateConfig.LandCells)
            {
                Vector3 worldPos = grid.GetWorldPos(cell);
                if (tilePrefab != null)
                {
                    PrefabUtility.InstantiatePrefab(landPrefab, LandsRoot.transform);
                }
            }
        }

        private void ClearMap()
        {
            for (int i = mapRoot.childCount - 1; i >= 0; i--)
                DestroyImmediate(mapRoot.GetChild(i).gameObject);
        }
    }
}