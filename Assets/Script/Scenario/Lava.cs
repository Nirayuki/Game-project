using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    // Start is called before the first frame update
    private bool onWFlight;

    [SerializeField]
    private GameObject lavaShot = null;
    public int force;
[SerializeField]
    private Transform p = null;

    [SerializeField]
    private float floatingTime = 0f;

    

    private bool projectileFlying=false;
    
    void Start(){
         
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("shotLava");




    }

    IEnumerator shotLava(){
        if(!projectileFlying){
            projectileFlying=true;
            
            GameObject shot = Instantiate(lavaShot,p.position,p.rotation);
            shot.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,force));
            yield return new WaitForSeconds(floatingTime);
            projectileFlying=false;
        }
    }
}
