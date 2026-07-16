using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 排行榜面板
/// 显示玩家排名、名称、分数和用时四个维度的数据
/// </summary>
public class Rankpanel : BasePanel<Rankpanel>
{
    /// <summary>排名列 Label 列表（1-10 名）</summary>
    private List<CustomGuiLab> rankLabels = new List<CustomGuiLab>();

    /// <summary>名称列 Label 列表</summary>
    private List<CustomGuiLab> nameLabels = new List<CustomGuiLab>();

    /// <summary>分数列 Label 列表</summary>
    private List<CustomGuiLab> scoreLabels = new List<CustomGuiLab>();

    /// <summary>用时列 Label 列表</summary>
    private List<CustomGuiLab> timeLabels = new List<CustomGuiLab>();

    /// <summary>
    /// 启动时初始化排行榜数据
    /// </summary>
    void Start()
    {
        for(int i = 1; i <= 10; i++)
        {
            // TODO: 从数据源加载排行榜数据并填充 Label
        }
    }
}
