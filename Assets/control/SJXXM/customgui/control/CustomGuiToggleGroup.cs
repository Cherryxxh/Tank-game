using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGuiToggleGroup : MonoBehaviour
{
    public CustomGuiToggle[] toggles;

    private CustomGuiToggle frontTurTog;
    // Start is called before the first frame update
    void Start()
    {
        if(toggles.Length == 0)
        return;
        
        for(int i = 0; i < toggles.Length; i++)
        {
            CustomGuiToggle toggle = toggles[i];
            toggle.changevalue += (value) =>
            {
                if(value)
                {
                    for(int j = 0; j < toggles.Length; j++)
                    {
                        if(toggles[j] != toggle)
                        {
                            toggles[j].issel = false;
                        }
                    }
                    frontTurTog = toggle;
                }
                else if (frontTurTog == toggle)
                {
                    toggle.issel = true;
                }
            };
        }
    }

    
}
