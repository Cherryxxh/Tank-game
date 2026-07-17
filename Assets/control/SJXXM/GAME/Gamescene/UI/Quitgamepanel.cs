using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quitgamepanel : BasePanel<Quitgamepanel>
{

    public CustomGuiButton quitbtn;

    public CustomGuiButton backbtn;

    public CustomGuiButton conbtn;

    // Start is called before the first frame update
    void Start()
    {
        quitbtn.ClickEvent += () =>
        {
            Application.Quit();
        };
        backbtn.ClickEvent += () =>
        {
            HideMe();
            Gamepanel.Instance.ShowMe();
        };
        conbtn.ClickEvent += () =>
        {
            HideMe();
            Gamepanel.Instance.ShowMe();
        };

        HideMe();
    }

    public override void HideMe()
    {
        base.HideMe();
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
