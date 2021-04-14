using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    // Start is called before the first frame update
    private Player_Projeto player;
    void Start()
    {
        player = FindObjectOfType(typeof(Player_Projeto)) as Player_Projeto;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeAttackState(bool attackState){
        player.attackingMelee=attackState;
    }
}
