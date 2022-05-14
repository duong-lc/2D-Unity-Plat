using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour
{
    public static PlayerBehavior Instance;

    private GameObject _playerKatana, _playerArcher, _playerHeavy, _playerMage;
    private List<GameObject> characterList = new List<GameObject>();
    
    public Rigidbody2D playerRB;
    public float moveInput;

    //Vars for checking ground contact and Jumping Mechanic
    public bool canMove = true;
    public bool isGrounded;
    public Transform groundCheck; //transform of gameobject attached to the foot of player sprite to see if it has came in contact with the ground or not
    public float checkRadius; //check radius for that gameobject
    public LayerMask GroundLayer;//telling the ground check gameobject what to check for
    private float _jumpForce;

    //public int currentCharacter;
    public CurrentCharacter currentCharacter;
    
    public bool isKatanaAlive = true, isArcherAlive = true, isHeavyAlive = true, isMageAlive = true;
    //public List<bool> AliveList = new List<bool>();

    public Transform collisionBox;
    public Vector2 colBoxDimension;
    //public Collider2D[] objectCollidedWith;
    public LayerMask enemiesLayer;
    public float SpikeDamageToPlayer;

    public Character_Select_UI character_Select_UI;

    public bool isShootingArrow = false, isInDeathAnim = false;
    //public int katanaTakeDamageDelayMS, archerTakeDamageDelayMS, heavyTakeDamageDelayMS, mageTakeDamageDelayMS, katanaDeathDelayMS, archerDeathDelayMS, heavyDeathDelayMS, mageDeathDelayMS;

    private void Awake()
    {
        Instance = this;
        
        try
        {
            _playerKatana = GetComponentInChildren<Player_KatanaBehavior>().gameObject;
            _playerArcher = GetComponentInChildren<Player_ArcherBehavior>().gameObject;
            _playerHeavy = GetComponentInChildren<Player_HeavyBehavior>().gameObject;
            _playerMage = GetComponentInChildren<Player_MageBehavior>().gameObject;
        }
        catch(Exception e)
        {
            Debug.LogError("can't get individual characters");
        }

        characterList = new List<GameObject>()
            {
                _playerKatana,
                _playerArcher,
                _playerHeavy,
                _playerMage
            };
        
        playerRB = GetComponent<Rigidbody2D>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        //currentCharacter = 1;
        currentCharacter = CurrentCharacter.Katana;
        
        //refreshing the system at start
        // ActivateKatanaOnly();
        // ActivateArcherOnly();
        // ActivateKatanaOnly();
        ActivateOnlyCharacter(CurrentCharacter.Katana);
        ActivateOnlyCharacter(CurrentCharacter.Archer);
        ActivateOnlyCharacter(CurrentCharacter.Katana);
        //show off current character from the start
        character_Select_UI.SwitchCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        //objectCollidedWith = Physics2D.OverlapBoxAll(collisionBox.position, colBoxDimension, 0f, enemiesLayer);
        playerRB.freezeRotation = true;
        //setting isGrounded to whether the overlap circle has overlapped with layer "ground"
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, GroundLayer);

        if (canMove)
        {
            if(!isShootingArrow || !isInDeathAnim){
                moveInput = Input.GetAxis("Horizontal");
                PlayerJumping();
            }else if (isShootingArrow)
                moveInput = 0;
        }
        
          

        
        
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
            playerRB.velocity = Vector2.up * _jumpForce;  
        }

        
        try{
            if (currentCharacter == CurrentCharacter.Katana)
            {
                _jumpForce = _playerKatana.GetComponent<Player_KatanaBehavior>().JumpForce;
                _playerKatana.GetComponent<PlayerMovementAnimHandler>().PlayerJumping();
            }
            else if (currentCharacter == CurrentCharacter.Archer)
            {
                _jumpForce = _playerArcher.GetComponent<Player_ArcherBehavior>().JumpForce;
                _playerArcher.GetComponent<PlayerMovementAnimHandler>().PlayerJumping();
            }
            else if (currentCharacter == CurrentCharacter.Heavy)
            {
                _jumpForce = _playerHeavy.GetComponent<Player_HeavyBehavior>().JumpForce;
                _playerHeavy.GetComponent<PlayerMovementAnimHandler>().PlayerJumping();
            }
            else if (currentCharacter == CurrentCharacter.Mage)
            {
                _jumpForce = _playerMage.GetComponent<Player_MageBehavior>().JumpForce;
                _playerMage.GetComponent<PlayerMovementAnimHandler>().PlayerJumping();
            }
        }catch (Exception e){;}
        
    }

    void PlayerRunning()
    {
        if(!isInDeathAnim)
        {
            var currentCharacter = GetCurrentCharacter();
            playerRB.velocity = new Vector2(moveInput * currentCharacter.GetComponent<PlayerBaseBehavior>().Speed,  playerRB.velocity.y);
            currentCharacter.GetComponent<PlayerMovementAnimHandler>().PlayerRunning();
            // if (currentCharacter == 1){
            //     //move left and right
            //     playerRB.velocity = new Vector2(moveInput * _playerKatana.GetComponent<Player_KatanaBehavior>().Speed,  playerRB.velocity.y);
            //     _playerKatana.GetComponent<PlayerMovementAnimHandler>().PlayerRunning();   
            // }
            // else if (currentCharacter == 2){
            //     //move left and right
            //     playerRB.velocity = new Vector2(moveInput * _playerArcher.GetComponent<Player_ArcherBehavior>().Speed,  playerRB.velocity.y);
            //     _playerArcher.GetComponent<PlayerMovementAnimHandler>().PlayerRunning();
            // }else if (currentCharacter == 3){
            //     //move left and right
            //     playerRB.velocity = new Vector2(moveInput * _playerHeavy.GetComponent<Player_HeavyBehavior>().Speed,  playerRB.velocity.y);
            //     _playerHeavy.GetComponent<PlayerMovementAnimHandler>().PlayerRunning();
            // }else if (currentCharacter == 4){
            //     //move left and right
            //     playerRB.velocity = new Vector2(moveInput * _playerMage.GetComponent<Player_MageBehavior>().Speed,  playerRB.velocity.y);
            //     _playerMage.GetComponent<PlayerMovementAnimHandler>().PlayerRunning();
            // }
        }  
    }

    void SwitchingCharacter()
    {   
        if(!isInDeathAnim)
        {
            if (isKatanaAlive == true){
                if (Input.GetKeyDown(KeyCode.Alpha1)){
                    //ActivateKatanaOnly();
                    ActivateOnlyCharacter(CurrentCharacter.Katana);
                    currentCharacter = CurrentCharacter.Katana;
                }
            }
        
            if (isArcherAlive == true){
                if (Input.GetKeyDown(KeyCode.Alpha2)){
                    //ActivateArcherOnly();
                    ActivateOnlyCharacter(CurrentCharacter.Archer);
                    currentCharacter = CurrentCharacter.Archer;
                }
            }

            if (isHeavyAlive == true){
                if (Input.GetKeyDown(KeyCode.Alpha3)){
                    //ActivateHeavyOnly();
                    ActivateOnlyCharacter(CurrentCharacter.Heavy);
                    currentCharacter = CurrentCharacter.Heavy;
                }
            }

            if(isMageAlive == true){
                if (Input.GetKeyDown(KeyCode.Alpha4)){
                    //ActivateMageOnly();
                    ActivateOnlyCharacter(CurrentCharacter.Mage);
                    currentCharacter = CurrentCharacter.Mage;
                }
            }
        }
        

        character_Select_UI.SwitchCharacter();
    }
    public void SwitchToAlive()
    {
        //if(isArcherAlive == true || isKatanaAlive == true || isHeavyAlive == true || isMageAlive == true)
        if(IsPlayerAlive())
        {
            if(isKatanaAlive){
                currentCharacter = CurrentCharacter.Katana;
                ActivateOnlyCharacter(CurrentCharacter.Katana);
                //ActivateKatanaOnly();
            }else if (isArcherAlive){
                currentCharacter = CurrentCharacter.Archer;
                ActivateOnlyCharacter(CurrentCharacter.Archer);
                //ActivateArcherOnly();
            }else if (isHeavyAlive){
                currentCharacter = CurrentCharacter.Heavy;
                ActivateOnlyCharacter(CurrentCharacter.Heavy);
                // ActivateHeavyOnly();
            }else if (isMageAlive){
                currentCharacter = CurrentCharacter.Mage;
                ActivateOnlyCharacter(CurrentCharacter.Mage);
                //ActivateMageOnly();
            }
        }
        else { //if (isArcherAlive == false && isKatanaAlive == false && isHeavyAlive == false && isMageAlive == false){
            GetComponent<Collider2D>().enabled = false;
            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
            this.enabled = false;
        }

        character_Select_UI.SwitchCharacter();
    }

    private void ActivateOnlyCharacter(CurrentCharacter character)
    {
        for (int i = 0; i < characterList.Count; i++)
        {
            characterList[i].SetActive((int) character == i);
        }
    }
    
    // private void ActivateKatanaOnly()
    // {
    //     _playerKatana.SetActive(true);
    //     _playerArcher.SetActive(false);
    //     _playerHeavy.SetActive(false);
    //     _playerMage.SetActive(false);
    // }
    // private void ActivateArcherOnly()
    // {
    //     _playerKatana.SetActive(false);
    //     _playerArcher.SetActive(true);
    //     _playerHeavy.SetActive(false);
    //     _playerMage.SetActive(false);
    // }
    // private void ActivateHeavyOnly()
    // {
    //     _playerKatana.SetActive(false);
    //     _playerArcher.SetActive(false);
    //     _playerHeavy.SetActive(true);
    //     _playerMage.SetActive(false);
    // }
    // private void ActivateMageOnly()
    // {
    //     _playerKatana.SetActive(false);
    //     _playerArcher.SetActive(false);
    //     _playerHeavy.SetActive(false);
    //     _playerMage.SetActive(true);
    // }

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
            
        GetCurrentCharacter().GetComponent<PlayerVitalityHandler>().TakingDamage(damageReceived);
        // switch (currentCharacter)
        // {
        //     case 1:
        //         _playerKatana.GetComponent<PlayerVitalityHandler>().TakingDamage(damageReceived);
        //         break;
        //     case 2:
        //         _playerArcher.GetComponent<PlayerVitalityHandler>().TakingDamage(damageReceived);
        //         break;
        //     case 3:
        //         _playerHeavy.GetComponent<PlayerVitalityHandler>().TakingDamage(damageReceived);
        //         break;
        //     case 4:
        //         _playerMage.GetComponent<PlayerVitalityHandler>().TakingDamage(damageReceived);
        //         break;
        // }
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

    public GameObject GetCurrentCharacter()
    {
        switch (currentCharacter)
        {
            case CurrentCharacter.Katana:
                return _playerKatana;
                break;
            case CurrentCharacter.Archer:
                return _playerArcher;
                break;
            case CurrentCharacter.Heavy:
                return _playerHeavy;
                break;
            case CurrentCharacter.Mage:
                return _playerMage;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return null;
    }

    private bool IsPlayerAlive()
    {
        return isArcherAlive != false || isKatanaAlive != false || isHeavyAlive != false || isMageAlive != false;
    }

    public void HaltMoveInput()
    {
        moveInput = 0;
    }

    public void RigidFreezeAll()
    { 
        playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Unfreeze()
    {
        playerRB.constraints = RigidbodyConstraints2D.None;
        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
