using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 控件样式开关（运行时决定是否使用 GUIStyle）
/// </summary>
public enum E_style
{
    on,   // 使用 guistyle 样式
    off,  // 不使用样式（默认）
}

/// <summary>
/// 自定义 GUI 控件抽象基类
/// 所有 UI 控件（Label、Button、Toggle 等）都继承此类
/// </summary>
public abstract class CustomGuicontrol : MonoBehaviour
{
    /// <summary>控件位置和大小配置</summary>
    public CustomGuiPos guipos;

    /// <summary>控件显示内容（文本 + 图片）</summary>
    public GUIContent guicontent;

    /// <summary>控件 GUIStyle 样式（仅在 guistyletype 为 on 时生效）</summary>
    public GUIStyle guistyle;

    /// <summary>样式开关状态</summary>
    public E_style guistyletype = E_style.off;

    /// <summary>
    /// 绘制控件（由 CustomGuiRoot 每帧在 OnGUI 中调用）
    /// 根据 guistyletype 决定使用带样式还是不带样式的绘制
    /// </summary>
    public void DrawGUI()
    {
        switch(guistyletype)
        {
            case E_style.on:
            Drawstyleon();
            break;
            case E_style.off:
            Drawstyleoff();
            break;
        }
    }

    /// <summary>带样式绘制（子类必须实现）</summary>
    protected abstract void Drawstyleon();

    /// <summary>不带样式绘制（子类必须实现）</summary>
    protected abstract void Drawstyleoff();
}
