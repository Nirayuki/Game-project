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

    public bool stunned;

    [SerializeField] public int idAnimation; //indica id de animacao a ser executada
    public bool attacking; // indica se esta executando um ataque

    public bool attackingMelee;

    //Gerenciamento de animacoes - Ataque/arma
    public GameObject[] animAtaque;
    public Transform arrowPoint;

    [Header("Vida e info personagem")]
    public float health=100;
    public float actualHealth=100;
    public Transform groundCheck;
    public float speed; // Variavel que armazena a velocidade do movimento
    public float jumpForce; //armazena a forca aplicada no pulo do personagem
    public bool lookLeft=false; //Armazena o resultado da validacao para qual lado estamos olhando
    
    public Vector3 velocity;

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
    public float charges=3;

    [SerializeField]
    public float actualCharges;

    public float mana=100;

    public float actualMana=100;

    private bool reloading=false;

    private bool tired=false;

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
        //velocity = rig.velocity;
        grounded = Physics2D.OverlapCircle(groundCheck.position,0.1448299f,whatIsGround); // Usado para validar com um true ou false se detecta uma colisao com o chao
                                                                                
        if(!stunned){
            rig.velocity = new Vector2(horizontal*speed,rig.velocity.y);
            if(horizontal>0 && lookLeft == true && !attacking){ // Dadas as variaveis anteriores ela valida para qual lado estamos olhando, por exemplo a horizntal
                flip(); 
                                                       //Armazena entre > < ou igual a 0 (sendo que 0 esta parado e -1 ou 1 depende para o lado que esta olhando tendo em vista)
            }else if(horizontal<0 && lookLeft==false && !attacking ){//horizontal>1 igual a direita
                flip();
            } 
        }
        else{
            //rig.velocity = new Vector2(0f,0f);
           
        }

        

        detectarObjeto();
    }
    
    void Update()// Executado em cada frame apos o Start
    {
            //A cada x intervalos, se actualCharges < charges ele vai dar um yield return wait for seconds 
        // e assim carregar a cada x trechos, mas vai ter outra var bool de reloading pra n rodar mais que um de
        //uma vez logo if(!reloading) reload() yield e por ai vai sequencial

            checkHealth();

            if(mgController.inPause()==false){
                StartCoroutine("rechargeShots");
        

                horizontal = Input.GetAxisRaw("Horizontal"); //Coleta as direcoes horizontal e vertical no momento
                vertical = Input.GetAxisRaw("Vertical");    

                

                if(!stunned){
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
                    if(Input.GetButtonDown("Jump" ) && grounded ){
                        rig.AddForce(new Vector2(0,jumpForce));//O primeiro seria o impulso para o x entao poderiamos criar um knock back ou front
                        sManager.play("PJump");
                        
                    }

                    if(Input.GetButtonDown("Fire1") && detectedObject!=null ){ // when not in combat too
                        detectedObject.SendMessage("use");
                    } else if(Input.GetButtonDown("Fire1") && detectedObject==null && actualCharges>0 ){
                        anim.SetTrigger("Shot");//reset bar and recharbe bar
                        tired=true;

                        //StartCoroutine(rechargeEnergy()); for magic spells, shooting envolves charges and etc, so it has some differente recharge mechanic
                    }

                    if(attacking && grounded){
                        horizontal=0;
                    }
                }
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
    

     public void shot(){
        if(actualCharges>0){
            actualCharges--;
            GameObject shot =    Instantiate(mgController.shots[dmgType],arrowPoint.position,arrowPoint.rotation);
            
            if(lookLeft){//fixDirection()
                shot.GetComponent<SpriteRenderer>().flipX=true;
            }else{
                shot.GetComponent<SpriteRenderer>().flipX=false;
            }

            shot.GetComponent<Rigidbody2D>().AddForce(new Vector2(shot.GetComponent<Projectile>().getArrowForce()*dir.x,0));
            
            //in recharge
            /*
            shot.GetComponent<Rigidbody2D>().AddForce(new Vector2(shot.GetComponent<Arrow>().getArrowForce()*dir.x,0));// should be a method in Arrow Script

            yield return new WaitForSeconds(1f);
            shooting=false;
            */

        }
    }

    private IEnumerator rechargeShots(){//gun/bow selected, something with need reload when bullets or any are empty
        if(!reloading && actualCharges<charges){
            reloading=true;
            yield return new WaitForSeconds(2f);
            actualCharges++;
            reloading=false;
        }
    }

    /*private IEnumerator rechargeEnergy(){//magic selected
        if(tired){
            yield return new WaitForSeconds(2.0f);
        }

        tired = false;
    }*/

    // Collisoes
     //Podemos utilizar o funcionamento por tags, layer ou ate other.gameObject.name que
     //seria o nome do objeto na cena
     //Sugestao caixa, deixar colisor logo acima, como um colisor adicional porem apenas para destrui-la?
     //On collision, ou trigger com moeda adiciona a variavel de pontos que vai ser exposta num 
     //Gameobject com update sempre a variavel exposta de texto de acordo com player.pontos
     //GameController faria isso

     //da pra usar tags para definir tipo de dano etc
    private void  OnCollisionEnter2D(Collision2D other) {
        Vector3 objColPos = other.gameObject.GetComponent<Transform>().position;


        if(other.gameObject.tag=="CanBreak"){
            rig.AddForce(new Vector2(0,jumpForce));
            Destroy(other.gameObject,2);
            Debug.Log("Tum!");
        }else if(other.gameObject.tag=="DamageKnock"){
            inflictDamage(5f);
            rig.AddForce(new Vector2(0,250));
            stun(0.3f);
            
            
        }
        else if(other.gameObject.tag=="enemy"){
            if(!stunned){

                StartCoroutine(applyKnockBack(0.02f,150,(transform.position.x-objColPos.x)/Mathf.Abs((transform.position.x-objColPos.x))));
                StartCoroutine(stun(0.3f));
                inflictDamage(5f);
                
            }
        }
        else if (other.gameObject.tag == "Finish")
        {
            mgController.changeScene("Menu");
        }

        else if(other.gameObject.tag == "changeGun"){
            dmgType = other.gameObject.GetComponent<Projectile>().getArrowType();
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
                inflictDamage(10);
                Destroy(other.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other){
        
    }

    

    //Todo Seria utilizado para reproduzir um som quando chamada tal funcao
    
    public void inflictDamage(float damage){
        damage = evictOverheal(damage);
        if (health>0){
                actualHealth-=damage;
                mgController.updateHealthUI();
                if (actualHealth == 0)//checkHealth() method?
                {
                    mgController.gameOver();
                }

            }
    }

    

    public IEnumerator applyKnockBack(float knockDur,float knockPwr, float direction){
        
        float timer = 0;
        rig.velocity = new Vector2(rig.velocity.x, 0);
        
        
        while(knockDur>timer){
            timer+= Time.deltaTime;
            rig.AddForce(new Vector2(direction*100,knockPwr));
        }
        yield return 0;
    }


    IEnumerator stun(float duration){
        stunned=true;
        setAnimation("stun",true);
        yield return new WaitForSeconds(duration);
        setAnimation("stun",false);
        stunned=false;
    }

    void checkHealth(){
        if(actualHealth<=0){
            mgController.gameOver();
        }
    }

    public void triggerShot(){
        shot();
    }

   
    public float getHealthPercent(){

        return actualHealth/health;
        
    }

    public string getHealthRelation(){

        return actualHealth+"/"+health;

    }

    float evictOverheal(float heal){
        if(heal<0){
            if((actualHealth-heal)>health){
                return -(health-actualHealth);
            }
        }

        return heal;
    }

    void setAnimation(string name,bool condition){
        switch(name){
            case "stun":
                if(condition){
                    anim.SetBool("stunned",true);
                    mgController.enableHitUI(true);
                }else if(!condition){
                    anim.SetBool("stunned",false);
                    mgController.enableHitUI(false);
                }
                break;
            
        }
    }

}
