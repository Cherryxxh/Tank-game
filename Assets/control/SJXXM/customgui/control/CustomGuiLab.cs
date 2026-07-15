using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGuiLab : CustomGuicontrol
{
    protected override void Drawstyleoff()
    {
        GUI.Label(guipos.Pos, guicontent);
    }

    protected override void Drawstyleon()
    {
        GUI.Label(guipos.Pos, guicontent, guistyle);
    }
}
