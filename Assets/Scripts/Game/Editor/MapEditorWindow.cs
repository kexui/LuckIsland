// Assets/Scripts/Game/Editor/Map/MapEditorWindow.cs

using System.Collections.Generic;
using System.Linq;
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
        private TileView startTileView;  // 临时变量，关闭窗口后会丢失
        
        private float neighborDistanceThreshold = 1.1f;
        private Vector2 scrollPosition;
        
        // ========== 窗口打开 ==========
        
        [MenuItem("Tools/Map/Map Editor Window")]
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
                    $"当前起点: Tile {startTileView.tileIndex}\n" +
                    $"位置: {startTileView.Position}",
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
            
            List<TileView> allTiles = new List<TileView>(FindObjectsOfType<TileView>());
            List<LandView> allLands = new List<LandView>(FindObjectsOfType<LandView>());
            
            GUI.enabled = startTileView != null;
            if (GUILayout.Button("从起点计算所有邻居关系", GUILayout.Height(30)))
            {
                CalculateAllNeighbors(allTiles);
            }
            
            GUI.enabled = true;
            
            if (GUILayout.Button("验证地图连接", GUILayout.Height(30)))
            {
                ValidateMapConnections();
            }
            
            if (GUILayout.Button("Tile绑定附件的Land", GUILayout.Height(30)))
            {
                LinkLandToTile(allTiles,allLands);
            }
            
            EditorGUILayout.EndScrollView();
        }
        
        // ========== 功能实现 ==========
        
        /// <summary>
        /// 从起点开始计算所有Tile的邻居关系
        /// </summary>
        private void CalculateAllNeighbors(List<TileView> allTiles)
        {
            if (startTileView == null)
            {
                EditorUtility.DisplayDialog("错误", "请先拖入一个TileView作为起点", "确定");
                return;
            }
            
            allTiles.Remove(startTileView);
            
            int index = 0;
            startTileView.tileIndex = index;
            TileView currentTile = startTileView;
            
            while (allTiles.Count > 0)
            {
                bool foundNeighbor = false;
                
                for (int i = allTiles.Count - 1; i >= 0; i--)
                {
                    var tile = allTiles[i];
                    float distance = Vector3.Distance(currentTile.Position, tile.Position);
            
                    if (distance < neighborDistanceThreshold)
                    {
                        foundNeighbor = true;
                        index++;
                        tile.tileIndex = index;
                
                        currentTile.FrontIndex = index;
                        tile.BackIndex = currentTile.tileIndex;
                
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

            currentTile.FrontIndex = startTileView.tileIndex;
            startTileView.BackIndex = currentTile.tileIndex;
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
        /// 验证地图连接
        /// </summary>
        private void ValidateMapConnections()
        {
            
        }

        private void LinkLandToTile(List<TileView> allTileViews, List<LandView> allLandViews)
        {
            if (startTileView == null)
            {
                EditorUtility.DisplayDialog("错误", "请先拖入一个TileView作为起点", "确定");
                return;
            }
    
            if (allTileViews == null || allTileViews.Count == 0)
            {
                EditorUtility.DisplayDialog("错误", "Tile列表为空", "确定");
                return;
            }
    
            if (allLandViews == null || allLandViews.Count == 0)
            {
                EditorUtility.DisplayDialog("警告", "Land列表为空，跳过关联", "确定");
                return;
            }
            
            // 初始化Land的ID
            for (int i = 0; i < allLandViews.Count; i++)
            {
                allLandViews[i].landId = i;
                EditorUtility.SetDirty(allLandViews[i]);
            }
            
            TileView currentTile = startTileView;
            int processedCount = 0;
            while (true)
            {
                // 确保数组已初始化
                if (currentTile.adjacentLandIds == null || currentTile.adjacentLandIds.Length == 0)
                {
                    currentTile.adjacentLandIds = new int[2];
                }
                
                int nums = 0;
                int maxCount = currentTile.adjacentLandIds.Length;
                
                foreach (var land in allLandViews)
                {
                    float distance = Vector3.Distance(currentTile.Position, land.Position);
                    if (distance < neighborDistanceThreshold)
                    {
                        // 防止数组越界
                        if (nums < maxCount)
                        {
                            currentTile.adjacentLandIds[nums] = land.landId;
                            nums++;
                        }
                        else
                        {
                            Debug.LogWarning($"Tile {currentTile.tileIndex} 找到的相邻Land超过数组容量 ({maxCount})");
                            break; // 数组已满，停止添加
                        }
                    }
                }
                
                // 标记为已修改
                EditorUtility.SetDirty(currentTile);
                processedCount++;
                
                // 防止无限循环
                if (processedCount > allTileViews.Count)
                {
                    Debug.LogError("检测到循环异常，强制退出");
                    break;
                }
                
                // 检查 FrontIndex 是否有效
                if (currentTile.FrontIndex < 0 || currentTile.FrontIndex >= allTileViews.Count)
                {
                    Debug.LogWarning($"Tile {currentTile.tileIndex} 的 FrontIndex ({currentTile.FrontIndex}) 无效");
                    break;
                }
        
                currentTile = allTileViews[currentTile.FrontIndex];
        
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
    }
}