using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyGameController : MonoBehaviour
{//fazer metodo de selection, o button vai trazer atravez da creator um valor e uma acao do switch
    [Header("Game general")]
    public  int points;
    public  string[] damageTypes;

    [Header("Player")]
    public playerScript player;
    public GameObject[] lifes;
    public GameObject[] shots;

    [Header("Fraquezas x Tipo")]
    public float[] normalWeaknesses;
    public float[] fireWeaknesses;
    public float[] waterWeaknesses;
    public float[] iceWeaknesses;
    public float[] acidWeaknesses;
    public float[] darknessWeaknesses;

    [SerializeField]
    private GameObject pauseInterface = null;
    private bool onPause=false;

    void Start(){
        player = FindObjectOfType(typeof(playerScript)) as playerScript;
    }

    void Update(){
        checkKeyInput();
    }

    /*{
        "Normal", //Normal, sem efeitos
        "Fire", // Deixa gama levemente vermelho
        "Water", //Levemente azul
        "Ice", //Levementente azul claro, nao permite movimento e em curto tempo o deixa escorregadio
        "Acid", //Levemente verde claro
        "Darkness" //Levemente preto, deixa visao escura(criar um canvas preto de gama baixo)
    }*/

 /*   public float translateMultiplier(string dmgType, string enemyType){
        switch(dmgType){
            case "Normal":
                return collectMultiplier(0,enemyType);
            case "Fire":
                return collectMultiplier(1,enemyType);
            case "Water":
                return collectMultiplier(2,enemyType);
            case "Ice":
                return collectMultiplier(3,enemyType);
            case "Acid":
                return collectMultiplier(4,enemyType);
            case "Darkness":
                return collectMultiplier(5,enemyType);
        }
    }*/

     public void sumPoints(int quantity){
        points+=quantity;
    }


    public void changeScene(string scene){
        SceneManager.LoadScene(scene);
        if(Time.timeScale == 0){
               ResumeGame();
        }

    }

    public void updatePlayerUI(string UItem,int value){
        switch(UItem){
            case "health":
                for(int i=0;i<3;i++){
                    lifes[i].GetComponent<SpriteRenderer>().enabled=false;
                }
                for(int i=0;i<value;i++){
                    lifes[i].GetComponent<SpriteRenderer>().enabled=true;
                }
                break;
        }

    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseInterface.SetActive(true);
        
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseInterface.SetActive(false);
        
    }

    void checkKeyInput(){
       if(Input.GetButtonDown("Cancel")){
           if(Time.timeScale == 1){
               PauseGame();
               
           }else if(Time.timeScale == 0){
               ResumeGame();
           }
           print(Time.timeScale);
       }
    }
}
