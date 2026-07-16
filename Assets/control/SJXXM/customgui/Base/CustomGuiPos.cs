using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 控件锚点类型，定义控件在屏幕上的 9 个参考位置
/// </summary>
public enum CustomGuiPosType
{
    LeftTop,        // 左上
    LeftCenter,     // 左中
    LeftBottom,     // 左下
    CenterTop,      // 中上
    CenterCenter,   // 中心（默认）
    CenterBottom,   // 中下
    RightTop,       // 右上
    RightCenter,    // 右中
    RightBottom     // 右下
}


/// <summary>
/// 自定义 GUI 坐标系统
/// 根据屏幕锚点和控件自身锚点，计算出 OnGUI 使用的 Rect
/// </summary>
[System.Serializable]
public class CustomGuiPos
{
    private Rect rpos = new Rect(0, 0, 100, 100);

    /// <summary>屏幕上的锚点位置</summary>
    public CustomGuiPosType screenCenterPosType = CustomGuiPosType.CenterCenter;

    /// <summary>控件自身的锚点位置</summary>
    public CustomGuiPosType ControlCenterPosType = CustomGuiPosType.CenterCenter;

    /// <summary>相对于锚点的偏移量</summary>
    public Vector2 pos;

    /// <summary>控件宽度</summary>
    public float width = 100;

    /// <summary>控件高度</summary>
    public float height = 50;

    /// <summary>控件自身锚点计算出的偏移量（缓存）</summary>
    private Vector2 centerpos;

    /// <summary>
    /// 根据 ControlCenterPosType 计算控件自身锚点的偏移量
    /// </summary>
    private void ClacCenterpos()
    {
        switch(ControlCenterPosType)
        {
            case CustomGuiPosType.LeftTop:
            centerpos.x = 0;
            centerpos.y = 0;
            break;
            case CustomGuiPosType.LeftCenter:
            centerpos.x = 0;
            centerpos.y = -height / 2;
            break;
            case CustomGuiPosType.LeftBottom:
            centerpos.x = 0;
            centerpos.y = -height;
            break;
            case CustomGuiPosType.CenterTop:
            centerpos.x = -width / 2;
            centerpos.y = 0;
            break;
            case CustomGuiPosType.CenterCenter:
            centerpos.x = -width / 2;
            centerpos.y = -height / 2;
            break;
            case CustomGuiPosType.CenterBottom:
            centerpos.x = -width / 2;
            centerpos.y = -height;
            break;
            case CustomGuiPosType.RightTop:
            centerpos.x = -width;
            centerpos.y = 0;
            break;
            case CustomGuiPosType.RightCenter:
            centerpos.x = -width;
            centerpos.y = -height / 2;
            break;
            case CustomGuiPosType.RightBottom:
            centerpos.x = -width;
            centerpos.y = -height;
            break;
        }
    }

    /// <summary>
    /// 根据 screenCenterPosType 和控件偏移量，计算最终的屏幕坐标
    /// </summary>
    private void Clacpos()
    {
        switch(screenCenterPosType)
        {
            case CustomGuiPosType.LeftTop:
            rpos.x = 0 + pos.x + centerpos.x;
            rpos.y = 0 + pos.y + centerpos.y;
            break;
            case CustomGuiPosType.LeftCenter:
            rpos.x = 0 + pos.x + centerpos.x;
            rpos.y = Screen.height / 2 + pos.y + centerpos.y;
            break;
            case CustomGuiPosType.LeftBottom:
            rpos.x = 0 + pos.x + centerpos.x;
            rpos.y = Screen.height - pos.y + centerpos.y;
            break;
            case CustomGuiPosType.CenterTop:
            rpos.x = Screen.width / 2 + centerpos.x + pos.x;
            rpos.y = 0 + centerpos.y + pos.y;
            break;
            case CustomGuiPosType.CenterCenter:
            rpos.x = Screen.width / 2 + centerpos.x + pos.x;
            rpos.y = Screen.height / 2 + centerpos.y + pos.y;
            break;
            case CustomGuiPosType.CenterBottom:
            rpos.x = Screen.width / 2 + centerpos.x + pos.x;
            rpos.y = Screen.height - pos.y + centerpos.y;
            break;
            case CustomGuiPosType.RightTop:
            rpos.x = Screen.width - pos.x + centerpos.x;
            rpos.y = 0 + centerpos.y + pos.y;
            break;
            case CustomGuiPosType.RightCenter:
            rpos.x = Screen.width - pos.x + centerpos.x;
            rpos.y = Screen.height / 2 + centerpos.y + pos.y;
            break;
            case CustomGuiPosType.RightBottom:
            rpos.x = Screen.width - pos.x + centerpos.x;
            rpos.y = Screen.height - pos.y + centerpos.y;
            break;
        }
    }

    /// <summary>
    /// 获取最终计算好的 Rect（每次访问都重新计算，保证屏幕尺寸变化时正确）
    /// </summary>
    public Rect Pos
    {
        get
        {
            ClacCenterpos();
            Clacpos();
            rpos.width = width;
            rpos.height = height;
            return rpos;
        }
    }
}
