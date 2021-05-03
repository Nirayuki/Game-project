using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyGameController : MonoBehaviour
{//fazer metodo de selection, o button vai trazer atravez da creator um valor e uma acao do switch

    [SerializeField] private GameObject UICharacter;

    [Header("Game general")]
    public  int points;
    public  string[] damageTypes;

    [Header("Player")]
    public Player_Projeto player;
    public Transform lifeBar;

    public Transform chargesBar;

    public Transform manaBar;

    public Text healthText;

    public Text manaText;

    public Text chargeText;
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

    [SerializeField]private GameObject gameOverInterface = null;

    

    private bool onPause=false;

    


    void Start(){
        player = FindObjectOfType(typeof(Player_Projeto)) as Player_Projeto;
        initiateUI();
    }

    void Update(){
        checkKeyInput();

        updateUI();
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
        changeTimeScale(0);
    }

    void initiateUI(){
        gameOverInterface.SetActive(false);
        onPause=false;

        changeTimeScale(1); 
    }



    public void gameOver(){
        gameOverInterface.SetActive(true);
        onPause=true;

        changeTimeScale(0); 
    }

    public void PauseGame()
    {
            changeTimeScale(0);
            setPause(true);
    }

    public void ResumeGame()
    {
            changeTimeScale(1);
            setPause(false);
    }

    public void enterOptions(){
            pauseInterface.SetActive(false);
            optionsInterface.SetActive(true);
    }
    
    public void exitOptions(){
            pauseInterface.SetActive(true);
            optionsInterface.SetActive(false);
    }

    void setPause(bool value){
        pauseInterface.SetActive(value);
        onPause=value;
    }

    void changeTimeScale(int value){
        if(Time.timeScale  != 0 && value == 0){
            Time.timeScale = value;
        }else if(Time.timeScale != 1 && value == 1){
            Time.timeScale = value;
        }
    }



    public void enableHitUI(bool boolean){

            UICharacter.GetComponent<Animator>().SetBool("stunned",boolean);

    }

    void updateUI(){
       updateManaUI();
       updateChargesUI(); 
    }

    void updateManaUI(){//compare mana in the txt and bar scale and changes, if needed compare if it changed or no, recycles
        manaText.text = player.actualMana + "/" + player.mana;

        manaBar.localScale = new Vector3((player.actualMana/player.mana), transform.localScale.y, transform.localScale.z);
    }

    void updateChargesUI(){//compare charges in the txt and bar scale nd changes, if needed
        chargeText.text = player.actualCharges + "/" + player.charges;

        chargesBar.localScale = new Vector3((player.actualCharges/player.charges), transform.localScale.y, transform.localScale.z);
    }




    void checkKeyInput(){
       if(Input.GetButtonDown("Cancel") && onPause==false){
           if(Time.timeScale == 1){
               PauseGame();
               
           }else if(Time.timeScale == 0){
               ResumeGame();
           }
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

    
}
