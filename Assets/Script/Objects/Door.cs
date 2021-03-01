 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator anim = null;
    private SoundManager SManager  = null;

    private Transform localPosition = null;

    private MyGameController mgController = null;


    [Header("Finalizacao de fase")]
    public Transform endPst = null;

    [SerializeField]
    private GameObject endScreen = null;

    [SerializeField]
    private GameObject endTransparency = null;

    [SerializeField]
    private float fadeFreq = 0f;

    [SerializeField]
    private Color endColor = Color.black;

    [SerializeField]
    private string nextLevel = null;


    //Auxiliar variables
    private IEnumerator coroutine = null;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        SManager = FindObjectOfType(typeof(SoundManager)) as SoundManager;
        mgController = FindObjectOfType(typeof(MyGameController)) as MyGameController;
        localPosition = GetComponent<Transform>();
    }


    // Update is called once per frame
    public void use(){//use opened varw
    SManager.play("doorOpening");
        anim.SetBool("open",true);
                
        endTransparency = Instantiate(endScreen,endPst.position,endPst.rotation);
        
        //while not 1 then continue
        coroutine = fade(fadeFreq);
        StartCoroutine(coroutine);

    }


    IEnumerator fade(float speed){
        float limit;

        if(speed<0){//will analyze and then tell if its decaying or ascending
            limit=0f;
            endColor.a=1f;
        }else{
            limit=1f;
            endColor.a=0f;
        }

        while((limit==1f && endColor.a<limit) ||  (limit==0f && endColor.a>limit) ){
            endColor.a+=speed;
            endTransparency.GetComponent<SpriteRenderer>().color=endColor;
            yield return new WaitForSeconds(0.06f);
        }


        mgController.changeScene(nextLevel);
    }


}
