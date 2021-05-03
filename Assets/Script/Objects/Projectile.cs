using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float arrowForce;

    private Rigidbody2D rig ;
 
    [SerializeField]private float dir;

    [SerializeField]private int dmgType;

    [SerializeField]private float damage;

    void Awake(){
        rig = GetComponent<Rigidbody2D>();
        
    }
    void Start(){
        print(dir*arrowForce);
        rig.AddForce(new Vector2(dir*arrowForce,0));
        Destroy(gameObject,5f);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="Player" && gameObject.tag=="EnemyProjectile"){
            other.gameObject.SendMessage("dealDamage",damage);
            other.gameObject.SendMessage("stun",2f);
            Destroy(gameObject);
        }
        if(other.gameObject.tag=="Enemy" && gameObject.tag=="Projectile"){
            
        }
    }

    public float getArrowForce(){
        return arrowForce;
    }

    public int getArrowType(){
        return dmgType;
    }

    public void setDirection(float direction){
        dir = direction;
    }

    public void autoDestroy(){
        Destroy(gameObject);
    }
}
