using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PropType
    {
        atk,
        def,
        Hp,
        maxHp
    }
public class rewardprop : MonoBehaviour
{
    public PropType propType = PropType.atk;

    public GameObject getobj;

    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        
        
        if(other.CompareTag("Player"))
        {
            Playerobj player = other.GetComponent<Playerobj>();
            switch (propType)
            {
                case PropType.atk:
                    player.atk += 10;
                    break;
                case PropType.def:
                    player.def += 10;
                    break;
                case PropType.Hp:
                    player.hp += 10;
                    if(player.hp > player.maxhp)
                    {
                        player.hp = player.maxhp;
                        
                    }
                    Gamepanel.Instance.Updatablood(player.maxhp, player.hp);
                    break;
                case PropType.maxHp:
                    player.maxhp += 10;
                    Gamepanel.Instance.Updatablood(player.maxhp, player.hp);
                    break;
            }

            GameObject eff =  Instantiate(getobj, this.transform.position,
             this.transform.rotation);
            AudioSource audio = eff.GetComponent<AudioSource>();
            audio.volume = GameDatamgr.Instance.musicData.soundvolue;
            audio.mute = !GameDatamgr.Instance.musicData.isopensound;
            Destroy(gameObject);
        }
    }
}
