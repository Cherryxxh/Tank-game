 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomGuiButton : CustomGuicontrol
{
    public event UnityAction ClickEvent;

    protected override void Drawstyleoff()
    {
        if(GUI.Button(guipos.Pos,guicontent))
        {
            ClickEvent?.Invoke();
        }
        
    }

    protected override void Drawstyleon()
    {
        if(GUI.Button(guipos.Pos,guicontent, guistyle))
        {
            ClickEvent?.Invoke();
        }
        
    }
}
