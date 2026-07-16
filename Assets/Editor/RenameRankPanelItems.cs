using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 把分类文件夹下的条目重命名为 "前缀+数字" 格式
/// 例: "rank (1)" → "rank1", "name (2)" → "name2"
/// 使用方法: Tools → Rename RankPanel Items
/// </summary>
public class RenameRankPanelItems : EditorWindow
{
    private static readonly string[] FolderNames = { "Rank", "Name", "ThroughTime", "Garden" };
    private static readonly string[] Prefixes    = { "rank", "name", "throughtime", "garden" };

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

            for (int j = 0; j < childCount; j++)
            {
                Transform child = t.GetChild(j);
                string oldName = child.name;

                // 从名字里提取数字: "rank (1)" → "1", "1" → "1"
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

    private static string ExtractNumber(string name)
    {
        int s = name.IndexOf('(');
        int e = name.IndexOf(')');
        if (s >= 0 && e > s)
            return name.Substring(s + 1, e - s - 1).Trim();
        return null;
    }
}
