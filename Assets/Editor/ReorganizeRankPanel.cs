using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 一键整理 RankPanel：去掉括号，加分类前缀，分到四个父物体中
/// 使用方法：菜单栏 Tools → Reorganize RankPanel
/// </summary>
public class ReorganizeRankPanel : EditorWindow
{
    private static readonly string[] Categories = { "Rank", "Name", "ThroughTime", "Garden" };
    private static readonly string[] Keywords   = { "rank", "name", "through time", "graden" };
    private static readonly string[] Prefixes   = { "rank", "name", "throughtime", "garden" };

    [MenuItem("Tools/Reorganize RankPanel")]
    public static void Execute()
    {
        GameObject rankpanel = GameObject.Find("rankpanel");
        if (rankpanel == null)
        {
            Debug.LogError("没找到 rankpanel！");
            return;
        }

        Transform root = rankpanel.transform;

        // 1. 创建四个父物体
        Transform[] parents = new Transform[Categories.Length];
        for (int i = 0; i < Categories.Length; i++)
        {
            GameObject go = new GameObject(Categories[i]);
            go.transform.SetParent(root);
            parents[i] = go.transform;
        }

        // 2. 收集所有子物体
        int childCount = root.childCount;
        List<Transform> all = new List<Transform>();
        for (int i = 0; i < childCount; i++)
            all.Add(root.GetChild(i));

        int count = 0;

        foreach (Transform child in all)
        {
            string oldName = child.name;

            // 跳过四个分类父物体
            if (oldName == "Rank" || oldName == "Name" ||
                oldName == "ThroughTime" || oldName == "Garden")
                continue;

            // 3. 提取括号里的数字（如 "rank (3)" → "3"）
            string num = ExtractNumber(oldName);
            if (string.IsNullOrEmpty(num))
                num = oldName; // 没有括号就用原名

            // 4. 匹配分类
            for (int i = 0; i < Keywords.Length; i++)
            {
                if (oldName.ToLower().Contains(Keywords[i]))
                {
                    // 5. 命名：前缀 + 数字 → "rank3"
                    string newName = Prefixes[i] + num;

                    child.SetParent(parents[i]);
                    child.name = newName;
                    count++;
                    Debug.Log($"\"{oldName}\" → \"{newName}\" → {Categories[i]}");
                    break;
                }
            }
        }

        Debug.Log($"完成! 处理 {count} 个对象");
        EditorUtility.DisplayDialog("完成", $"整理完成!\n\n处理 {count} 个对象\n重命名并分类到:\n  - Rank\n  - Name\n  - ThroughTime\n  - Garden", "确定");
    }

    private static string ExtractNumber(string name)
    {
        int s = name.IndexOf('(');
        int e = name.IndexOf(')');
        if (s >= 0 && e > s)
            return name.Substring(s + 1, e - s - 1).Trim();
        return null;
    }
}
