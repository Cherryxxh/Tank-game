using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 批量移除预制体上的 Missing Script
/// 菜单路径: Tools → Remove Missing Scripts
/// </summary>
public class RemoveMissingScripts : EditorWindow
{
    private string folderPath = "Assets/ArtRes/TDSTK";

    [MenuItem("Tools/Remove Missing Scripts From ArtRes")]
    public static void ShowWindow()
    {
        GetWindow<RemoveMissingScripts>("Remove Missing Scripts");
    }

    private void OnGUI()
    {
        GUILayout.Label("移除预制体上的 Missing Script", EditorStyles.boldLabel);
        GUILayout.Space(10);

        GUILayout.Label("目标文件夹:");
        folderPath = EditorGUILayout.TextField(folderPath);

        GUILayout.Space(10);

        if (GUILayout.Button("查找并移除所有 Missing Script", GUILayout.Height(30)))
        {
            RemoveAllMissingScripts(folderPath);
        }

        GUILayout.Space(10);
        GUILayout.Label("说明: 该操作会扫描目标文件夹下所有 .prefab 文件，\n自动移除上面的 Missing Script 组件并保存。", EditorStyles.wordWrappedLabel);
    }

    /// <summary>
    /// 批量移除指定文件夹下所有 prefab 的 Missing Script
    /// </summary>
    public static void RemoveAllMissingScripts(string folderPath)
    {
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogError($"文件夹不存在: {folderPath}");
            return;
        }

        // 递归查找所有 .prefab 文件
        string[] allPrefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
        int totalCount = allPrefabGuids.Length;
        int fixedCount = 0;
        int cleanedCount = 0;

        List<string> cleanedPrefabs = new List<string>();

        for (int i = 0; i < totalCount; i++)
        {
            string prefabPath = AssetDatabase.GUIDToAssetPath(allPrefabGuids[i]);

            EditorUtility.DisplayProgressBar(
                "正在检查预制体...",
                $"({i + 1}/{totalCount}) {prefabPath}",
                (float)i / totalCount
            );

            // 加载预制体
            GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);
            int removedCount = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(prefabRoot);

            if (removedCount > 0)
            {
                // 保存修改
                PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
                Debug.Log($"[Fixed] {prefabPath} — 移除了 {removedCount} 个 Missing Script");
                cleanedPrefabs.Add($"{prefabPath} ({removedCount} 个脚本)");
                fixedCount++;
                cleanedCount += removedCount;
            }

            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }

        EditorUtility.ClearProgressBar();

        // 刷新资源数据库
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // 输出结果
        Debug.Log("=========================================");
        Debug.Log($"扫描完成! 共检查 {totalCount} 个预制体，修复 {fixedCount} 个，移除 {cleanedCount} 个 Missing Script。");
        if (cleanedPrefabs.Count > 0)
        {
            Debug.Log("以下预制体已被修复:");
            foreach (string path in cleanedPrefabs)
            {
                Debug.Log($"  - {path}");
            }
        }
        else
        {
            Debug.Log("没有发现 Missing Script！");
        }
        Debug.Log("=========================================");

        EditorUtility.DisplayDialog(
            "完成",
            $"扫描完成!\n共检查 {totalCount} 个预制体\n修复 {fixedCount} 个预制体\n移除 {cleanedCount} 个 Missing Script\n\n详情请查看 Console 窗口",
            "确定"
        );
    }
}
