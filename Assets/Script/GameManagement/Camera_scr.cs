
using System.Globalization;
using System.Diagnostics;
using System.Security;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_scr : MonoBehaviour
{
    public Vector3 offset; 
    private GameObject playerObj = null;
    public Camera maincam = null;
    public static bool playerMorreu ;
    // Start is called before the first frame update
    void Start()
    {
       playerMorreu = false;
       if (playerObj == null)
             playerObj = GameObject.Find("Personagem");
        
         maincam = GameObject.Find("Main Camera").GetComponent<Camera>(); 
         //transform.position = playerObj.transform.position; 
         offset = maincam.transform.position - playerObj.transform.position;
         maincam.transform.position = playerObj.transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        mudaConformePsg();
    }

    private void mudaConformePsg(){
        if(playerMorreu == false){
            maincam.transform.position = playerObj.transform.position + offset;
            transform.position = new Vector3(transform.position.x, playerObj.transform.position.y, transform.position.z);
        }

    }

    public static void avisarMorte(){playerMorreu=true;}


}
