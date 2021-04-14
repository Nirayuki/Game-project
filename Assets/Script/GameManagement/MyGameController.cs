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
    public Player_Projeto player;
    public Transform lifeBar;
    public Text healthText;
    public GameObject[] shots;

    [Header("Fraquezas x Tipo")]
    public float[] normalWeaknesses;
    public float[] fireWeaknesses;
    public float[] waterWeaknesses;
    public float[] iceWeaknesses;
    public float[] acidWeaknesses;
    public float[] darknessWeaknesses;

    
    [SerializeField]private GameObject pauseInterface = null;
    [SerializeField]private GameObject optionsInterface = null;
    private bool onPause=false;




    void Start(){
        player = FindObjectOfType(typeof(Player_Projeto)) as Player_Projeto;
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



    

    public void PauseGame()
    {
            Time.timeScale = 0;
            pauseInterface.SetActive(true);
            onPause=true;
    }

    public void ResumeGame()
    {
            Time.timeScale = 1;
            pauseInterface.SetActive(false);
            onPause=false;
    }

    public void enterOptions(){
            pauseInterface.SetActive(false);
            optionsInterface.SetActive(true);
    }
    
    public void exitOptions(){
            pauseInterface.SetActive(true);
            optionsInterface.SetActive(false);
    }


    void checkKeyInput(){
       if(Input.GetButtonDown("Cancel") && onPause==false){
           if(Time.timeScale == 1){
               PauseGame();
               
           }else if(Time.timeScale == 0){
               ResumeGame();
           }
           print(Time.timeScale);
       }
    }

    public bool inPause(){return onPause;}

    public void updateHealthUI(){

        float lifePercent = player.getHealthPercent();

        if(lifePercent>0f){
            //atualiza a barra de vida atual
            lifeBar.localScale = new Vector3(lifePercent, transform.localScale.y, transform.localScale.z);
            
            //atualiza o texto da quantidade de vida
            healthText.text = player.getHealthRelation();


        } else if(lifePercent==0f){
            lifeBar.localScale = new Vector3(lifePercent, transform.localScale.y, transform.localScale.z);

            healthText.text = "0/0";
        }
    }

    public void gameOver(){
        
    }
}
