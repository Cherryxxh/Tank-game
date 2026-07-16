using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 排行榜面板
/// 显示玩家排名、名称、分数和用时四个维度的数据
/// </summary>
public class Rankpanel : BasePanel<Rankpanel>
{

    public CustomGuiButton btnback;

    /// <summary>排名列 Label 列表（1-10 名）</summary>


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
        for(int i = 1; i <= 9; i++)
        {

        nameLabels.Add(this.transform.Find($"Name/name{i}").GetComponent<CustomGuiLab>());
        timeLabels.Add(this.transform.Find($"ThroughTime/throughtime{i}").GetComponent<CustomGuiLab>());
        scoreLabels.Add(this.transform.Find($"Garden/garden{i}").GetComponent<CustomGuiLab>());
        }

        btnback.ClickEvent += () =>
        {
            HideMe();
            BeginPanel.Instance.ShowMe();
        };
        

        HideMe();
    }
    public override void ShowMe()
    {
        base.ShowMe();
        rankupdate();
    }
    public void rankupdate()
    {
        List<Rankinfo> rankList = GameDatamgr.Instance.rankdata.rankklist;

        for(int i = 0; i < rankList.Count; i++)
        {
            nameLabels[i].guicontent.text = rankList[i].name;
            scoreLabels[i].guicontent.text = rankList[i].score.ToString();
            int time = (int)rankList[i].time;
            timeLabels[i].guicontent.text = "";
            if(time/3600 > 0)
            {
                timeLabels[i].guicontent.text += time/3600 + "时";
            }
            if(time%3600/60 > 0||timeLabels[i].guicontent.text != "")
            {
                timeLabels[i].guicontent.text += time%3600/60 + "分";
            }
            
                timeLabels[i].guicontent.text += time%60 + "秒";
            
        }
    }


}
