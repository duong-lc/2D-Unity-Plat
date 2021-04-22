using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class PlayerBehavior : MonoBehaviour
{
    private GameObject playerKatana;
    private GameObject playerArcher;
    private GameObject playerHeavy;
    private GameObject playerMage;
    public float moveInput;
    private Player_KatanaBehavior playerKatanaScript;
    private Player_ArcherBehavior playerArcherScript;
    private Player_HeavyBehavior playerHeavyScript;
    private Player_MageBehavior playerMageScript;

    public Rigidbody2D playerRB;

    //Vars for checking ground contact and Jumping Mechanic
    public bool isGrounded;
    public Transform groundCheck; //transform of gameobject attached to the foot of player sprite to see if it has came in contact with the ground or not
    public float checkRadius; //check radius for that gameobject
    public LayerMask GroundLayer;//telling the ground check gameobject what to check for
    private float jumpForce;

    public int currentCharacter;

    public bool isKatanaAlive = true;
    public bool isArcherAlive = true;
    public bool isHeavyAlive = true;
    public bool isMageAlive = true;

    public List<bool> AliveList = new List<bool>();

    public Transform collisionBox;
    public Vector2 colBoxDimension;
    public Collider2D[] objectCollidedWith;
    public LayerMask enemiesLayer;
    public float SpikeDamageToPlayer;

    public Character_Select_UI character_Select_UI;


    // Start is called before the first frame update
    void Start()
    {
        AliveList.Add(isKatanaAlive);
        AliveList.Add(isArcherAlive);
        AliveList.Add(isHeavyAlive);
        AliveList.Add(isMageAlive);

        currentCharacter = 1;
        
        playerRB = this.gameObject.GetComponent<Rigidbody2D>();
        
        try{
            playerKatana = GameObject.Find("Player-Katana").gameObject;
            playerArcher = GameObject.Find("Player-Archer").gameObject;
            playerHeavy = GameObject.Find("Player-Heavy").gameObject;
            playerMage = GameObject.Find("Player-Mage").gameObject;
        }
        catch(Exception e)
        {
            ;
        }
        
        //refreshing the system at start
        ActivateKatanaOnly();
        ActivateArcherOnly();
        ActivateKatanaOnly();
        //show off current character from the start
        character_Select_UI.SwitchCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        objectCollidedWith = Physics2D.OverlapBoxAll(collisionBox.position, colBoxDimension, 0f, enemiesLayer);
        playerRB.freezeRotation = true;
        //setting isGrounded to whether the overlap circle has overlapped with layer "ground"
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, GroundLayer);
        moveInput = Input.GetAxis("Horizontal");
        //y_Velocity = playerRB.velocity.y;
        
        PlayerJumping();
        PlayerRunning();
        SwitchingCharacter();

        //if(objectCollidedWith!= null)
           // ScanContact();

           
        
    }


    void PlayerJumping()
    {
        if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            playerRB.velocity = Vector2.up * jumpForce;  
        }

        
        try{
            if (currentCharacter == 1)
            {
                jumpForce = playerKatanaScript.jumpForce;
                playerKatanaScript.PlayerJumping();

            }
            else if (currentCharacter == 2)
            {
                jumpForce = playerArcherScript.jumpForce;
                playerArcherScript.PlayerJumping();
            }
            else if (currentCharacter == 3)
            {
                jumpForce = playerHeavyScript.jumpForce;
                playerHeavyScript.PlayerJumping();
            }
            else if (currentCharacter == 4)
            {
                jumpForce = playerMageScript.jumpForce;
                playerMageScript.PlayerJumping();
            }
        }catch (Exception e){;}
        
    }

    void PlayerRunning()
    {
        if (currentCharacter == 1){
            playerKatanaScript = playerKatana.GetComponent<Player_KatanaBehavior>();
            //move left and right
            playerRB.velocity = new Vector2(moveInput * playerKatanaScript.speed,  playerRB.velocity.y);
            playerKatanaScript.PlayerRunning();   
        }
        else if (currentCharacter == 2){
            playerArcherScript = playerArcher.GetComponent<Player_ArcherBehavior>();
            //move left and right
            playerRB.velocity = new Vector2(moveInput * playerArcherScript.speed,  playerRB.velocity.y);
            playerArcherScript.PlayerRunning();
        }else if (currentCharacter == 3){
            playerHeavyScript = playerHeavy.GetComponent<Player_HeavyBehavior>();
            //move left and right
            playerRB.velocity = new Vector2(moveInput * playerHeavyScript.speed,  playerRB.velocity.y);
            playerHeavyScript.PlayerRunning();
        }else if (currentCharacter == 4){
            playerMageScript = playerMage.GetComponent<Player_MageBehavior>();
            //move left and right
            playerRB.velocity = new Vector2(moveInput * playerMageScript.speed,  playerRB.velocity.y);
            playerMageScript.PlayerRunning();
        }
    }

    void SwitchingCharacter()
    {   
        if (isKatanaAlive == true){
            if (Input.GetKeyDown(KeyCode.Alpha1)){
                ActivateKatanaOnly();
                currentCharacter = 1;
            }
        }
       
        if (isArcherAlive == true){
            if (Input.GetKeyDown(KeyCode.Alpha2)){
                ActivateArcherOnly();
                currentCharacter = 2;
            }
        }

        if (isHeavyAlive == true){
            if (Input.GetKeyDown(KeyCode.Alpha3)){
                ActivateHeavyOnly();
                currentCharacter = 3;
            }
        }

        if(isMageAlive == true){
            if (Input.GetKeyDown(KeyCode.Alpha4)){
                ActivateMageOnly();
                currentCharacter = 4;
            }
        }

        character_Select_UI.SwitchCharacter();
    }
    public void SwitchToAlive()
    {
        if(isArcherAlive == true || isKatanaAlive == true || isHeavyAlive == true || isMageAlive == true)
        {
            for (int i = 0; i < 4; i++)
            {
                if(AliveList[i] == true)
                {
                    if(i == 0){
                        currentCharacter = 1;
                        ActivateKatanaOnly();
                    }
                    else if(i==1){
                        currentCharacter = 2;
                        ActivateArcherOnly();
                    }
                    else if(i==2){
                        currentCharacter = 3;
                        ActivateHeavyOnly();
                    }
                    else if(i==3){
                        currentCharacter = 4;
                        ActivateMageOnly();
                    }
                }
            }
        }
        else if (isArcherAlive == false && isKatanaAlive == false && isHeavyAlive == false && isMageAlive == false){
           /* if(this.gameObject.tag != null){
                this.gameObject.tag = null;
            }*/
            
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
            this.enabled = false;
        }

        character_Select_UI.SwitchCharacter();
    }

    private void ActivateKatanaOnly()
    {
        playerKatana.SetActive(true);
        playerArcher.SetActive(false);
        playerHeavy.SetActive(false);
        playerMage.SetActive(false);
    }
    private void ActivateArcherOnly()
    {
        playerKatana.SetActive(false);
        playerArcher.SetActive(true);
        playerHeavy.SetActive(false);
        playerMage.SetActive(false);
    }
    private void ActivateHeavyOnly()
    {
        playerKatana.SetActive(false);
        playerArcher.SetActive(false);
        playerHeavy.SetActive(true);
        playerMage.SetActive(false);
    }
    private void ActivateMageOnly()
    {
        playerKatana.SetActive(false);
        playerArcher.SetActive(false);
        playerHeavy.SetActive(false);
        playerMage.SetActive(true);
    }

    void OnDrawGizmosSelected()
    {    
        if (collisionBox == null)
        {
            return;
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(collisionBox.position, colBoxDimension);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        
    }


    /*void ScanContact()
    {
        if (objectCollidedWith.gameObject.tag == "Spike")
        {
            Debug.Log("smt");
            Vector2 difference = objectCollidedWith.gameObject.transform.position - this.gameObject.transform.position;
            //Debug.Log(this.gameObject.transform.right * 80f * difference.normalized.x);
            playerRB.AddForce(this.gameObject.transform.right * 80f * difference.normalized.x, ForceMode2D.Impulse);
            playerRB.AddForce(this.gameObject.transform.up * 5f, ForceMode2D.Force);
            //DamageCurrCharSpike();
        }   
    }*/

    public void Contact()
    {
        StartCoroutine(ForceAdd());
    }

    private IEnumerator ForceAdd()
    {
        for(int i = 0; i < 24; i++)
        {
            playerRB.AddForce(this.gameObject.transform.up * 7f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.001f);
        }
    }
}
