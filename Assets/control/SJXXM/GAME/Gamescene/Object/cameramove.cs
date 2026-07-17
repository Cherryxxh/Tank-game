using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameramove : MonoBehaviour
{

    public Transform target;
    public float Height;
    public Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }
        else
        {
            pos.x = target.position.x;
            pos.y = Height;
            pos.z = target.position.z;
            transform.position = pos;
        }
    }
}
