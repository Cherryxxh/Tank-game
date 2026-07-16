using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginPanel : BasePanel<BeginPanel>
{
    public CustomGuiButton btnbegin;
    public CustomGuiButton btnsetting;
    public CustomGuiButton btnquit;

    public CustomGuiButton btnrank;

    

    // Start is called before the first frame update
    void Start()
    {
        btnbegin.ClickEvent += () =>
        {
            SceneManager.LoadScene("Gamescene");
        };
        btnsetting.ClickEvent += () =>
        {
            settingpanel.Instance.ShowMe();
            HideMe();
        };
        btnquit.ClickEvent += () =>
        {
            Application.Quit();
        };
        btnrank.ClickEvent += () =>
        {
            
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
