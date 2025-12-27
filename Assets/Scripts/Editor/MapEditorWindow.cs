using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using Game.Data.Config;
using Game.Data.Map;
using Game.Utils;
using Game.View.Building;
using UnityEngine;
using UnityEditor;
using Game.View.Map;

namespace Game.Editor.Map
{
    /// <summary>
    /// 地图编辑器窗口
    /// 提供可视化界面来编辑地图方块关系
    /// </summary>
    public class MapEditorWindow : EditorWindow
    {
        // ========== 常量 ==========
        private const string FOLDER_PATH = "Assets/Resources/Configs/MapRuntime"; //配置保存地址
        private const string PREFAB_PATH = "Assets/Resources/Prefabs"; //预设体位置
        private const int MAX_ADJACENT_LANDS = 2; //最大邻居数量
        
        // ========== 窗口字段 ==========
        // 注意：EditorWindow的字段不会自动序列化
        // 使用 EditorPrefs 或 ScriptableObject 来持久化，或者直接使用临时变量
        
        private string configName = "MapConfig"; //默认名
        private float cellSize = 1f;
        private float distanceOffect = 0.1f; //偏移、扩大
        
        private List<TileView> allTiles;
        private List<LandView> allLands;
        private List<BuildingView>allBuildings;
        
        private TileView startTileView; //起点
        private LandView startLandView; //Start下的Land
        private LandView shopLandView; //Shop下的Land
        private Transform buildingRoot;
        
        private Vector2 scrollPosition;
        
        // ========== 窗口打开 ==========

        [MenuItem("Tools/Map/Map Editor")]
        public static void ShowWindow()
        {
            MapEditorWindow window = GetWindow<MapEditorWindow>("Map Editor");//获取或创建窗口
            window.minSize = new Vector2(400, 300);//窗口属性
            window.Show();//显示窗口
        }

        private void OnEnable()
        {
            allTiles = new List<TileView>(FindObjectsOfType<TileView>());
            allLands = new List<LandView>(FindObjectsOfType<LandView>());
            allBuildings = new List<BuildingView>(FindObjectsOfType<BuildingView>());

            UpdataGridPosByPosition();
        }

        private void OnDestroy()
        {
            
        }

        // ========== 窗口绘制 ==========
        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            DrawHeader();
            DrawParameters();
            DrawPath();
            DrawOperations();
            DrawBuildingSettings();
            DrawSaveSettings();

            EditorGUILayout.EndScrollView();
        }
        
        // ========== UI 绘制方法 ==========
        
        //标题
        private void DrawHeader()
        {
            EditorGUILayout.LabelField("地图编辑器", EditorStyles.boldLabel);
            EditorGUILayout.Space();
        }

        //参数
        private void DrawParameters()
        {
            EditorGUILayout.LabelField("计算参数", EditorStyles.boldLabel);
            cellSize = EditorGUILayout.FloatField(
                "CellSize",
                cellSize
            );
            EditorGUILayout.HelpBox(
                "两个Tile之间的距离小于等于此值时，视为邻居",
                MessageType.None
            );
            distanceOffect = EditorGUILayout.FloatField("距离偏移", distanceOffect);
            EditorGUILayout.HelpBox(
                "用于扩大邻居判断范围",
                MessageType.None
            );
            
            EditorGUILayout.Space();
        }

        //路线
        private void DrawPath()
        {
            EditorGUILayout.LabelField("路线设置", EditorStyles.boldLabel);
            //起点 TileVie
            startTileView = (TileView)EditorGUILayout.ObjectField(
                "起点 TileView",
                startTileView,
                typeof(TileView),
                true // 允许场景中的对象
            );

            //示提示框
            if (startTileView != null)
            {
                EditorGUILayout.HelpBox(
                    $"当前起点: Tile: " +
                    $"位置: {startTileView.transform.position}",
                    MessageType.Info
                );
            }
            else
            {
                EditorGUILayout.HelpBox("请从Hierarchy中拖入一个TileView", MessageType.Warning);
            }
            
            GUI.enabled = startTileView != null;
            if (GUILayout.Button("从起点计算出路线", GUILayout.Height(30)))
            {
                CalculateAllNeighbors();
            }

            EditorGUILayout.Space();
        }

        //操作
        private void DrawOperations()
        {
            EditorGUILayout.LabelField("操作", EditorStyles.boldLabel);

            GUI.enabled = true;
            if (GUILayout.Button("刷新所有Views的Pos", GUILayout.Height(30)))
            {
                UpdataGridPosByPosition();
            }
            if (GUILayout.Button("随机分配LandsId", GUILayout.Height(30)))
            {
                RandomLandsID();
            }
            if (GUILayout.Button("确定Tile与Land的邻居关系", GUILayout.Height(30)))
            {
                LinkLandToTile();
            }
            EditorGUILayout.Space();
        }

