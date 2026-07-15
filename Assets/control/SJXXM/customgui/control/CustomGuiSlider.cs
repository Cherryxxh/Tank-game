using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum E_SliderType
{
    Horizontal,
    Vertical
}

public class CustomGuiSlider : CustomGuicontrol
{
    public float minvalue = 0;
    public float maxvalue = 1;
    public float nowvalue = 0;

    public GUIStyle stylethumb;

    public E_SliderType sliderType = E_SliderType.Horizontal;

    public event UnityAction<float> ValueChange;

    private float oldvalue = 0;

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
        if( oldvalue!= nowvalue)
        {
            ValueChange?.Invoke(nowvalue);
            oldvalue = nowvalue;
        }
        
    }

    protected override void Drawstyleon()
    {
        switch (sliderType)
        {
            case E_SliderType.Horizontal:
                nowvalue = GUI.HorizontalSlider(guipos.Pos, nowvalue, minvalue,
                 maxvalue, guistyle,stylethumb);
                break;
            case E_SliderType.Vertical:
                nowvalue = GUI.VerticalSlider(guipos.Pos, nowvalue, minvalue,
                 maxvalue, guistyle,stylethumb);
                break;
        }
        if( oldvalue!= nowvalue)
        {
            ValueChange?.Invoke(nowvalue);
            oldvalue = nowvalue;
        }
    }
}
