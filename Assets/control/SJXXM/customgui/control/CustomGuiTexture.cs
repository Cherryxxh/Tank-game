using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自定义纹理控件
/// 直接在屏幕上绘制一张纹理图片
/// </summary>
public class CustomGuiTexture : CustomGuicontrol
{
    /// <summary>图片缩放模式，默认拉伸填充</summary>
    public ScaleMode scalemode = ScaleMode.StretchToFill;

    /// <summary>绘制纹理（两种模式绘制逻辑相同，都直接画图）</summary>
    protected override void Drawstyleoff()
    {
        GUI.DrawTexture(guipos.Pos, guicontent.image, scalemode);
    }

    /// <summary>带样式绘制纹理</summary>
    protected override void Drawstyleon()
    {
        GUI.DrawTexture(guipos.Pos, guicontent.image, scalemode);
    }
}
