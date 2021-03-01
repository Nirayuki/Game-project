using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling_Platform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag=="Player"){
            Invoke("fall",2);
        }
    }

    void fall(){
        print("Falling.");
        GetComponent<SpringJoint2D>().enabled=false;
        GetComponent<BoxCollider2D>().enabled=false;
    }
}
