using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour//bow may implement weapopn
{
    public float damage;
    public string damageType;
    
    private MyGameController mgController;

    void Start(){
        mgController = FindObjectOfType(typeof(MyGameController)) as MyGameController;
    }

    void GetDmgType(){
        //mgController.translateType(damageType);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="enemy"){
            //Enemy enemy = other.GetComponent<Enemy>();
            //inflict(weapon.GetDmgType()*mgController.tranlateMultiplier(dmgType,enemy.getEnemyType()));
        }
    }
}
