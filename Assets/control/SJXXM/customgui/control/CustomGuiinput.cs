using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomGuiinput : CustomGuicontrol
{
    public event UnityAction<string> textchange;
    public string oldtext = "";


    protected override void Drawstyleoff()
    {
        guicontent.text = GUI.TextField(guipos.Pos,guicontent.text);
        if(oldtext !=guicontent.text )
        {
            textchange?.Invoke(guicontent.text);
            oldtext = guicontent.text;
        }
    }

    protected override void Drawstyleon()
    {
        guicontent.text = GUI.TextField(guipos.Pos,guicontent.text,guistyle);
        if(oldtext !=guicontent.text )
        {
            textchange?.Invoke(guicontent.text);
            oldtext = guicontent.text;
        }
    }
}
