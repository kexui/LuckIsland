using System.Collections.Generic;
using System.Linq;
using Game.Data.Comfig;
using UnityEngine;
using UnityEditor;
using Game.View.Map;
using System.IO;
using Game.Data.Config;

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
        private TileView startTileView;  // 临时变量，关闭窗口后会丢失
        private LandView startLandView;  //StartBuilding下的Land
        private LandView ShopLandView;  //Shop下的Land
        
        private List<TileView> allTiles;
        private List<LandView> allLands;
        
        private float neighborDistanceThreshold = 1.1f;
        private Vector2 scrollPosition;
        
        private string configName = "MapConfig";//默认名
        private const string folderPath = "Assets/Resources/Configs/MapRuntime";//配置保存地址
        
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
            
            // 标题
            EditorGUILayout.LabelField("地图编辑器", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            // 起点TileView选择
            EditorGUILayout.LabelField("起点设置", EditorStyles.boldLabel);
            
            // 使用 ObjectField 来拖入对象
            startTileView = (TileView)EditorGUILayout.ObjectField(
                "起点 TileView", 
                startTileView, 
                typeof(TileView), 
                true  // 允许场景中的对象
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
            neighborDistanceThreshold = EditorGUILayout.FloatField(
                "邻居距离阈值", 
                neighborDistanceThreshold
            );
            EditorGUILayout.HelpBox(
                "两个Tile之间的距离小于等于此值时，视为邻居",
                MessageType.None
            );
            
            EditorGUILayout.Space();
            
            // 操作按钮
            EditorGUILayout.LabelField("操作", EditorStyles.boldLabel);

            
            var mapConfig = ScriptableObject.CreateInstance<MapRuntimeConfig>();
            AssetDatabase.CreateAsset(mapConfig,"Assets/Resources/Configs/MapRuntime/MapConfig.asset");
            AssetDatabase.SaveAssets();
            allTiles = new List<TileView>(FindObjectsOfType<TileView>());
            allLands = new List<LandView>(FindObjectsOfType<LandView>());
            
            GUI.enabled = true;
            if (GUILayout.Button("随机分配id", GUILayout.Height(30)))
            {
                RandomID(allTiles, allLands);
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
            
            // 使用 ObjectField 来拖入对象
            startLandView = (LandView)EditorGUILayout.ObjectField(
                "起点 LandView", 
                startLandView, 
                typeof(LandView), 
                true  // 允许场景中的对象
            );
            
            ShopLandView = (LandView)EditorGUILayout.ObjectField(
                "商店 LandView", 
                ShopLandView, 
                typeof(LandView), 
                true  // 允许场景中的对象
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
            if (startTileView == null)
            {
                EditorUtility.DisplayDialog("错误", "请先拖入一个TileView作为起点", "确定");
                return;
            }
            
            allTiles.Remove(startTileView);
            
            int index = 0;
            startTileView.Config.TileId = index;
            TileView currentTile = startTileView;
            
            while (allTiles.Count > 0)
            {
                bool foundNeighbor = false;
                
                for (int i = allTiles.Count - 1; i >= 0; i--)
                {
                    var tile = allTiles[i];
                    float distance = Vector3.Distance(currentTile.transform.position, tile.transform.position);
                    
                    if (distance < neighborDistanceThreshold)
                    {
                        foundNeighbor = true;
                        index++;
                        tile.Config.TileId = index;
                
                        currentTile.Config.FrontIndex = index;
                        tile.Config.BackIndex = currentTile.Config.TileId;
                
                        currentTile = tile;
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

            currentTile.Config.FrontIndex = startTileView.Config.TileId;
            startTileView.Config.BackIndex = currentTile.Config.TileId;
            EditorUtility.SetDirty(currentTile); 
            
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
                if (currentTile.Config.AdjacentLandIds == null || currentTile.Config.AdjacentLandIds.Length == 0)
                {
                    currentTile.Config.AdjacentLandIds = new int[2];
                }
                
                int nums = 0;
                int maxCount = currentTile.Config.AdjacentLandIds.Length;
                
                foreach (var land in allLands)
                {
                    float distance = Vector3.Distance(currentTile.transform.position, land.transform.position);
                    if (distance < neighborDistanceThreshold)
                    {
                        // 防止数组越界
                        if (nums < maxCount)
                        {
                            currentTile.Config.AdjacentLandIds[nums] = land.GetID();
                            land.Config.TileId = currentTile.GetId();
                            nums++;
                        }
                        else
                        {
                            Debug.LogWarning($"Tile {currentTile.Config.TileId} 找到的相邻Land超过数组容量 ({maxCount})");
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
                if (currentTile.Config.FrontIndex < 0 || currentTile.Config.FrontIndex >= allTiles.Count)
                {
                    Debug.LogWarning($"Tile {currentTile.Config.FrontIndex} 的 FrontIndex ({currentTile.Config.FrontIndex}) 无效");
                    break;
                }
        
                currentTile = allTiles[currentTile.Config.FrontIndex];
        
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

        private void BuildBuilding()
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

            if (startTileView.Config.TileId == null || !startTileView.Config.AdjacentLandIds.Contains(startLandView.GetID()))
            {
                EditorUtility.DisplayDialog("错误", "startLandView与startTileView没有相邻或尚未生成邻接数据", "确定");
                return;
            }

            //startLandView.BuildingType = BuildingType.Start;
            EditorUtility.SetDirty(startLandView);

            if (ShopLandView == null)
            {
                Debug.LogWarning("ShopLandView 未指定，跳过设置商店");
            }
            else
            {
                //ShopLandView.BuildingType = BuildingType.Shop;
                EditorUtility.SetDirty(ShopLandView);
            }
            
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateOrOverrideConfig()
        {
            // 1. 确保目录存在
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
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
            
            TileView tileView = startTileView;
            for (int i = 0; i < allTiles.Count; i++)
            {
                asset.tiles.Add(new TileConfig(tileView.Config));
                //todo
                //细节问题
                tileView = allTiles[tileView.Config.TileId];
            }

            LandView landView = startLandView;
            for (int i = 0; i < allLands.Count; i++)
            {
                asset.lands.Add(new LandConfig(landView.Config));
            }

            foreach (var land in allLands)
            {
                asset.lands.Add(new LandConfig(land.Config));
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
        private void RandomID(List<TileView> tileViews,List<LandView> landViews)
        {
            for (int i = 0; i < tileViews.Count; i++)
            {
                tileViews[i].Config.TileId = i;
            }
            
            for (int i = 0; i < landViews.Count; i++)
            {
                landViews[i].Config.LandId = i;
            }
        }
    }
}