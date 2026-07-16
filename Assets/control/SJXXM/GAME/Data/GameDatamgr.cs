using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDatamgr
{
    private static GameDatamgr instance = new GameDatamgr();

    public static GameDatamgr Instance { get => instance;  }
    
    public MusicData musicData;
    private GameDatamgr()
    {
        musicData = PlayerprefsdataMgr.Instance.
        LoadData(typeof(MusicData),"musicData") as MusicData;



        if (!musicData.notfirstopen)
        {
            musicData.notfirstopen = true;
            musicData.isopenbkmusic = true;
            musicData.isopensound = true;
            musicData.soundvolue = 1;
            musicData.musicvolue = 1;
            PlayerprefsdataMgr.Instance.SaveData(musicData,"musicData");
        }
    }

    public void isopenbkmusic(bool isopen)
    {
        musicData.isopenbkmusic = isopen;
        PlayerprefsdataMgr.Instance.SaveData(musicData,"musicData");
    }

    public void isopensound(bool isopen)
    {
        musicData.isopensound = isopen;
        PlayerprefsdataMgr.Instance.SaveData(musicData,"musicData");
    }

    public void musicvolue(float volue)
    {
        musicData.musicvolue = volue;
        PlayerprefsdataMgr.Instance.SaveData(musicData,"musicData");
    }

    public void soundvolue(float volue)
    {
        musicData.soundvolue = volue;
        PlayerprefsdataMgr.Instance.SaveData(musicData,"musicData");
    }
}
