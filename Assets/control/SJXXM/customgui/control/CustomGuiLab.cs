using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自定义标签控件（Label）
/// 用于在屏幕上显示文字和图片内容
/// </summary>
public class CustomGuiLab : CustomGuicontrol
{
    /// <summary>不带样式绘制：只显示 guicontent 的内容</summary>
    protected override void Drawstyleoff()
    {
        GUI.Label(guipos.Pos, guicontent);
    }

    /// <summary>带样式绘制：使用 guistyle 渲染标签</summary>
    protected override void Drawstyleon()
    {
        GUI.Label(guipos.Pos, guicontent, guistyle);
    }
}
