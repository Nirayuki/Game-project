using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Detector e relacionados")]
    public LayerMask targets;

    public Vector2 right = new Vector2(1f,0);
    public Vector2 left = new Vector2(-1f,0);

    private RaycastHit2D detectorLeft;

    private RaycastHit2D detectorRight;

    public Transform detector;

    public GameObject detectedRight;
    public GameObject detectedLeft;

    [Header("Modulo de ataque")]
    public GameObject bulletObject;

    public bool cooldown;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        detectPlayer();
        react();
    }

    void detectPlayer(){
        detectorLeft = Physics2D.Raycast(detector.position,Vector2.left,5.3f,targets); // objeto de referencia para inicio do raycast, direcao, e comprimento, objeto colidido interativo
        detectorRight = Physics2D.Raycast(detector.position,Vector2.right,5.3f,targets);
    

        
        if(detectorLeft){
            detectedLeft = detectorLeft.collider.gameObject;
        }else{
            detectedLeft = null;
        }
        if(detectorRight){
            detectedRight = detectorRight.collider.gameObject;
        }else{
            detectedRight = null;
        }

        Debug.DrawRay(detector.position,right*5.3f, Color.red);
        Debug.DrawRay(detector.position,left*5.3f, Color.red);
    }

    void react(){
        if(detectedLeft!=null && !cooldown){
            StartCoroutine(shot(-1f));
        }else if(detectedRight!=null && !cooldown){
            StartCoroutine(shot(1f));
            
        }
    }

    IEnumerator shot(float dir){
        cooldown=true;
        GameObject shot = Instantiate(bulletObject,detector.position,detector.rotation);
        shot.GetComponent<Projectile>().setDirection(dir);

        yield return new WaitForSeconds(3f);
        cooldown=false;
    }

  
}