        //建筑
        private void DrawBuildingSettings()
        {
            EditorGUILayout.LabelField("建筑", EditorStyles.boldLabel);
            
            buildingRoot = (Transform)EditorGUILayout.ObjectField(
                "BuildingRoot",
                buildingRoot,
                typeof(Transform),
                true
            );
            startLandView = (LandView)EditorGUILayout.ObjectField(
                "起点 LandView",
                startLandView,
                typeof(LandView),
                true
            );
            shopLandView = (LandView)EditorGUILayout.ObjectField(
                "商店 LandView",
                shopLandView,
                typeof(LandView),
                true
            );

            if (GUILayout.Button("创建Building", GUILayout.Height(30)))
            {
                BuildBuilding();
            }
            EditorGUILayout.Space();
        }

        //保存
        private void DrawSaveSettings()
        {
            EditorGUILayout.LabelField("保存",EditorStyles.boldLabel);
            //配置保存
            configName = EditorGUILayout.TextField("生成配置名称", configName);
            // 防止空名
            if (string.IsNullOrEmpty(configName))
            {
                EditorGUILayout.HelpBox("配置名称不能为空", MessageType.Warning);
                return;
            }
            if (GUILayout.Button("创建 / 覆盖 MapConfig 资源"))
            {
                CreateOrOverrideConfig();
            }
        }


        // ========== 功能实现 ==========

        /// <summary>
        /// 刷新所有View的Pos数据
        /// </summary>
        private void UpdataGridPosByPosition()
        {
            if (allTiles != null)
            {
                foreach (var tileView in allTiles)
                {
                    if (tileView != null)
                    {
                        tileView.data.Pos = GridHelper.GetGridPosByPosition(tileView.transform.position, cellSize);
                    }
                }
            }

            if (allLands != null)
            {
                foreach (var landView in allLands)
                {
                    if (landView != null)
                    {
                        landView.data.Pos = GridHelper.GetGridPosByPosition(landView.transform.position,cellSize);
                    }
                }
            }

            if (allBuildings != null)
            {
                foreach (var buildingView in allBuildings)
                {
                    buildingView.data.Pos = GridHelper.GetGridPosByPosition(buildingView.transform.position, cellSize);
                }
            }
            
            Debug.Log($"刷新完成: {allTiles.Count} Tiles, {allLands.Count} Lands, {allBuildings.Count} Buildings");
        }
        
        /// <summary>
        /// 从起点开始计算所有Tile的邻居关系
        /// </summary>
        private void CalculateAllNeighbors()
        {
            if (startTileView == null)
            {
                EditorUtility.DisplayDialog("错误", "请先拖入一个TileView作为起点", "确定");
                return;
            }

            List<TileView> processedTiles = new List<TileView>();
            List<TileView> remainingTiles = new List<TileView>(allTiles);

            processedTiles.Add(startTileView);
            remainingTiles.Remove(startTileView);

            int index = 0;
            startTileView.data.TileId = index;
            TileView currentTile = startTileView;

            while (remainingTiles.Count > 0)
            {
                bool foundNeighbor = false;
                for (int i = remainingTiles.Count - 1; i >= 0; i--)
                {
                    var tile = remainingTiles[i];

                    if (IsWithinDistance(currentTile.transform.position, tile.transform.position,cellSize+distanceOffect))
                    {
                        foundNeighbor = true;
                        index++;
                        tile.data.TileId = index;
                        
                        currentTile.data.FrontIndex = index;
                        tile.data.BackIndex = currentTile.data.TileId;

                        currentTile = tile;
                        processedTiles.Add(currentTile);
                        remainingTiles.RemoveAt(i);

                        EditorUtility.SetDirty(tile);
                        break;
                    }
                }

                if (!foundNeighbor)
                {
                    break;
                }
            }
            
            //是否成环
            if (IsWithinDistance(currentTile.transform.position, startTileView.transform.position, cellSize+distanceOffect))
            {
                currentTile.data.FrontIndex = startTileView.data.TileId;
                startTileView.data.BackIndex = currentTile.data.TileId;
                EditorUtility.SetDirty(currentTile);
            }
            
            EditorUtility.SetDirty(startTileView);
            allTiles = processedTiles;

            // 保存
            AssetDatabase.SaveAssets();

            if (remainingTiles.Count > 0)
            {
                EditorUtility.DisplayDialog(
                    "警告",
                    $"已计算{allTiles.Count}个关系，\n还有 {remainingTiles.Count} 个未连接\n",
                    "确定"
                );
            }
            else
            {
                EditorUtility.DisplayDialog(
                    "完成",
                    $"已计算{allTiles.Count}个关系，\n还有 {remainingTiles.Count} 个未连接\n",
                    "确定"
                );
            }
        }

