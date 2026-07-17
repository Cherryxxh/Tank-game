using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weaponobj : MonoBehaviour
{
    public GameObject bullet;

    public Transform[] shootpos;

    public Tankbase tankown;

    public void Setowner(Tankbase owner)
    {
        tankown = owner;
    }
    
    public void Shoot()
    {
        for (int i = 0; i < shootpos.Length; i++)
        {
            GameObject obj = Instantiate(bullet,
                shootpos[i].position, shootpos[i].rotation);
            bulletobj bullett = obj.GetComponent<bulletobj>();
            bullett.Setfather(tankown);
        }
        
    }
}
