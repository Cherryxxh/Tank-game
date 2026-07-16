using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 一键整理 RankPanel：创建分类文件夹，将条目按类别归类，去掉括号并重命名为 "前缀+数字" 格式
/// 例: "rank (1)" → 移动到 Rank/ 下 → 重命名为 "rank1"
/// 使用方法: 菜单栏 Tools → Reorganize RankPanel
/// </summary>
public class ReorganizeRankPanel : EditorWindow
{
    /// <summary>四个分类文件夹名称</summary>
    private static readonly string[] Categories = { "Rank", "Name", "ThroughTime", "Garden" };

    /// <summary>匹配关键词（用于判断条目的归属分类）</summary>
    private static readonly string[] Keywords   = { "rank", "name", "through time", "graden" };

    /// <summary>重命名前缀（对应四个分类）</summary>
    private static readonly string[] Prefixes   = { "rank", "name", "throughtime", "garden" };

    /// <summary>执行整理操作（菜单入口）</summary>
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

        // 2. 收集所有当前子物体
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
                num = oldName;

            // 4. 匹配分类并移动重命名
            for (int i = 0; i < Keywords.Length; i++)
            {
                if (oldName.ToLower().Contains(Keywords[i]))
                {
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

    /// <summary>
    /// 从名称字符串中提取括号内的数字
    /// </summary>
    /// <param name="name">如 "rank (3)"</param>
    /// <returns>如 "3"，如果没有括号则返回 null</returns>
    private static string ExtractNumber(string name)
    {
        int s = name.IndexOf('(');
        int e = name.IndexOf(')');
        if (s >= 0 && e > s)
            return name.Substring(s + 1, e - s - 1).Trim();
        return null;
    }
}
