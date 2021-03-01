 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCover : MonoBehaviour
{

       [Header("Finalizacao de fase")]
    public Transform endPst;

    [SerializeField]
    private GameObject endScreen = null;


    [SerializeField]
    private float fadeFreq=0;

    [SerializeField]
    private Color endColor = Color.black;

    [SerializeField]
    private string nextLevel;




    //Auxiliar variables
    private IEnumerator coroutine;
    // Start is called before the first frame update
    void Start()
    {
        coroutine = fade(fadeFreq);
        StartCoroutine(coroutine);
    }

   IEnumerator fade(float speed){//inheritance with Door fade
        float limit;

        if(speed<0){//will analyze and then tell if its decaying or ascending
            limit=0f;
            endColor.a=1f;
        }else{
            limit=1f;
            endColor.a=0f;
        }

        while( (limit==1f && endColor.a<limit) ||  (limit==0f && endColor.a>limit)){
            endColor.a+=speed;
            endScreen.GetComponent<SpriteRenderer>().color=endColor;
            yield return new WaitForSeconds(0.02f);
        }
   }
}
