using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible_Scenario : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="arma"){
            Destroy(gameObject);
        }
    }
}
