using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using Game.Data.Config;
using Game.Data.Map;
using Game.View.Building;
using UnityEngine;
using UnityEditor;
using Game.View.Map;

namespace Game.Editor.Map
{
    /// <summary>
    /// 地图编辑器窗口
    /// 提供可视化界面来编辑地图连接关系
    /// </summary>
    public class MapEditorWindow : EditorWindow
    {
        // ========== 窗口字段 ==========
        // 注意：EditorWindow的字段不会自动序列化
        // 使用 EditorPrefs 或 ScriptableObject 来持久化，或者直接使用临时变量
        private TileView startTileView; //起点
        private LandView startLandView; //Start下的Land
        private LandView ShopLandView; //Shop下的Land
        private Transform buildingRoot;
        private List<TileView> allTiles;
        private List<LandView> allLands;
        private List<BuildingView> allBuildings;

        private float neighborDistance = 1f;
        private float distanceOffset = 0.1f;
        private Vector2 scrollPosition;

        private string configName = "MapConfig"; //默认名
        private const string folderPath = "Assets/Resources/Configs/MapRuntime"; //配置保存地址
        private const string PrefabPath = "Assets/Resources/Prefabs";
        
        // ========== 窗口打开 ==========

        [MenuItem("Tools/Map/Map Editor")]
        public static void ShowWindow()
        {
            MapEditorWindow window = GetWindow<MapEditorWindow>("Map Editor");
            window.minSize = new Vector2(400, 300);
            window.Show();
        }

        // ========== 窗口绘制 ==========

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            // 标题，地图编辑器
            EditorGUILayout.LabelField("地图编辑器", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // 标题，起点TileView选择
            EditorGUILayout.LabelField("起点设置", EditorStyles.boldLabel);

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

            EditorGUILayout.Space();

            // 配置参数
            EditorGUILayout.LabelField("计算参数", EditorStyles.boldLabel);
            neighborDistance = EditorGUILayout.FloatField(
                "邻居距离阈值",
                neighborDistance
            );
            EditorGUILayout.HelpBox(
                "两个Tile之间的距离小于等于此值时，视为邻居",
                MessageType.None
            );

            EditorGUILayout.Space();

            // 操作按钮
            EditorGUILayout.LabelField("操作", EditorStyles.boldLabel);

            GUI.enabled = true;
            if (GUILayout.Button("随机分配LandsId", GUILayout.Height(30)))
            {
                RandomLandsID();
            }

            GUI.enabled = startTileView != null;
            if (GUILayout.Button("从起点计算出路线", GUILayout.Height(30)))
            {
                CalculateAllNeighbors();
            }

            GUI.enabled = true;

            if (GUILayout.Button("确定Tile与Land的邻居关系", GUILayout.Height(30)))
            {
                LinkLandToTile();
            }


            buildingRoot = (Transform)EditorGUILayout.ObjectField(
                "buildingRoot",
                buildingRoot,
                typeof(Transform),
                true
            );
            // 使用 ObjectField 来拖入对象
            startLandView = (LandView)EditorGUILayout.ObjectField(
                "起点 LandView",
                startLandView,
                typeof(LandView),
                true // 允许场景中的对象
            );

            ShopLandView = (LandView)EditorGUILayout.ObjectField(
                "商店 LandView",
                ShopLandView,
                typeof(LandView),
                true // 允许场景中的对象
            );

            if (GUILayout.Button("创建Building", GUILayout.Height(30)))
            {
                BuildBuilding();
            }

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

            EditorGUILayout.EndScrollView();
        }

        // ========== 功能实现 ==========

