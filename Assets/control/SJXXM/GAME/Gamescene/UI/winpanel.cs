using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class winpanel : BasePanel<winpanel>
{

    public CustomGuiButton yesbtn;

    public CustomGuiinput inputinfo;
    // Start is called before the first frame update
    void Start()
    {
        yesbtn.ClickEvent += (() =>
        {
            GameDatamgr.Instance.AddRankinfo(inputinfo.guicontent.text, 
            Gamepanel.Instance.nowscore, Gamepanel.Instance.nowtime);

            SceneManager.LoadScene("Beginscene");
        });

        HideMe();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
