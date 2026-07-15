using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGuiTexture : CustomGuicontrol
{
    public ScaleMode scalemode = ScaleMode.StretchToFill;
    protected override void Drawstyleoff()
    {
        GUI.DrawTexture(guipos.Pos,guicontent.image,scalemode);
    }

    protected override void Drawstyleon()
    {
        GUI.DrawTexture(guipos.Pos,guicontent.image,scalemode);
    }
}
