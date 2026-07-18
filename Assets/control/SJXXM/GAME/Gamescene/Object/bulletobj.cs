using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletobj : MonoBehaviour
{

    public Tankbase fatherobj;

    public float movespeed = 50f;

    public GameObject effobj;

    public void Setfather(Tankbase father)
    {
        fatherobj = father;
    }


    private void OnTriggerEnter(Collider other)
    {
        

        if(other.CompareTag("cube"))
        {
            if(effobj != null)
            {
            GameObject eff = Instantiate(effobj, this.transform.position, 
            this.transform.rotation);
            AudioSource audio = eff.GetComponent<AudioSource>();
            audio.volume = GameDatamgr.Instance.musicData.soundvolue;
            audio.mute = !GameDatamgr.Instance.musicData.isopensound;
            }
            Destroy(gameObject);
        }
        
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * movespeed * Time.deltaTime);
    }
}
