using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Slime_Move : MonoBehaviour
{

    //Atributos do inimigo
    [Header("Slime atributos")]
    public float[] resistances;
    public int health;

    private MyGameController gmController;

    private Rigidbody2D rig;
    public Vector3 dir = Vector3.left;


    [SerializeField]
    private int forceX=1000,forceY=1250;

    //Colisoes
    private bool inPlayerCollision=false; //Detecta colisao com player
    private bool inWeaponCollision=false; //Detecta colisao com arma



    public GameObject left;
    public GameObject right;
    public GameObject middle;

    //Sons Slime
    public AudioSource jumpSound;
    

    // Start is called before the first frame update
    void Start()
    {
        jumpSound = GetComponent<AudioSource>();
        gmController = FindObjectOfType(typeof(MyGameController)) as MyGameController;

        rig  = GetComponent<Rigidbody2D>();
        
        Invoke("jump",2); 
        
    }

    // Update is called once per frame
    void Update()
    {  
        
        
       

    }


    void jump(){
        
        flip();
        rig.AddForce(new Vector2(forceX,forceY));
        Invoke("jump",2); 
        
        //x1=-x1;
        //y=-y;
    }

    void flip(){
        
        
        float x = transform.localScale.x;
        x *= -1;

        transform.localScale = new Vector3(x,transform.localScale.y,transform.localScale.z);

        dir = -dir;
    }

    void OnCollisionEnter2D(Collision2D other){
        
    }

    void OnTriggerEnter2D(Collider2D other){
        switch(other.gameObject.tag){
            case "Player":
                if(inPlayerCollision==false){//Funcionamento de impacto ao player
                applyForceByTag("Player",other,inPlayerCollision);
                }
                break;

            case "Weapon":
                
                applyDamage(other);
                Destroy(other);
                print("what");
                applySelfForceByTag("Weapon",other,inWeaponCollision); //Impacto ao slime
                
                break;

            default:
                print("Triggered but havent found any switch, triggered with tag: "+other.gameObject.tag);
                break;
        }
        print("Triggered but havent switched, triggered with tag: "+other.gameObject.tag);
    }

    void applyForceByTag(string tag,Collider2D other,bool collisionControl){
        
        if(other.gameObject.tag==tag){
            if(other.transform.position.x<transform.position.x){
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(100f,200f));
                print("Esquerda");
                
            }else if(other.transform.position.x>transform.position.x){
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100f,200f));
                print("Direita");
            }
            collisionControl=true;
        }
    }

    void applySelfForceByTag(string tag,Collider2D other,bool collisionControl){
        print("hit");
        if(other.gameObject.tag==tag){
            if(other.transform.position.x<transform.position.x){
                GetComponent<Rigidbody2D>().AddForce(new Vector2(100f,200f));
                print("Esquerda");
                
            }else if(other.transform.position.x>transform.position.x){
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-100f,200f));
                print("Direita");
            }
            collisionControl=true;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        warnUncollideByTag(other.tag);
        

    }

    void warnUncollideByTag(string tag){
        if(tag=="Player"){
            inPlayerCollision=false;
        }else if(tag=="Weapon"){
            inWeaponCollision=false;
        }
    }

    void applyDamage(Collider2D other){
        //WeaponInfo info = other.GetComponent<WeaponInfo>();

        //int weaponDmgType = info.damageType;
       // float weaponDamage = info.damage;
    }

}
