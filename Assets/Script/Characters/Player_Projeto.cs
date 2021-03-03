using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Projeto : MonoBehaviour
{


    //
    public int dmgType=0;

    private Animator anim;
    private Rigidbody2D rig; // Armazena a Rig responsavel por aplicar a fisica de peso e etc
    public LayerMask whatIsGround; //indica o que e superficie/chao/objetos colidiveis para o Groundcheck(para verificar se esta no chao)
    
    

    //Gerenciamento de Animacoes   - Movimento
    public bool grounded; //Pisa ou n pisa
    public int idAnimation; //indica id de animacao a ser executada
    public bool attacking; // indica se esta executando um ataque

    //Gerenciamento de animacoes - Ataque/arma
    public GameObject[] animAtaque;
    public Transform arrowPoint;

    [Header("Vida e info personagem")]
    public int health=2;
    public Transform groundCheck;
    public float speed; // Variavel que armazena a velocidade do movimento
    public float jumpForce; //armazena a forca aplicada no pulo do personagem
    public bool lookLeft=false; //Armazena o resultado da validacao para qual lado estamos olhando
    

    [Header("Interacao com objetos")]
    //Interacao com objetos
    public LayerMask interactive; //Indica o que e interativo e o que nao e via-layer
    public  Vector3 dir = Vector3.right;
    public Transform hand;
    public  GameObject detectedObject;

    [SerializeField]
    public static   RaycastHit2D hit;


    //Gerenciamento de som/jogo
    public SoundManager sManager;
    public MyGameController mgController;

    //Sons do personagem
    public AudioSource jump;

    [Header("Itens e Armas")]
    [SerializeField]
    private int charges=0;

    [SerializeField]
    private int actualCharges;

    private bool reloading=false;
    private bool shooting= false;
    




    

    private float horizontal,vertical;

    
    void Start()
    {
        jump = GetComponent<AudioSource>();
        anim = GetComponent<Animator>(); //Aqui foi coletada a referencia aos componentes e dada as variaveis
        rig  = GetComponent<Rigidbody2D>(); //Dessa forma podemos utilizar destas para utilizar dos metodos
       
       sManager = FindObjectOfType(typeof(SoundManager)) as SoundManager;
       mgController = FindObjectOfType(typeof(MyGameController)) as MyGameController;

        foreach(GameObject o in animAtaque){
            o.SetActive(false);
        }
    }


    void FixedUpdate(){ //taxa de atualizacao fixa 0.02, comandos relacionados a fisica
        grounded = Physics2D.OverlapCircle(groundCheck.position,0.002f,whatIsGround); // Usado para validar com um true ou false se detecta uma colisao com o chao
                                                                                      
        
        rig.velocity = new Vector2(horizontal*speed,rig.velocity.y);

        if(horizontal>0 && lookLeft == true && !attacking){ // Dadas as variaveis anteriores ela valida para qual lado estamos olhando, por exemplo a horizntal
            flip(); 
                                                       //Armazena entre > < ou igual a 0 (sendo que 0 esta parado e -1 ou 1 depende para o lado que esta olhando tendo em vista)
        }else if(horizontal<0 && lookLeft==false && !attacking){//horizontal>1 igual a direita
            flip();
        } 

        detectarObjeto();
    }
    
    void Update()// Executado em cada frame apos o Start
    {
        //A cada x intervalos, se actualCharges < charges ele vai dar um yield return wait for seconds 
       // e assim carregar a cada x trechos, mas vai ter outra var bool de reloading pra n rodar mais que um de
       //uma vez logo if(!reloading) reload() yield e por ai vai sequencial
       StartCoroutine("rechargeShots");
       

        horizontal = Input.GetAxisRaw("Horizontal"); //Coleta as direcoes horizontal e vertical no momento
        vertical = Input.GetAxisRaw("Vertical");    

        


        if(vertical<0){//Ja esse, tendo em vista que vertical seria para cima, seria o responsavel por saber se solicitamos uma subida ou nao
            idAnimation=2; //Pois de acordo com os controles se apertamos ^ o vertical >1
            if(grounded){ //logo , para baixo seria vertical<1
                horizontal=0;
            }
            
        }
        else if(horizontal!=0 ){//Aqui, de acordo com nossas regras definimos a animcao utilizada, mudando a idanimation que, no animator funciona para trocas de animacao
            idAnimation=1;
            
        }else{
            idAnimation=0;
        }

 
        //Comandos
        if(Input.GetButtonDown("Jump" ) && grounded){
            rig.AddForce(new Vector2(0,jumpForce));//O primeiro seria o impulso para o x entao poderiamos criar um knock back ou front
            sManager.play("PJump");
            jump.Play();
        }

        if(Input.GetButtonDown("Fire1") && detectedObject!=null){ // when not in combat too
            detectedObject.SendMessage("use");
        } else if(Input.GetButtonDown("Fire1") && detectedObject==null ){
            StartCoroutine("shot");
        }

        if(attacking && grounded){
            horizontal=0;
        }
      


      anim.SetBool("isGrounded",grounded);//Define as variaveis e parametros configurados no Animator, no geral podemos utiliza-las como condicoes de troca de animacao
      anim.SetInteger("idAnimation",idAnimation); // logo torna-se util acessa-las via script para fazer interacoes
      anim.SetFloat("SpeedW",rig.velocity.y); 
    }

    void flip(){
        lookLeft = !lookLeft;
        float x = transform.localScale.x;
        x *= -1;

        transform.localScale = new Vector3(x,transform.localScale.y,transform.localScale.z);

        dir.x=x;
        
    }
    

    public void detectarObjeto(){
        hit = Physics2D.Raycast(hand.position,dir,0.3f,interactive); // objeto de referencia para inicio do raycast, direcao, e comprimento, objeto colidido interativo
        Debug.DrawRay(hand.position,dir*0.3f, Color.red);

        if(hit){
            detectedObject = hit.collider.gameObject;

        }else{
            detectedObject = null;
        }

    }
    

     public IEnumerator shot(){
        if(!shooting){
            shooting=true;
            actualCharges--;
            GameObject shot =    Instantiate(mgController.shots[dmgType],arrowPoint.position,arrowPoint.rotation);

            shot.GetComponent<Rigidbody2D>().AddForce(new Vector2(shot.GetComponent<Arrow>().getArrowForce()*dir.x,0));// should be a method in Arrow Script

            yield return new WaitForSeconds(1f);
            shooting=false;
        }
    }

    private IEnumerator rechargeShots(){
        if(!reloading && actualCharges<charges){
            reloading=true;
            yield return new WaitForSeconds(0.2f);
            actualCharges++;
            reloading=false;
        }
    }

    // Collisoes
     //Podemos utilizar o funcionamento por tags, layer ou ate other.gameObject.name que
     //seria o nome do objeto na cena
     //Sugestao caixa, deixar colisor logo acima, como um colisor adicional porem apenas para destrui-la?
     //On collision, ou trigger com moeda adiciona a variavel de pontos que vai ser exposta num 
     //Gameobject com update sempre a variavel exposta de texto de acordo com player.pontos
     //GameController faria isso

     //da pra usar tags para definir tipo de dano etc
    private void  OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag=="CanBreak"){
            rig.AddForce(new Vector2(0,jumpForce));
            Destroy(other.gameObject,2);
            Debug.Log("Tum!");
        }else if(other.gameObject.tag=="DamageKnock"){
            takeDamage();
        }
        else if(other.gameObject.tag=="Enemy"){
            rig.AddForce(new Vector2(0,200));
        }
        else if (other.gameObject.tag == "Finish")
        {
            mgController.changeScene("Menu");
        }

        else if(other.gameObject.tag == "changeGun"){
            dmgType = other.gameObject.GetComponent<Arrow>().getArrowType();
            Destroy(other.gameObject);
        }
    }


     void OnTriggerEnter2D(Collider2D other) {//precisa arranjar uma forma de ativar o gameobject dos fire
        //Por exemplo ao trigger com os postes eles comecam a ser mexer pra baixo
        //usando a movimentacao como a do personagem porem sem getaxis
        //isso da pra jogar pra um script individual por objetos interativos
        if(other.gameObject.tag=="EnableFlame"){
            if(GameObject.Find("Fire").GetComponent<SpriteRenderer>().enabled==false){
                GameObject.Find("Fire").GetComponent<SpriteRenderer>().enabled=true;
                Debug.Log("Faca se luz");
            }
        } 
        else if(other.tag=="EnemyWeapon"){
                takeDamage();
                Destroy(other.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other){
        
    }


    //Todo Seria utilizado para reproduzir um som quando chamada tal funcao


   

    public void switchType(int tipoNum) {//better with the arrow class
        dmgType = tipoNum;
    }
    
    void takeDamage(){
        if (health>0){
                rig.AddForce(new Vector2(0,150f));
                health--; // usar de um metodo pra tudo isso de reduzir vida
                mgController.updatePlayerUI("health",health);
                Debug.Log("Ouch");
                if (health == 0)//checkHealth() method?
                {
                    mgController.changeScene("Menu");
                }

            }
    }

}
