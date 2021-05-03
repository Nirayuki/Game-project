using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bee_move : MonoBehaviour
{

    public float speed;

    public float direction=1f;

    public float flyingTime=4f;

    private SpriteRenderer spRenderer=null;
    private Rigidbody2D rig;

    float timer=0;
    // Start is called before the first frame update
    void Start()
    {
        spRenderer = GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();

        //decideDirection();   
    }

    // Update is called once per frame
    void Update()
    {
        
        fly();
        fixDirection();

        
        timer+=Time.deltaTime;
        

    }

    void decideDirection(){ //Will wait x seconds or wall collide
        // if hits flyingTime trigger redirect, OR hits wall
        // can split to onCollide, but it needs to reset the timer, set flyingTimer Random+xf
            while(timer<flyingTime){// if method == true  it resets, otherwise, waits or resets acoording to collider
                print("1");
            }
            
            direction = -direction;
            resetFlyingTimer();
            decideDirection();
    }

    void fly(){
        rig.velocity = new Vector2(direction*speed,rig.velocity.y);// random +1f for flip or wall collide
    }

    void fixDirection(){
        if(speed>0 && spRenderer.flipX){
            spRenderer.flipX=false;
        }else if(speed<0 && !spRenderer.flipX){
            spRenderer.flipX=true;
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag=="ground"){
            direction = -direction;
            resetFlyingTimer();
        }
    }

    void resetFlyingTimer(){
        flyingTime = Random.Range(3f,5f);
        timer= 0;
    }
}