        /// <summary>
        /// 从起点开始计算所有Tile的邻居关系
        /// </summary>
        private void CalculateAllNeighbors()
        {
            allTiles = new List<TileView>(FindObjectsOfType<TileView>());
            allLands = new List<LandView>(FindObjectsOfType<LandView>());

            if (startTileView == null)
            {
                EditorUtility.DisplayDialog("错误", "请先拖入一个TileView作为起点", "确定");
                return;
            }

            List<TileView> tempTileViews = new List<TileView>();

            tempTileViews.Add(startTileView);
            allTiles.Remove(startTileView);

            int index = 0;
            startTileView.data.TileId = index;
            TileView currentTile = startTileView;

            while (allTiles.Count > 0)
            {
                bool foundNeighbor = false;

                for (int i = allTiles.Count - 1; i >= 0; i--)
                {
                    var tile = allTiles[i];
                    float distance = Vector3.Distance(currentTile.transform.position, tile.transform.position);

                    if (distance < neighborDistance + distanceOffset)
                    {
                        foundNeighbor = true;
                        index++;
                        tile.data.TileId = index;

                        currentTile.data.FrontIndex = index;
                        tile.data.BackIndex = currentTile.data.TileId;

                        currentTile = tile;
                        tempTileViews.Add(currentTile);
                        allTiles.RemoveAt(i);

                        EditorUtility.SetDirty(tile);
                        break;
                    }
                }

                if (!foundNeighbor)
                {
                    break;
                }
            }

            EditorUtility.SetDirty(startTileView);

            if (allTiles.Count > 0)
            {
                EditorUtility.DisplayDialog(
                    "警告",
                    $"还有 {allTiles.Count} 个Tile未连接\n",
                    "确定"
                );
                return;
            }

            //判断是否闭环
            float dis = Vector3.Distance(currentTile.transform.position, startTileView.transform.position);
            if (dis > neighborDistance + distanceOffset)
            {
                EditorUtility.DisplayDialog("错误", "路线未闭环", "确定");
                return;
            }

            currentTile.data.FrontIndex = startTileView.data.TileId;
            startTileView.data.BackIndex = currentTile.data.TileId;
            EditorUtility.SetDirty(currentTile);

            allTiles = tempTileViews;

            // 保存
            AssetDatabase.SaveAssets();

            EditorUtility.DisplayDialog(
                "完成",
                $"已计算 {index + 1} 个Tile的邻居关系",
                "确定"
            );

            Debug.Log($"地图编辑器: 已计算 {index + 1} 个Tile的邻居关系");
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
            while (true)
            {
                // 确保数组已初始化
                if (currentTile.data.AdjacentLandIds == null || currentTile.data.AdjacentLandIds.Length == 0)
                {
                    currentTile.data.AdjacentLandIds = new int[2];
                }

                int nums = 0;
                int maxCount = currentTile.data.AdjacentLandIds.Length;

                foreach (var land in allLands)
                {
                    float distance = Vector3.Distance(currentTile.transform.position, land.transform.position);
                    if (distance < neighborDistance)
                    {
                        // 防止数组越界
                        if (nums < maxCount)
                        {
                            currentTile.data.AdjacentLandIds[nums] = land.GetID();
                            land.data.TileId = currentTile.GetId();
                            nums++;
                        }
                        else
                        {
                            Debug.LogWarning($"Tile {currentTile.data.TileId} 找到的相邻Land超过数组容量 ({maxCount})");
                            break; // 数组已满，停止添加
                        }
                    }
                }

                // 标记为已修改
                EditorUtility.SetDirty(currentTile);
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
                    !startTileView.data.AdjacentLandIds.Contains(startLandView.GetID()))
                {
                    EditorUtility.DisplayDialog("错误", "startLandView与startTileView没有相邻", "确定");
                    return;
                }
                
                var startPrefab = AssetDatabase.LoadAssetAtPath<BuildingView>(PrefabPath + "/Building/Start.prefab");
                if (startPrefab == null)
                {
                    EditorUtility.DisplayDialog("错误", $"找不到起点建筑 Prefab：\n{PrefabPath}/Building/Start.prefab","确定");
                    return;
                }

                BuildingView start = (BuildingView)PrefabUtility.InstantiatePrefab(startPrefab, buildingRoot);
                start.transform.position = startLandView.transform.position + Vector3.up * neighborDistance;
            
                start.data.Id = ++index;
                start.data.LandId = startLandView.GetID();
                startLandView.data.BuildingId = start.data.Id;
                EditorUtility.SetDirty(startLandView);
            
                EditorUtility.SetDirty(start);
                EditorUtility.SetDirty(startLandView);
            }

            if (ShopLandView == null)
            {
                Debug.LogWarning("ShopLandView 未指定，跳过设置商店");
            }
            else
            {
                var shopPrefab = AssetDatabase.LoadAssetAtPath<BuildingView>(PrefabPath + "/Building/Shop.prefab");
                if (shopPrefab == null)
                {
                    EditorUtility.DisplayDialog("错误", $"找不到商店建筑 Prefab：\n{PrefabPath}/Building/Shop.prefab", "确定");
                    return;
                }

                BuildingView shop = (BuildingView)PrefabUtility.InstantiatePrefab(shopPrefab, buildingRoot);
                shop.transform.position = ShopLandView.transform.position + Vector3.up * neighborDistance;
                shop.data.Id = ++index;
                shop.data.LandId = ShopLandView.GetID();
                ShopLandView.data.BuildingId = shop.data.Id;
                
                EditorUtility.SetDirty(shop);
                EditorUtility.SetDirty(ShopLandView);
            }

            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private void CreateOrOverrideConfig()
        {
            // 1. 确保目录存在
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath); //创建目录
                AssetDatabase.Refresh();
            }

            // 2. 组合完整路径
            string assetPath = $"{folderPath}/{configName}.asset";

            // 3. 检查是否已存在同名资源
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
            // 重新扫描
            allLands = new List<LandView>(FindObjectsOfType<LandView>());

            // 先准备 0 ~ n-1，再随机洗牌
            System.Random rng = new System.Random();

            // --- Land 随机 ID ---
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
    }
}