using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class losepanel : BasePanel<losepanel>
{
    public CustomGuiButton againbtn;

    public CustomGuiButton exitbtn;
    // Start is called before the first frame update
    void Start()
    {
        againbtn.ClickEvent += (() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Gamescene");
        });
        exitbtn.ClickEvent += (() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("BeginScene");
        });


        HideMe();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
