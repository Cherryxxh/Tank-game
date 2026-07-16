using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 开始面板（主菜单界面）
/// 提供开始游戏、打开设置、退出游戏、排行榜四个入口
/// </summary>
public class BeginPanel : BasePanel<BeginPanel>
{
    /// <summary>开始游戏按钮</summary>
    public CustomGuiButton btnbegin;

    /// <summary>打开设置面板按钮</summary>
    public CustomGuiButton btnsetting;

    /// <summary>退出游戏按钮</summary>
    public CustomGuiButton btnquit;

    /// <summary>排行榜按钮</summary>
    public CustomGuiButton btnrank;

    /// <summary>
    /// 启动时为各按钮绑定点击事件回调
    /// </summary>
    void Start()
    {
        // 开始游戏 → 加载游戏场景
        btnbegin.ClickEvent += () =>
        {
            SceneManager.LoadScene("Gamescene");
        };

        // 设置 → 显示设置面板，隐藏开始面板
        btnsetting.ClickEvent += () =>
        {
            settingpanel.Instance.ShowMe();
            HideMe();
        };

        // 退出 → 关闭应用
        btnquit.ClickEvent += () =>
        {
            Application.Quit();
        };

        // 排行榜（暂未实现）
        btnrank.ClickEvent += () =>
        {

        };
    }

    void Update()
    {

    }
}
