using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[ExecuteAlways]
public class CustomGuiRoot : MonoBehaviour
{
    private CustomGuicontrol[] allcontrols;

    // Start is called before the first frame update
    void Start()
    {
        allcontrols = this.GetComponentsInChildren<CustomGuicontrol>(); 
    }
    private void OnGUI()
    {
        //if(!Application.isPlaying)
        //{
            allcontrols = this.GetComponentsInChildren<CustomGuicontrol>(); 
        //}
        
        for(int i = 0; i < allcontrols.Length; i++)
        {
            allcontrols[i].DrawGUI();
        }

    }

    
}
