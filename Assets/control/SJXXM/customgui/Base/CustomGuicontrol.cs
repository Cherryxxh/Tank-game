using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum E_style
{
    on,
    off,
}

public abstract class CustomGuicontrol : MonoBehaviour
{
    public CustomGuiPos guipos;
    public GUIContent guicontent;
    public GUIStyle guistyle;
    public E_style guistyletype = E_style.off;

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


    protected abstract void Drawstyleon();
    
    protected abstract void Drawstyleoff();
    
}
