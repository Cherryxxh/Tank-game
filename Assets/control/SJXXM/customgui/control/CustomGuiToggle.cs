using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomGuiToggle : CustomGuicontrol
{

    public bool issel;
    public event UnityAction<bool> changevalue; 

    public bool isoldsel;
    protected override void Drawstyleoff()
    {
        GUI.Toggle(guipos.Pos,issel,guicontent);
        if(isoldsel!=issel)
        {
            changevalue?.Invoke(issel);
            isoldsel = issel;
        }
        
    }

    protected override void Drawstyleon()
    {
        GUI.Toggle(guipos.Pos,issel,guicontent,guistyle);
        if(isoldsel!=issel)
        {
        changevalue?.Invoke(issel);
        isoldsel = issel;
         }
    }
}