        /// <summary>
        /// 建立Tile和land连接
        /// </summary>
        /// <param name="allTiles"></param>
        /// <param name="allLands"></param>
        private void LinkLandToTile()
        {
            if (startTileView == null)
            {
                EditorUtility.DisplayDialog("错误", "请先拖入一个TileView作为起点", "确定");
                return;
            }

            if (allTiles == null || allTiles.Count == 0)
            {
                EditorUtility.DisplayDialog("错误", "Tile列表为空", "确定");
                return;
            }

            if (allLands == null || allLands.Count == 0)
            {
                EditorUtility.DisplayDialog("警告", "Land列表为空，跳过关联", "确定");
                return;
            }

            TileView currentTile = startTileView;
            int processedCount = 0;
            HashSet<TileView> visited = new HashSet<TileView>();
            while (currentTile != null && !visited.Contains(currentTile))
            {
                visited.Add(currentTile);
                // 确保数组已初始化
                LinkAdjacentLandToTile(currentTile);
                processedCount++;

                // 防止无限循环
                if (processedCount > allTiles.Count)
                {
                    Debug.LogError("检测到循环异常，强制退出");
                    break;
                }

                // 检查 FrontIndex 是否有效
                if (currentTile.data.FrontIndex < 0 || currentTile.data.FrontIndex >= allTiles.Count)
                {
                    Debug.LogWarning(
                        $"Tile {currentTile.data.FrontIndex} 的 FrontIndex ({currentTile.data.FrontIndex}) 无效");
                    break;
                }

                currentTile = allTiles[currentTile.data.FrontIndex];

                if (currentTile == startTileView)
                {
                    break;
                }
            }

            // 保存
            AssetDatabase.SaveAssets();

            EditorUtility.DisplayDialog(
                "完成",
                $"已关联 {processedCount} 个Tile到Land",
                "确定"
            );
            Debug.Log($"地图编辑器: 已关联 {processedCount} 个Tile到Land");
        }

        private void LinkAdjacentLandToTile(TileView tile)
        {
            if (tile.data.AdjacentLandIds == null || tile.data.AdjacentLandIds.Length == 0)
            {
                tile.data.AdjacentLandIds = new int[MAX_ADJACENT_LANDS];
            }

            int count = 0;
            foreach (var land in allLands)
            {
                if (land == null || count >= MAX_ADJACENT_LANDS)
                {
                    break;
                }
                if (IsWithinDistance(tile.transform.position, land.transform.position,cellSize + distanceOffect) )
                {
                    tile.data.AdjacentLandIds[count] = land.GetId();
                    land.data.TileId = tile.GetId();
                    count++;
                }
            }

            if (count > MAX_ADJACENT_LANDS)
            {
                Debug.LogWarning($"Tile {tile.data.TileId} 找到的相邻Land超过数组容量 ({MAX_ADJACENT_LANDS})");
            }
            
            // 标记为已修改
            EditorUtility.SetDirty(tile);
        }

