using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamepanel : BasePanel<Gamepanel>
{

    public CustomGuiButton quitbtn;
    public CustomGuiButton settingbtn;

    public CustomGuiLab timelab;

    public CustomGuiLab scorelab;

    public CustomGuiTexture blood;

    public float hpw = 350;

    [HideInInspector]
    public int nowscore = 0;

    [HideInInspector]
    public float nowtime = 0;

    private int time = 0;

    void Start()
    {
        quitbtn.ClickEvent += () =>
        {
            Quitgamepanel.Instance.ShowMe();
            HideMe();
        };
        settingbtn.ClickEvent += () =>
        {
            settingpanel.Instance.ShowMe();
            HideMe();
            Time.timeScale = 0;
        };
        
        
    }

    void Update()
    {
        nowtime += Time.deltaTime;
        time = (int)nowtime;
        timelab.guicontent.text = "";
        if(time/3600 > 0)
        {
            timelab.guicontent.text += time/3600 + "时";
        }
        if(time%3600/60 > 0||timelab.guicontent.text != "")
        {
            timelab.guicontent.text += time%3600/60 + "分";
        }
            
            timelab.guicontent.text += time%60 + "秒";
    }


    public void addscore(int score)
    {
        nowscore += score;
        scorelab.guicontent.text = nowscore.ToString("0");
    }

    public void Updatablood(int maxhp, int nowhp)
    {
        blood.guipos.width = (float)nowhp/maxhp*hpw;
    }

    



    
}
