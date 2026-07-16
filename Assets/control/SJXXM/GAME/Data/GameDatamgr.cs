using System.Collections;
using System.Collections.Generic;
using UnityEngine;/// <summary>
/// 游戏数据管理器（单例）
/// 管理音乐/音效等全局设置数据，提供读写接口并自动持久化到 PlayerPrefs
/// </summary>
public class GameDatamgr
{
    /// <summary>单例实例（私有构造函数 + 立即初始化）</summary>
    private static GameDatamgr instance = new GameDatamgr();

    /// <summary>获取单例入口</summary>
    public static GameDatamgr Instance { get => instance; }

    /// <summary>音乐/音效设置数据</summary>
    public MusicData musicData;

    public RankList rankdata;

    /// <summary>
    /// 私有构造函数，首次访问时自动加载持久化数据
    /// 如果是首次运行，初始化默认值并保存
    /// </summary>
    private GameDatamgr()
    {
        musicData = PlayerprefsdataMgr.Instance.
        LoadData(typeof(MusicData), "musicData") as MusicData;

        if (!musicData.notfirstopen)
        {
            // 首次打开：设置默认值（音乐开、音效开、音量满）
            musicData.notfirstopen = true;
            musicData.isopenbkmusic = true;
            musicData.isopensound = true;
            musicData.soundvolue = 1;
            musicData.musicvolue = 1;
            PlayerprefsdataMgr.Instance.SaveData(musicData, "musicData");
        }


        rankdata = PlayerprefsdataMgr.Instance.
        LoadData(typeof(RankList), "rankList") as RankList;
        if (rankdata == null)
        {
            rankdata = new RankList();
            PlayerprefsdataMgr.Instance.SaveData(rankdata, "rankList");
        }
    }


    public void AddRankinfo(string name, int score, float time)
    {
        rankdata.rankklist.Add(new Rankinfo(name, score, time));
        rankdata.rankklist.Sort((a, b) => a.time<b.time?-1:1);
        for(int i = rankdata.rankklist.Count-1; i >= 9; i--)
        {
            rankdata.rankklist.RemoveAt(i);
        }
        PlayerprefsdataMgr.Instance.SaveData(rankdata, "rankList");
    }

    /// <summary>设置背景音乐开关状态并保存</summary>
    /// <param name="isopen">true=开启，false=关闭</param>
    public void isopenbkmusic(bool isopen)
    {
        musicData.isopenbkmusic = isopen;
        Bkmusic.Instance?.changeopen(isopen);
        PlayerprefsdataMgr.Instance.SaveData(musicData, "musicData");
    }

    /// <summary>设置音效开关状态并保存</summary>
    /// <param name="isopen">true=开启，false=关闭</param>
    public void isopensound(bool isopen)
    {
        musicData.isopensound = isopen;
        
        PlayerprefsdataMgr.Instance.SaveData(musicData, "musicData");
    }

    /// <summary>设置背景音乐音量并保存</summary>
    /// <param name="volue">音量值（0~1）</param>
    public void musicvolue(float volue)
    {
        musicData.musicvolue = volue;
        if (Bkmusic.Instance == null)
            Debug.LogWarning("Bkmusic.Instance 是 null！脚本没挂到场景的 Bkmusic 物体上！");
        else
            Bkmusic.Instance.changevalue(volue);
        PlayerprefsdataMgr.Instance.SaveData(musicData, "musicData");
    }

    /// <summary>设置音效音量并保存</summary>
    /// <param name="volue">音量值（0~1）</param>
    public void soundvolue(float volue)
    {
        musicData.soundvolue = volue;
        PlayerprefsdataMgr.Instance.SaveData(musicData, "musicData");
    }
    
}
