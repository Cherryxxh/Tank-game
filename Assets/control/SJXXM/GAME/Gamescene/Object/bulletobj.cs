using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletobj : MonoBehaviour
{

    public Tankbase fatherobj;

    public float movespeed = 50f;

    public void Setfather(Tankbase father)
    {
        fatherobj = father;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("cube"))
        {
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
