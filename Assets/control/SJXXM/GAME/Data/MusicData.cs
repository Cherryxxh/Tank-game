using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音乐/音效设置的纯数据类
/// 通过 PlayerprefsdataMgr 持久化存储到 PlayerPrefs
/// </summary>
public class MusicData
{
    /// <summary>背景音乐是否开启</summary>
    public bool isopenbkmusic;

    /// <summary>音效是否开启</summary>
    public bool isopensound;

    /// <summary>背景音乐音量（0~1）</summary>
    public float musicvolue;

    /// <summary>音效音量（0~1）</summary>
    public float soundvolue;

    /// <summary>是否首次打开游戏（用于初始化默认值）</summary>
    public bool notfirstopen;
}
