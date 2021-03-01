using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scroll : MonoBehaviour
{
    public float speed = 0;
    void Start()
    {
        
    }

    
    void Update()
    {
        transform.position = new Vector3(transform.position.x+speed,transform.position.y,transform.position.z);
    }
}
