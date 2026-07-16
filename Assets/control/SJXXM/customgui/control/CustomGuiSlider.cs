using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 滑块方向枚举
/// </summary>
public enum E_SliderType
{
    Horizontal,  // 水平滑块
    Vertical     // 垂直滑块
}

/// <summary>
/// 自定义滑块控件
/// 值变化时触发 ValueChange 事件，传出当前滑块值
/// </summary>
public class CustomGuiSlider : CustomGuicontrol
{
    /// <summary>最小值</summary>
    public float minvalue = 0;

    /// <summary>最大值</summary>
    public float maxvalue = 1;

    /// <summary>当前值</summary>
    public float nowvalue = 0;

    /// <summary>滑块拇指的样式（仅 styleon 模式下生效）</summary>
    public GUIStyle stylethumb;

    /// <summary>滑块方向（水平/垂直）</summary>
    public E_SliderType sliderType = E_SliderType.Horizontal;

    /// <summary>值变化事件，传出当前滑块值</summary>
    public event UnityAction<float> ValueChange;

    /// <summary>上一帧的值，用于检测变化</summary>
    private float oldvalue = 0;

    /// <summary>不带样式绘制：绘制滑块并检测值变化</summary>
    protected override void Drawstyleoff()
    {
        switch (sliderType)
        {
            case E_SliderType.Horizontal:
                nowvalue = GUI.HorizontalSlider(guipos.Pos, nowvalue, minvalue, maxvalue);
                break;
            case E_SliderType.Vertical:
                nowvalue = GUI.VerticalSlider(guipos.Pos, nowvalue, minvalue, maxvalue);
                break;
        }
        if(oldvalue != nowvalue)
        {
            ValueChange?.Invoke(nowvalue);
            oldvalue = nowvalue;
        }
    }

    /// <summary>带样式绘制：使用 guistyle 和 stylethumb 绘制滑块</summary>
    protected override void Drawstyleon()
    {
        switch (sliderType)
        {
            case E_SliderType.Horizontal:
                nowvalue = GUI.HorizontalSlider(guipos.Pos, nowvalue, minvalue,
                 maxvalue, guistyle, stylethumb);
                break;
            case E_SliderType.Vertical:
                nowvalue = GUI.VerticalSlider(guipos.Pos, nowvalue, minvalue,
                 maxvalue, guistyle, stylethumb);
                break;
        }
        if(oldvalue != nowvalue)
        {
            ValueChange?.Invoke(nowvalue);
            oldvalue = nowvalue;
        }
    }
}
