using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monstertower : Tankbase
{
    public GameObject bulletobj;

    public float shoottime =1f;
    private float nowtime = 0f;
    public Transform[] shootpos;

    public override void Attack()
    {
        for(int i = 0; i < shootpos.Length; i++)
        {
            GameObject bullet = Instantiate(bulletobj, shootpos[i].position,
            shootpos[i].rotation);
            bulletobj obj = bullet.GetComponent<bulletobj>();
            obj.Setfather(this);
        }
    }

    public override void Wound(Tankbase tank)
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        nowtime += Time.deltaTime;
        if(nowtime >= shoottime)
        {
            nowtime = 0f;
            Attack();
        }
    }
}
