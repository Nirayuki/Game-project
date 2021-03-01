using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bee_move : MonoBehaviour
{
    public float speed;
    public float pastPosition;
    public float actualPosition;
    public float time;

    private SpriteRenderer spRenderer;
    private Rigidbody2D rig;
    // Start is called before the first frame update
    void Start()
    {
        pastPosition=transform.position.x;

        spRenderer = GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();
        Invoke("flip",time);
    }

    // Update is called once per frame
    void Update()
    {
        actualPosition = transform.position.x;
        
         rig.AddForce(new Vector2(speed,0));
    
    }

    void flip(){
        speed *=-1;
        GetComponent<SpriteRenderer>().flipX=!GetComponent<SpriteRenderer>().flipX;
        Invoke("flip",time);
    }
}
