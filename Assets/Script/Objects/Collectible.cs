using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public string type;

    public float effectPower;

    public Player_Projeto player;

    void Start(){
        player = FindObjectOfType(typeof(Player_Projeto)) as Player_Projeto;
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag=="Player"){
            enableEffect();
        }
    }

    void enableEffect(){
        switch(type){
            case "heal":
                if(player.getHealthPercent()!=1f){
                    player.inflictDamage(effectPower);
                    //soundeffect
                    Destroy(gameObject);
                }
                break;
        }
    }
}