        /// <summary>
        /// 创建Building
        /// </summary>
        private void BuildBuilding()
        {
            bool hasStart = false;
            int index = 0;
            allBuildings = new List<BuildingView>(FindObjectsOfType<BuildingView>());
            if (allBuildings != null && allBuildings.Count != 0)
            {
                foreach (var building in allBuildings)
                {
                    if (building == null)
                    {
                        continue;
                    }
                    if (building.GetId() > index)
                    {
                        index = building.GetId();
                    }

                    if (building.data.Type == BuildingType.Start)
                    {
                        hasStart = true;
                    }
                }
            }

            if (!hasStart)
            {
                if (startLandView == null)
                {
                    EditorUtility.DisplayDialog("错误", "请先拖入一个LandView作为StartBuilding", "确定");
                    return;
                }

                if (startTileView == null)
                {
                    EditorUtility.DisplayDialog("错误", "请先拖入一个TileView作为起点", "确定");
                    return;
                }

                if (startTileView.data.AdjacentLandIds == null ||
                    !startTileView.data.AdjacentLandIds.Contains(startLandView.GetId()))
                {
                    EditorUtility.DisplayDialog("错误", "startLandView与startTileView没有相邻", "确定");
                    return;
                }
                
                var startPrefab = AssetDatabase.LoadAssetAtPath<BuildingView>(PREFAB_PATH + "/Building/Start.prefab");
                if (startPrefab == null)
                {
                    EditorUtility.DisplayDialog("错误", $"找不到起点建筑 Prefab：\n{PREFAB_PATH}/Building/Start.prefab","确定");
                    return;
                }

                BuildingView start = (BuildingView)PrefabUtility.InstantiatePrefab(startPrefab, buildingRoot);
                start.transform.position = startLandView.transform.position + Vector3.up * cellSize;
                allBuildings.Add(start);
            
                start.data.Id = ++index;
                start.data.LandId = startLandView.GetId();
                startLandView.data.BuildingId = start.data.Id;
                EditorUtility.SetDirty(startLandView);
            
                EditorUtility.SetDirty(start);
                EditorUtility.SetDirty(startLandView);
            }

            if (shopLandView == null)
            {
                Debug.LogWarning("ShopLandView 未指定，跳过设置商店");
            }
            else
            {
                var shopPrefab = AssetDatabase.LoadAssetAtPath<BuildingView>(PREFAB_PATH + "/Building/Shop.prefab");
                if (shopPrefab == null)
                {
                    EditorUtility.DisplayDialog("错误", $"找不到商店建筑 Prefab：\n{PREFAB_PATH}/Building/Shop.prefab", "确定");
                    return;
                }

                BuildingView shop = (BuildingView)PrefabUtility.InstantiatePrefab(shopPrefab, buildingRoot);
                allBuildings.Add(shop);
                shop.transform.position = shopLandView.transform.position + Vector3.up * cellSize;
                shop.data.Id = ++index;
                shop.data.LandId = shopLandView.GetId();
                shopLandView.data.BuildingId = shop.data.Id;
                
                EditorUtility.SetDirty(shop);
                EditorUtility.SetDirty(shopLandView);
            }

            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private void CreateOrOverrideConfig()
        {
            // 确保目录存在
            if (!System.IO.Directory.Exists(FOLDER_PATH))
            {
                System.IO.Directory.CreateDirectory(FOLDER_PATH); //创建目录
                AssetDatabase.Refresh();
            }

            string assetPath = $"{FOLDER_PATH}/{configName}.asset";

            //检查是否已存在同名资源
            var mapConfig = AssetDatabase.LoadAssetAtPath<MapRuntimeConfig>(assetPath);
            if (mapConfig != null)
            {
                // 4. 弹出是否覆盖对话框
                bool overwrite = EditorUtility.DisplayDialog(
                    "同名文件已存在",
                    $"路径：{assetPath}\n已存在同名 MapConfig 资源。\n\n是否覆盖？",
                    "覆盖",
                    "取消"
                );

                if (!overwrite)
                {
                    // 选择取消，直接返回
                    return;
                }

                // 想“真正覆盖”，可以删掉老的
                AssetDatabase.DeleteAsset(assetPath);
            }

            // 5. 创建新实例并保存
            var asset = ScriptableObject.CreateInstance<MapRuntimeConfig>();
            asset.tiles = new();
            asset.lands = new();
            asset.buildings = new();
            UpdataGridPosByPosition();

            TileView tileView = startTileView;
            HashSet<TileView> visited = new();
            while (tileView != null && !visited.Contains(tileView))
            {
                visited.Add(tileView);
                asset.tiles.Add(new TileData(tileView.data));

                int nextIndex = tileView.data.FrontIndex;
                if (nextIndex < 0 || nextIndex >= allTiles.Count)
                    break;

                tileView = allTiles[nextIndex];
            }

            foreach (var land in allLands)
            {
                asset.lands.Add(new LandData(land.data));
            }

            foreach (var building in allBuildings)
            {
                asset.buildings.Add(new BuildingData(building.data));
            }

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // 6. 选中并聚焦
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

            Debug.Log($"MapConfig 资源已创建：{assetPath}");
        }

        /// <summary>
        /// 分配随机Id
        /// </summary>
        private void RandomLandsID()
        {
            if (allLands == null || allLands.Count == 0)
            {
                EditorUtility.DisplayDialog("警告", "Land列表为空", "确定");
                return;
            }
            
            System.Random rng = new System.Random();
            int landCount = allLands.Count;
            int[] landIds = new int[landCount];
            for (int i = 0; i < landCount; i++) landIds[i] = i;

            for (int i = landCount - 1; i > 0; i--)
            {
                int j = rng.Next(0, i + 1);
                (landIds[i], landIds[j]) = (landIds[j], landIds[i]);
            }

            for (int i = 0; i < landCount; i++)
            {
                if (allLands[i]?.data == null) continue;
                allLands[i].data.LandId = landIds[i]; // 0 ~ landCount-1 的随机排列
                EditorUtility.SetDirty(allLands[i]);
            }

            AssetDatabase.SaveAssets();
        }

        //相邻距离是否小于指定值
        private bool IsWithinDistance(Vector3 start, Vector3 end,float distance)
        {
            float dis = Vector3.Distance(start, end);
            return dis < distance;
        }
    }
}