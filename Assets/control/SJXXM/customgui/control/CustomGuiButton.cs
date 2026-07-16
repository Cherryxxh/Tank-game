using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 自定义按钮控件
/// 点击时触发 ClickEvent 事件
/// </summary>
public class CustomGuiButton : CustomGuicontrol
{
    /// <summary>点击事件，外部订阅以响应按钮点击</summary>
    public event UnityAction ClickEvent;

    /// <summary>不带样式绘制：检测按钮点击并触发事件</summary>
    protected override void Drawstyleoff()
    {
        if(GUI.Button(guipos.Pos, guicontent))
        {
            ClickEvent?.Invoke();
        }
    }

    /// <summary>带样式绘制：使用 guistyle 渲染按钮并检测点击</summary>
    protected override void Drawstyleon()
    {
        if(GUI.Button(guipos.Pos, guicontent, guistyle))
        {
            ClickEvent?.Invoke();
        }
    }
}
