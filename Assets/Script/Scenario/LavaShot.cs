using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaShot : MonoBehaviour
{

    private Rigidbody2D rig;
    private SpriteRenderer spRenderer;
    //damage apply, by time will be done ny something like 

    //damageDealerClass
    /*
    public void dealDamage(GameObject object,float damage,int seconds){
        float dmgPerTime = damage/seconds;
        while(damage>0){

        }
    }
    */
    // Where would it be better? gameController or a individual class?
    //Maybe a interesting thing to, implement it into GameController

    void Start(){
         rig= GetComponent<Rigidbody2D>();
         spRenderer = GetComponent<SpriteRenderer>();

         GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, 90f);
    }

    void Update(){
        if(rig.velocity.y>0){
            spRenderer.flipX=false;
        }else{
            spRenderer.flipX=true;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag=="destructPoint" && spRenderer.flipX==true){
            Destroy(gameObject);
            print("oh");
        }
    }
}
