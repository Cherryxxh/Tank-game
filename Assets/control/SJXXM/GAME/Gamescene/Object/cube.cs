using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour
{
    public GameObject[] rewardobj;

    public GameObject deadobj;

    private void OnTriggerEnter(Collider other)
    {
        int random = Random.Range(0, 100);
        int index = Random.Range(0, rewardobj.Length);
        if(random < 50)
        {
            Instantiate(rewardobj[index], this.transform.position,
             this.transform.rotation);
        }
        if(random >= 50)
        {
            
        }
        GameObject deadeff = Instantiate(deadobj, this.transform.position,
             this.transform.rotation);
        AudioSource audio = deadeff.GetComponent<AudioSource>();
        audio.volume = GameDatamgr.Instance.musicData.soundvolue;
        audio.mute = !GameDatamgr.Instance.musicData.isopensound;
        
        
        Destroy(gameObject);
    }
}
