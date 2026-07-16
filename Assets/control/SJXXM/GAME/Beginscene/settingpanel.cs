using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingpanel : BasePanel<settingpanel>
{
    public CustomGuiButton btnback;
    public CustomGuiToggle tog_music;
    public CustomGuiToggle tog_sound;
    public CustomGuiSlider slider_music;
    public CustomGuiSlider slider_sound;
    // Start is called before the first frame update
    void Start()
    {
        btnback.ClickEvent += () =>
        {
            HideMe();
            BeginPanel.Instance.ShowMe();
        };
        slider_music.ValueChange += (value) =>
        {
            GameDatamgr.Instance.musicvolue(value);
        };
        slider_sound.ValueChange += (value) =>
        {
            GameDatamgr.Instance.soundvolue(value);
        };
        tog_music.changevalue+= (value) =>
        {
            GameDatamgr.Instance.isopenbkmusic(value);
        };
        tog_sound.changevalue += (value) =>
        {
            GameDatamgr.Instance.isopensound(value);
        };

        HideMe();
    }

    public void Updatapanelinfo()
    {
        MusicData rmusicData = GameDatamgr.Instance.musicData;
        tog_music.issel = rmusicData.isopenbkmusic;
        tog_sound.issel = rmusicData.isopensound;
        slider_music.nowvalue = rmusicData.musicvolue;
        slider_sound.nowvalue = rmusicData.soundvolue;
    }
   
   public override void ShowMe()
   {
       base.ShowMe();
       Updatapanelinfo();
   }
}
