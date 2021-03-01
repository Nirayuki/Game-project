using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject connectedObject;

    private SpriteRenderer spRenderer;

    public Sprite[] leverStates;

    private bool activated=false;
    // Update is called once per frame

    void Start(){
        spRenderer = GetComponent<SpriteRenderer>();
    }

    void use(){//deactivate and activate
        if(!activated){
            connectedObject.SendMessage("activate");
            spRenderer.sprite = leverStates[1];
            activated=true;
            print();//ss
        }else{
            connectedObject.SendMessage("deactivate");
            spRenderer.sprite = leverStates[0];
            activated=false;
        } 
    }
}
