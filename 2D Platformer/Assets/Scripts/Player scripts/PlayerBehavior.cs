using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour
{
    private GameObject playerKatana, playerArcher, playerHeavy, playerMage;
    public float moveInput;
    public Rigidbody2D playerRB;

    //Vars for checking ground contact and Jumping Mechanic
    public bool isGrounded;
    public Transform groundCheck; //transform of gameobject attached to the foot of player sprite to see if it has came in contact with the ground or not
    public float checkRadius; //check radius for that gameobject
    public LayerMask GroundLayer;//telling the ground check gameobject what to check for
    private float jumpForce;

    public int currentCharacter;

    public bool isKatanaAlive = true, isArcherAlive = true, isHeavyAlive = true, isMageAlive = true;
    //public List<bool> AliveList = new List<bool>();

    public Transform collisionBox;
    public Vector2 colBoxDimension;
    public Collider2D[] objectCollidedWith;
    public LayerMask enemiesLayer;
    public float SpikeDamageToPlayer;

    public Character_Select_UI character_Select_UI;

    public bool isShootingArrow = false, isInDeathAnim = false;
    public int katanaTakeDamageDelayMS, archerTakeDamageDelayMS, heavyTakeDamageDelayMS, mageTakeDamageDelayMS, katanaDeathDelayMS, archerDeathDelayMS, heavyDeathDelayMS, mageDeathDelayMS;

    private void Awake()
    {
        try
        {
            playerKatana = GetComponent<Player_KatanaBehavior>().gameObject;
            playerArcher = GetComponent<Player_ArcherBehavior>().gameObject;
            playerHeavy = GetComponent<Player_HeavyBehavior>().gameObject;
            playerMage = GetComponent<Player_MageBehavior>().gameObject;
        }
        catch(Exception e)
        {
            Debug.LogError("can't get individual characters");
        }
        
        playerRB = GetComponent<Rigidbody2D>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        currentCharacter = 1;
        
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

        if(!isShootingArrow || !isInDeathAnim){
            moveInput = Input.GetAxis("Horizontal");
            PlayerJumping();
        }else if(isShootingArrow)
            moveInput = 0;
        
        
        PlayerRunning();
        SwitchingCharacter();

        if(!isKatanaAlive && !isArcherAlive && !isHeavyAlive && !isMageAlive)//when all characters have died
        {
            SceneManager.LoadScene("Stage 1");
        }
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
                jumpForce = playerKatana.GetComponent<Player_KatanaBehavior>().jumpForce;
                playerKatana.GetComponent<PlayerMovementAnimHandler>().PlayerJumping();

            }
            else if (currentCharacter == 2)
            {
                jumpForce = playerArcher.GetComponent<Player_ArcherBehavior>().jumpForce;
                playerArcher.GetComponent<PlayerMovementAnimHandler>().PlayerJumping();
            }
            else if (currentCharacter == 3)
            {
                jumpForce = playerHeavy.GetComponent<Player_HeavyBehavior>().jumpForce;
                playerHeavy.GetComponent<PlayerMovementAnimHandler>().PlayerJumping();
            }
            else if (currentCharacter == 4)
            {
                jumpForce = playerMage.GetComponent<Player_MageBehavior>().jumpForce;
                playerMage.GetComponent<PlayerMovementAnimHandler>().PlayerJumping();
            }
        }catch (Exception e){;}
        
    }

    void PlayerRunning()
    {
        if(!isInDeathAnim){
            if (currentCharacter == 1){
                //move left and right
                playerRB.velocity = new Vector2(moveInput * playerKatana.GetComponent<Player_KatanaBehavior>().speed,  playerRB.velocity.y);
                playerKatana.GetComponent<PlayerMovementAnimHandler>().PlayerRunning();   
            }
            else if (currentCharacter == 2){
                //move left and right
                playerRB.velocity = new Vector2(moveInput * playerArcher.GetComponent<Player_ArcherBehavior>().speed,  playerRB.velocity.y);
                playerArcher.GetComponent<PlayerMovementAnimHandler>().PlayerRunning();
            }else if (currentCharacter == 3){
                //move left and right
                playerRB.velocity = new Vector2(moveInput * playerHeavy.GetComponent<Player_HeavyBehavior>().speed,  playerRB.velocity.y);
                playerHeavy.GetComponent<PlayerMovementAnimHandler>().PlayerRunning();
            }else if (currentCharacter == 4){
                //move left and right
                playerRB.velocity = new Vector2(moveInput * playerMage.GetComponent<Player_MageBehavior>().speed,  playerRB.velocity.y);
                playerMage.GetComponent<PlayerMovementAnimHandler>().PlayerRunning();
            }
        }  
    }

    void SwitchingCharacter()
    {   
        if(!isInDeathAnim)
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
        }
        

        character_Select_UI.SwitchCharacter();
    }
    public void SwitchToAlive()
    {
        if(isArcherAlive == true || isKatanaAlive == true || isHeavyAlive == true || isMageAlive == true)
        {
            if(isKatanaAlive){
                currentCharacter = 1;
                ActivateKatanaOnly();
            }else if (isArcherAlive){
                currentCharacter = 2;
                ActivateArcherOnly();
            }else if (isHeavyAlive){
                currentCharacter = 3;
                ActivateHeavyOnly();
            }else if (isMageAlive){
                currentCharacter = 4;
                ActivateMageOnly();
            }
        }
        else if (isArcherAlive == false && isKatanaAlive == false && isHeavyAlive == false && isMageAlive == false){
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
        for(int i = 0; i < 10; i++)
        {
            playerRB.AddForce(this.gameObject.transform.up * 7f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.001f);
        }
    }

    public void CallDamage(float damageReceived)
    {   
        if(damageReceived!= 0)
            SpawnDamageText(damageReceived.ToString(), Color.red, 0.65f);
            
        switch (currentCharacter)
        {
            case 1:
                playerKatana.GetComponent<PlayerVitalityHandler>().TakingDamage(damageReceived);
                break;
            case 2:
                playerArcher.GetComponent<PlayerVitalityHandler>().TakingDamage(damageReceived);
                break;
            case 3:
                playerHeavy.GetComponent<PlayerVitalityHandler>().TakingDamage(damageReceived);
                break;
            case 4:
                playerMage.GetComponent<PlayerVitalityHandler>().TakingDamage(damageReceived);
                break;
        }
    }

    public void SpawnDamageText(string strText, Color color, float duration){
        GameObject text = this.gameObject.GetComponent<DamagePopUpSpawnScript>().SpawnDamagedText();
        DamagePopUpTextScript scriptText = text.GetComponent<DamagePopUpTextScript>();

        text.transform.position = this.transform.position;
        scriptText.SetText(strText);
        scriptText.colorStart = color;
        scriptText.colorEnd = color;
        scriptText.colorEnd.a = 0;
        scriptText.fadeDuration = duration;
    }
}
