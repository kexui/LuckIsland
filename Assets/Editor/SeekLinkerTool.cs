#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class LandFinderTool : EditorWindow//�༭������
{
    private float checkDistance = 1f;

    [MenuItem("Tools/�������ܵ�LandBase")]//Tool>�������ܵ�LandBase
    public static void ShowWindow()
    {
        GetWindow<LandFinderTool>("Land Finder");//����
    }

    private void OnGUI()
    {
        GUILayout.Label("�Զ��������ܵ� LandBase �ű�", EditorStyles.boldLabel);
        checkDistance = EditorGUILayout.FloatField("������", checkDistance);

        if (GUILayout.Button("����ѡ�ж������ܵ� LandBase"))
        {
            FindAdjacentLandBases();
        }
    }

    private void FindAdjacentLandBases()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("δѡ���κ����壡");
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
                        Debug.Log($"[{obj.name}] �ڷ��� {dir} ���� LandBase: {land.name}");
                        totalFound++;
                    }
                }
            }
        }
        Debug.Log($"������ɣ��ܹ����� {totalFound} �� LandBase��");
    }
}
#endif
