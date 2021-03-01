using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launcher : MonoBehaviour
{

    [Header("ObjetoAtirado")]
    [SerializeField]
    private GameObject throwObject = null;

    [SerializeField]
    private GameObject shotObject = null;

    public Transform shotPosition = null;

    [Header("Controle de tiro")]
    [SerializeField]
    private float interval=0;

    [SerializeField]
    private Vector3 direction = Vector3.right;

    [SerializeField]
    private float force= 0f;

    private bool inInterval = false;

    [SerializeField]
    private bool activated=true;

    

    private IEnumerator coroutine;    

    // Update is called once per frame
    void Update(){
        coroutine = shot(interval);
        StartCoroutine(coroutine);
    }

    IEnumerator shot(float interval){
        if(!inInterval && activated){
            inInterval=true;
            shotObject = Instantiate(throwObject,transform.position,transform.rotation);

            shotObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(force*direction.x,0));

            yield return new WaitForSeconds(interval);
            inInterval=false;
        }
    }

    void activate(){
        activated=true;
    }

    void deactivate(){
        activated=false;
    }
}
