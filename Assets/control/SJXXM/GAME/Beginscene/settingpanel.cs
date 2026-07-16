using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 设置面板
/// 提供音乐/音效开关和音量调节，数据通过 GameDatamgr 持久化存储
/// </summary>
public class settingpanel : BasePanel<settingpanel>
{
    /// <summary>返回按钮</summary>
    public CustomGuiButton btnback;

    /// <summary>背景音乐开关</summary>
    public CustomGuiToggle tog_music;

    /// <summary>音效开关</summary>
    public CustomGuiToggle tog_sound;

    /// <summary>背景音乐音量滑块</summary>
    public CustomGuiSlider slider_music;

    /// <summary>音效音量滑块</summary>
    public CustomGuiSlider slider_sound;

    /// <summary>
    /// 启动时绑定控件事件，并从持久化数据同步到 UI
    /// </summary>
    void Start()
    {
        // 返回按钮 → 隐藏设置面板，回到开始面板
        btnback.ClickEvent += () =>
        {
            HideMe();
            BeginPanel.Instance.ShowMe();
        };

        // 音乐音量变化 → 保存到 GameDatamgr
        slider_music.ValueChange += (value) =>
        {
            GameDatamgr.Instance.musicvolue(value);
        };

        // 音效音量变化 → 保存到 GameDatamgr
        slider_sound.ValueChange += (value) =>
        {
            GameDatamgr.Instance.soundvolue(value);
        };

        // 音乐开关变化 → 保存到 GameDatamgr
        tog_music.changevalue += (value) =>
        {
            GameDatamgr.Instance.isopenbkmusic(value);
        };

        // 音效开关变化 → 保存到 GameDatamgr
        tog_sound.changevalue += (value) =>
        {
            GameDatamgr.Instance.isopensound(value);
        };

        // 启动时隐藏面板
        HideMe();
    }

    /// <summary>从 GameDatamgr 读取保存的数据，同步到所有 UI 控件</summary>
    public void Updatapanelinfo()
    {
        MusicData rmusicData = GameDatamgr.Instance.musicData;
        tog_music.issel = rmusicData.isopenbkmusic;
        tog_sound.issel = rmusicData.isopensound;
        slider_music.nowvalue = rmusicData.musicvolue;
        slider_sound.nowvalue = rmusicData.soundvolue;
    }

    /// <summary>显示设置面板时，自动同步最新数据到 UI</summary>
    public override void ShowMe()
    {
        base.ShowMe();
        Updatapanelinfo();
    }
}
