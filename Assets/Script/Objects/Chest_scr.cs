using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest_scr : MonoBehaviour
{
    MyGameController gmController;
    public SpriteRenderer sprRenderer ;
    public Sprite[] sprites;
    public bool open=false;

    void Start(){
        sprRenderer = GetComponent<SpriteRenderer>();
        gmController = FindObjectOfType(typeof(MyGameController) ) as MyGameController;
    }

    public void use(){
        open = !open;
        GetComponent<BoxCollider2D>().isTrigger = !GetComponent<BoxCollider2D>().isTrigger;
        switch(open){
            case true:
                sprRenderer.sprite = sprites[1];

                break;

            case false:
                sprRenderer.sprite = sprites[0];
                break;
        }
    }
}
