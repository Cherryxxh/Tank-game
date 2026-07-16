using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 自定义输入框控件
/// 文本变化时触发 textchange 事件，传出当前文本内容
/// </summary>
public class CustomGuiinput : CustomGuicontrol
{
    /// <summary>文本变化事件，传出新的文本内容</summary>
    public event UnityAction<string> textchange;

    /// <summary>上一帧的文本内容，用于检测变化</summary>
    public string oldtext = "";

    /// <summary>不带样式绘制：绘制输入框并检测文本变化</summary>
    protected override void Drawstyleoff()
    {
        guicontent.text = GUI.TextField(guipos.Pos, guicontent.text);
        if(oldtext != guicontent.text)
        {
            textchange?.Invoke(guicontent.text);
            oldtext = guicontent.text;
        }
    }

    /// <summary>带样式绘制：使用 guistyle 绘制输入框并检测文本变化</summary>
    protected override void Drawstyleon()
    {
        guicontent.text = GUI.TextField(guipos.Pos, guicontent.text, guistyle);
        if(oldtext != guicontent.text)
        {
            textchange?.Invoke(guicontent.text);
            oldtext = guicontent.text;
        }
    }
}
