using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private MyGameController mGController;
    // Start is called before the first frame update
    void Start()
    {
        mGController = FindObjectOfType(typeof(MyGameController)) as MyGameController;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        string tag = other.gameObject.tag;

        if(tag=="Player"){
            Destroy(other.gameObject);
            Camera_scr.avisarMorte();
        }else if(tag=="Enemy"){
            mGController.sumPoints(5);
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        
    }
}
