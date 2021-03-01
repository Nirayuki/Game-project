using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
 public Vector3 offset; 
 [SerializeField]   
    private GameObject obj  = null;
    [SerializeField]
    private Player_Projeto personagem;
    public Camera maincam = null;
    public static bool playerMorreu ;

    private Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {

        
         maincam = GameObject.Find("Main Camera").GetComponent<Camera>(); 
         //transform.position = playerObj.transform.position; 
         offset = maincam.transform.position - obj.transform.position;
         maincam.transform.position = obj.transform.position + offset;

         
    }

    // Update is called once per frame
    void Update()
    {
        mudaConformePsg();
    }

    private void mudaConformePsg(){
            transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, transform.position.z);
    }

}
