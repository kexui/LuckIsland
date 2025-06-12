#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class LandFinderTool : EditorWindow//编辑器窗口
{
    private float checkDistance = 1f;

    [MenuItem("Tools/查找四周的LandBase")]//Tool>查找四周的LandBase
    public static void ShowWindow()
    {
        GetWindow<LandFinderTool>("Land Finder");//窗口
    }

    private void OnGUI()
    {
        GUILayout.Label("自动查找四周的 LandBase 脚本", EditorStyles.boldLabel);
        checkDistance = EditorGUILayout.FloatField("检测距离", checkDistance);

        if (GUILayout.Button("查找选中对象四周的 LandBase"))
        {
            FindAdjacentLandBases();
        }
    }

    private void FindAdjacentLandBases()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("未选中任何物体！");
            return;
        }

        int totalFound = 0;

        foreach (GameObject obj in selectedObjects)
        {
            Vector3 origin = obj.transform.position;
            Vector3[] directions = new Vector3[]
            {
                Vector3.forward,
                Vector3.back,
                Vector3.left,
                Vector3.right
            };

            foreach (var dir in directions)
            {
                Vector3 checkPos = origin + dir * checkDistance;
                Collider[] hits = Physics.OverlapSphere(checkPos, 0.1f);

                foreach (var hit in hits)
                {
                    IdleLand land = hit.GetComponent<IdleLand>();
                    if (land != null)
                    {
                        Debug.Log($"[{obj.name}] 在方向 {dir} 发现 LandBase: {land.name}");
                        totalFound++;
                    }
                }
            }
        }
        Debug.Log($"查找完成，总共发现 {totalFound} 个 LandBase。");
    }
}
#endif
