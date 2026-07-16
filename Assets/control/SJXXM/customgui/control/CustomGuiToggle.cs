using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 自定义开关/复选框控件
/// 状态变化时触发 changevalue 事件，传出当前选中状态
/// </summary>
public class CustomGuiToggle : CustomGuicontrol
{
    /// <summary>当前选中状态</summary>
    public bool issel;

    /// <summary>值变化事件，传出新的选中状态（true=选中，false=取消）</summary>
    public event UnityAction<bool> changevalue;

    /// <summary>上一帧的选中状态，用于检测是否发生变化</summary>
    private bool isoldsel;

    /// <summary>不带样式绘制：绘制 Toggle 并检测状态变化</summary>
    protected override void Drawstyleoff()
    {
        // 接住 GUI.Toggle 的返回值，issel 才会随点击变化
        issel = GUI.Toggle(guipos.Pos, issel, guicontent);
        if(isoldsel != issel)
        {
            changevalue?.Invoke(issel);
            isoldsel = issel;
        }
    }

    /// <summary>带样式绘制：使用 guistyle 绘制 Toggle 并检测状态变化</summary>
    protected override void Drawstyleon()
    {
        issel = GUI.Toggle(guipos.Pos, issel, guicontent, guistyle);
        if(isoldsel != issel)
        {
            changevalue?.Invoke(issel);
            isoldsel = issel;
        }
    }
}
