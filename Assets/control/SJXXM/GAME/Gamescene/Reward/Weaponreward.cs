using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weaponreward : MonoBehaviour
{
    public GameObject[] weaponobj;

    public GameObject getobj;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            int index = Random.Range(0, weaponobj.Length);
            Playerobj player = other.GetComponent<Playerobj>();
            player.changeweapon(weaponobj[index]);
            GameObject eff =  Instantiate(getobj, this.transform.position,
             this.transform.rotation);
            AudioSource audio = eff.GetComponent<AudioSource>();
            audio.volume = GameDatamgr.Instance.musicData.soundvolue;
            audio.mute = GameDatamgr.Instance.musicData.isopensound;
             
            Destroy(gameObject);

        }
    }
}
