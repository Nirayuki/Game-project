using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicEnemy : MonoBehaviour
{

    private Rigidbody2D rig;
    //Create a script just for living things
    public string name;
    
    [SerializeField]
    private float healthPoints;

    [SerializeField]
    private float speed;

    [SerializeField]
    private string dmgType;


    [SerializeField]
    private RaycastHit2D hit;

    [SerializeField]
    private GameObject detectedObject;

    [SerializeField]
    private bool foundPlayer=false;


    public Vector3 dir = Vector3.right;

    private MyGameController mgController;


    // Start is called before the first frame update
    void Start()
    {

        rig= GetComponent<Rigidbody2D>();

        mgController = FindObjectOfType(typeof(MyGameController)) as MyGameController;
    }

    // Update is called once per frame
    void Update()
    {
        detect();
        move();
        
    }

    void detect(){
        //hit = Physics2D.Raycast(hand.position,dir,0.3f,interactive); // objeto de referencia para inicio do raycast, direcao, e comprimento, objeto colidido interativo
        

        //if(hit){
        //    detectedObject = hit.collider.gameObject;

        //}else{
       //     detectedObject = null;
    //    }

    }

    void move(){
        if(foundPlayer){
            followPlayer();
        }else{
            randomMove();
        }
    }


    void followPlayer(){

    }

    void randomMove(){
        //actualPosition = transform.position.x;
        //if(actualPosition>=(pastPosition+2f) || actualPosition<=(pastPosition-2f)){
        //    speed *=-1;
         //   GetComponent<SpriteRenderer>().flipX=!GetComponent<SpriteRenderer>().flipX;
           
        //}
         //transform.position = new Vector3(transform.position.x+speed,transform.position.y,transform.position.z);
         print("move");
         rig.AddForce(new Vector2(speed,0));
         

    }


    void OnCollisionEnter2D(Collision2D other){// pegar o multiplicador do mygameController
        
        if(other.gameObject.tag=="weapon"){
            //use getComponent from the gameobject collecting his script script==class
        }
    }



}
