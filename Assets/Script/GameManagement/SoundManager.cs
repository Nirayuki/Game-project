using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour {
    public   AudioClip sound ;
    public  AudioSource audioSource; 

    //Jogador
    public  AudioClip jumpPlayer;
    public  AudioClip atkPlayer;

    //Slime
    public  AudioClip jumpSlime;

    //Music
    public AudioClip BMusic;
    
    //Interacoes
    public AudioClip doorOpening;


    void Start(){

        audioSource = new AudioSource();
        audioSource = GetComponent<AudioSource>();
        play("BMusic");
    }

    public void play(string description){

        switch(description){
            case "PJump":
                audioSource.PlayOneShot(jumpPlayer);
                return;

            
            case "PAttack":
                audioSource.PlayOneShot(atkPlayer);
                return;

            
            case "SJump":
              audioSource.PlayOneShot(jumpSlime);
                return;


            case "BMusic":
                audioSource.PlayOneShot(BMusic);
                return;

            case "doorOpening":
                audioSource.PlayOneShot(doorOpening);
                return;
 
        }//Assign an then, if not null, play it

        if(audioSource.clip!=null && audioSource.clip!=BMusic){
            audioSource.Play();
        }
    }
}
