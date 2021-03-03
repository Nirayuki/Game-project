using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float arrowForce=0;
 
    
    [SerializeField]
    private int dmgType;


    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Enemy" ){
            
        }
    }

    public float getArrowForce(){
        return arrowForce;
    }

    public int getArrowType(){
        return dmgType;
    }

    
}
