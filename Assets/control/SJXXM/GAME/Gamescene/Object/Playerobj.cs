using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerobj : Tankbase
{
    public Weaponobj weapon;

    public override void Attack()
    {
        if (weapon != null)
        {
            weapon.Shoot();
        }
    }

    public override void Die()
    {
        base.Die();
    }

    public override void Wound(Tankbase other)
    {
        base.Wound(other);
        Gamepanel.Instance.Updatablood(this.maxhp, this.hp);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Input.GetAxis("Vertical") * Vector3.forward 
        * moveSpeed * Time.deltaTime);
        this.transform.Rotate(Input.GetAxis("Horizontal") * Vector3.up 
        * rotateroundSpeed * Time.deltaTime);
        tankhead.transform.Rotate(Input.GetAxis("Mouse X") * Vector3.up 
        * rotateheadSpeed * Time.deltaTime);
        if (Input.GetMouseButtonDown(0))
        {
            this.Attack();
        }
    }
}
