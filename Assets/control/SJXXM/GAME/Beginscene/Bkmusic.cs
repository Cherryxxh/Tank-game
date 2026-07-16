using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bkmusic : MonoBehaviour
{

    public static Bkmusic instance;

    public static Bkmusic Instance => instance;

    private AudioSource audioSource;
    
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();

        changevalue(GameDatamgr.Instance.musicData.musicvolue);
        changeopen(GameDatamgr.Instance.musicData.isopenbkmusic);
    }


    public void changevalue(float value)
    {
        audioSource.volume = value;
    }

    public void changeopen(bool value)
    {
        audioSource.mute = !value;
    }
}
