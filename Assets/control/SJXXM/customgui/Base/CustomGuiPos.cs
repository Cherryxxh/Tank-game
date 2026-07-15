using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CustomGuiPosType
{
    LeftTop,
    LeftCenter,
    LeftBottom,
    CenterTop,
    CenterCenter,
    CenterBottom,
    RightTop,
    RightCenter,
    RightBottom
}



[System.Serializable]
public class CustomGuiPos 
{
    private Rect rpos = new Rect(0, 0, 100, 100);

    public CustomGuiPosType screenCenterPosType =CustomGuiPosType.CenterCenter;

    public CustomGuiPosType ControlCenterPosType = CustomGuiPosType.CenterCenter;

    public Vector2 pos;
    
      
    public float width = 100;

    public float height = 50;

    private Vector2 centerpos;

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

    private void Clacpos()
    {
        switch(screenCenterPosType)
        {
            case CustomGuiPosType.LeftTop:
            rpos.x = 0+ pos.x+centerpos.x;
            rpos.y = 0+ pos.y+centerpos.y;
            break;
            case CustomGuiPosType.LeftCenter:
            rpos.x = 0+ pos.x+centerpos.x;
            rpos.y = Screen.height / 2 + pos.y+centerpos.y;
            break;
            case CustomGuiPosType.LeftBottom:
            rpos.x = 0+ pos.x+centerpos.x;
            rpos.y = Screen.height - pos.y+centerpos.y;
            break;
            case CustomGuiPosType.CenterTop:
            rpos.x = Screen.width / 2 + centerpos.x+pos.x;
            rpos.y = 0 + centerpos.y+pos.y;
            break;
            case CustomGuiPosType.CenterCenter:
            rpos.x = Screen.width / 2 + centerpos.x+pos.x;
            rpos.y = Screen.height / 2 + centerpos.y+pos.y;
            break;
            case CustomGuiPosType.CenterBottom:
            rpos.x = Screen.width / 2 + centerpos.x+pos.x;
            rpos.y = Screen.height - pos.y+centerpos.y;
            break;
            case CustomGuiPosType.RightTop:
            rpos.x = Screen.width - pos.x+centerpos.x;
            rpos.y = 0 + centerpos.y+pos.y;
            break;
            case CustomGuiPosType.RightCenter:
            rpos.x = Screen.width - pos.x+centerpos.x;
            rpos.y = Screen.height / 2 + centerpos.y+pos.y;
            break;
            case CustomGuiPosType.RightBottom:
            rpos.x = Screen.width - pos.x+centerpos.x;
            rpos.y = Screen.height - pos.y+centerpos.y;
            break;
        }
    }
    
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
