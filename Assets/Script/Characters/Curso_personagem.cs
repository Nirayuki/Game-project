using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerScript : MonoBehaviour
{
    [Header("Layers")]
    private Animator anim;
    private Rigidbody2D rig; // Armazena a Rig responsavel por aplicar a fisica de peso e etc
    public LayerMask whatIsGround; //indica o que e superficie/chao/objetos colidiveis para o Groundcheck(para verificar se esta no chao)
    public LayerMask interactive; //Indica o que e interativo e o que nao e via-layer
    

    //Gerenciamento de Animacoes   - Movimento
    public bool grounded; //Pisa ou n pisa
    public int idAnimation; //indica id de animacao a ser executada
    public bool attacking; // indica se esta executando um ataque

    //Gerenciamento de animacoes - Ataque/arma
    public GameObject[] animAtaque;

    [Header("Vida e info personagem")]
    public Transform groundCheck;
    public float speed; // Variavel que armazena a velocidade do movimento
    public float jumpForce; //armazena a forca aplicada no pulo do personagem
    public bool lookLeft=false; //Armazena o resultado da validacao para qual lado estamos olhando
    public int health=3;
    
    [Header("Itens e Armas")]
    [SerializeField]
    private int charges = 0;
    private int actualCharges;

    private bool reloading=false;

    //Interacao com objetos
    private  Vector3 dir = Vector3.right;
    public Transform hand;
    public  GameObject CanInteract;


    //Gerenciamento de som/jogo
    public SoundManager sManager;
    public MyGameController mgController;

    //Sons do personagem
    public AudioSource jump;

    //UI

    public string descUI= "Vidas: ";
    


    public Collider2D standing,crouching;


    

    private float horizontal,vertical;

    // Start is called before the first frame update
    void Start()
    {
        actualCharges=charges;
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
                                                                                      //detalhe das variaveis: groundCheck e nosso outro gameObject que esta logo abaixo do player
                                                                                      //Ele
        
        rig.velocity = new Vector2(horizontal*speed,rig.velocity.y);

        interagir();
    }
    // Update is called once per frame
    void Update()// Executado em cada frame apos o Start
    {
       //A cada x intervalos, se actualCharges < charges ele vai dar um yield return wait for seconds 
       // e assim carregar a cada x trechos, mas vai ter outra var bool de reloading pra n rodar mais que um de
       //uma vez logo if(!reloading) reload() yield e por ai vai sequencial
        StartCoroutine("rechargeShots");


        horizontal = Input.GetAxisRaw("Horizontal"); //Coleta as direcoes horizontal e vertical no momento
        vertical = Input.GetAxisRaw("Vertical");    

        if(horizontal>0 && lookLeft == true && !attacking){ // Dadas as variaveis anteriores ela valida para qual lado estamos olhando, por exemplo a horizntal
            flip(); 
                                                       //Armazena entre > < ou igual a 0 (sendo que 0 esta parado e -1 ou 1 depende para o lado que esta olhando tendo em vista)
        }else if(horizontal<0 && lookLeft==false && !attacking){//horizontal>1 igual a direita
            flip();
        } 


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

        if(Input.GetButtonDown("Fire1") && vertical>=0 && !attacking && !CanInteract){
            anim.SetTrigger("Attack");
            sManager.play("PAttack");
        }

        if(Input.GetButtonDown("Jump" ) && grounded){
            rig.AddForce(new Vector2(0,jumpForce));//O primeiro seria o impulso para o x entao poderiamos criar um knock back ou front
            sManager.play("PJump");
        }

        if(Input.GetButtonDown("Fire1") && CanInteract!=null){
            CanInteract.SendMessage("interact");
        }

        if(attacking && grounded){
            horizontal=0;
        }
      
        if(vertical>=0 && grounded){
            standing.enabled=true;
            crouching.enabled=false;
        }if(horizontal<0 && grounded){
            standing.enabled=false;
            crouching.enabled=true;
        }

      anim.SetBool("isGrounded",grounded);//Define as variaveis e parametros configurados no Animator, no geral podemos utiliza-las como condicoes de troca de animacao
      anim.SetInteger("idAnimation",idAnimation); // logo torna-se util acessa-las via script para fazer interacoes
      anim.SetFloat("speedW",rig.velocity.y); 

      
    }

    void flip(){
        lookLeft = !lookLeft;
        float x = transform.localScale.x;
        x *= -1;

        transform.localScale = new Vector3(x,transform.localScale.y,transform.localScale.z);

        dir.x=x;
    }

    
    //Utilizado para informar o parametro de animacao se estamos atacando ou nao
    public void attack(int atk){
        switch(atk){
            case 0:
                attacking=false;
                animAtaque[2].SetActive(false);
                break;
            case 1:
                attacking=true;
                break;
        }
        
    }

    public void setAnimWeapon(int id){
        foreach(GameObject o in animAtaque){
            o.SetActive(false);
        }

        animAtaque[id].SetActive(true);
    }

    

    public void interagir(){
        RaycastHit2D hit = Physics2D.Raycast(hand.position,dir,0.3f,interactive); // objeto de referencia para inicio do raycast, direcao, e comprimento
        Debug.DrawRay(hand.position,dir*0.3f, Color.red);

        if(hit){
            CanInteract = hit.collider.gameObject;
        }else{
            CanInteract = null;
        }

    }

    private IEnumerator rechargeShots(){
        if(!reloading && actualCharges<charges){
            yield return new WaitForSeconds(0.2f);
            actualCharges++;
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
            rig.AddForce(new Vector2(0,200));
            mgController.updatePlayerUI("health",health);
            Debug.Log("Ouch");
        }
        else if(other.gameObject.tag=="Enemy"){
            rig.AddForce(new Vector2(0,200));
        }
        
    }


    private void OnTriggerEnter2D(Collider2D other) {//precisa arranjar uma forma de ativar o gameobject dos fire
        //Por exemplo ao trigger com os postes eles comecam a ser mexer pra baixo
        //usando a movimentacao como a do personagem porem sem getaxis
        //isso da pra jogar pra um script individual por objetos interativos
        if(other.gameObject.tag=="EnableFlame"){
            if(GameObject.Find("Fire").GetComponent<SpriteRenderer>().enabled==false){
                GameObject.Find("Fire").GetComponent<SpriteRenderer>().enabled=true;
                Debug.Log("Faca se luz");
            }
        }
    }

    //Todo Seria utilizado para reproduzir um som quando chamada tal funcao


    


    //Seria mais ideal no GameController, usada para carregar outra fase
   //public void load(){
 //       SceneManager.LoadScene("Game1");
   // }

}
