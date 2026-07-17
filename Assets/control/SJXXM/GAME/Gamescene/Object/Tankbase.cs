using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tankbase : MonoBehaviour
{
    public int atk;
    public int def;
    public int hp;
    public int maxhp;
    public int moveSpeed =10;
    public int rotateheadSpeed =100;
    public int rotateroundSpeed =100;

    public Transform tankhead;  
    

    public GameObject deadeff;

    public abstract void Attack();
    

    public virtual void Die()
    { 
        Destroy(this.gameObject);
        if (deadeff != null)
        {
            GameObject effobj = Instantiate(deadeff,
            this.transform.position, this.transform.rotation);
            AudioSource audio = effobj.GetComponent<AudioSource>();
            audio.volume = GameDatamgr.Instance.musicData.soundvolue;
            audio.mute = ! GameDatamgr.Instance.musicData.isopensound;
            audio.Play();
        }
    }
    

    public virtual void Wound(Tankbase other)
    {
        int dmg = other.atk - this.def;
        if (dmg > 0)
        {
            this.hp -= dmg;
        }
        if (dmg <= 0)
        {
            return;
        }
        if (this.hp <= 0)
        {
            this.hp = 0;
            this.Die();
        }
    }
    
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
