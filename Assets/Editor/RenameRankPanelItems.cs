using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 将分类文件夹下的条目重命名为 "前缀+数字" 格式
/// 前提：条目已经移动到对应的分类文件夹下
/// 例: Rank/ 下 "1" → "rank1", Name/ 下 "2" → "name2"
/// 使用方法: Tools → Rename RankPanel Items
/// </summary>
public class RenameRankPanelItems : EditorWindow
{
    /// <summary>四个分类文件夹的名称</summary>
    private static readonly string[] FolderNames = { "Rank", "Name", "ThroughTime", "Garden" };

    /// <summary>对应的重命名前缀</summary>
    private static readonly string[] Prefixes    = { "rank", "name", "throughtime", "garden" };

    /// <summary>执行重命名操作（菜单入口）</summary>
    [MenuItem("Tools/Rename RankPanel Items")]
    public static void Execute()
    {
        int renamed = 0;

        for (int i = 0; i < FolderNames.Length; i++)
        {
            GameObject folder = GameObject.Find(FolderNames[i]);
            if (folder == null)
            {
                Debug.LogWarning($"没找到 {FolderNames[i]}，跳过");
                continue;
            }

            Transform t = folder.transform;
            int childCount = t.childCount;

            // 遍历分类文件夹下的每个子物体
            for (int j = 0; j < childCount; j++)
            {
                Transform child = t.GetChild(j);
                string oldName = child.name;

                // 从名字里提取数字: "rank (1)" → "1", 或纯数字 "1" → "1"
                string num = ExtractNumber(oldName);
                if (num == null) num = oldName;

                string newName = Prefixes[i] + num;

                if (oldName != newName)
                {
                    child.name = newName;
                    renamed++;
                    Debug.Log($"\"{oldName}\" → \"{newName}\"");
                }
            }
        }

        Debug.Log($"完成! 重命名 {renamed} 个");
        EditorUtility.DisplayDialog("完成", $"重命名完成!\n共处理 {renamed} 个对象\n\n格式: rank1, name2, throughtime3, garden4 ...", "确定");
    }

    /// <summary>
    /// 从名称字符串中提取括号内的数字
    /// </summary>
    /// <param name="name">如 "rank (3)"</param>
    /// <returns>如 "3"，没有括号则返回 null</returns>
    private static string ExtractNumber(string name)
    {
        int s = name.IndexOf('(');
        int e = name.IndexOf(')');
        if (s >= 0 && e > s)
            return name.Substring(s + 1, e - s - 1).Trim();
        return null;
    }
}
