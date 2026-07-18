using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destoryself : MonoBehaviour
{


    public float delaytime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, delaytime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
