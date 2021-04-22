using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Player_MageBehavior : MonoBehaviour
{
    public float speed; //movement speed (left and right)
    public float jumpForce; //jump force (up)
    public Animator animator;//getting animator to set conditions for animation transitions
    private bool facingRight = true; //bool value to determine the direction the player sprite is facing
    private bool isPlayerTakingDamage = false;

    //Setting up health system
    public float currentHealth;
    public float maxHealth;
    public HealthBar healthBar;

    //Attack speed of player-archer
    public float elapsedTimeFireBall = 0;
    public float attackIntervalFireBallSec;
    public float elapsedTimeIceBall = 0;
    public float attackIntervalIceBallSec;

    //reference to parent player game obj
    private PlayerBehavior parent_PlayerBehaviorScript;
    private GameObject parent_Player;

    //shooting arrow mechanics configurations
    public GameObject fireBall_playerPrefab;
    private GameObject fireBall;
    public float directHitDamage;
    public float burnIntervalSec;
    public float totalBurnTime_Loop;
    public float burnDamage;

    public GameObject iceBall_playerPrefab;
    private GameObject iceBall;
    public float freezePeriod;

    public Transform shootingPoint;
    public int ballSpeed;
    //public float arrowDamage;

    
    public GameObject mageCoolDownBarFire;
    public GameObject mageCoolDownbarIce;

    void Start(){
        //referencing the variables
        parent_PlayerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        parent_Player = GameObject.Find("Player");
        //setup the damage of arrow from archer-player to the prefab
        fireBall_playerPrefab.GetComponent<FireBall_Player>().burnInterval = burnIntervalSec;
        fireBall_playerPrefab.GetComponent<FireBall_Player>().burnDamage = burnDamage;
        fireBall_playerPrefab.GetComponent<FireBall_Player>().directHitDamage = directHitDamage;
        fireBall_playerPrefab.GetComponent<FireBall_Player>().totalBurnTime_Loop = totalBurnTime_Loop;

        iceBall_playerPrefab.GetComponent<IceBall_Player>().freezePeriod = freezePeriod;
        //setting up current health and max health of the player
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        //Attack mechanic of player-archer
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {  
            if(Time.time > elapsedTimeFireBall)//regulate attack speed of the player
            {
                PlayAttackAnim(1);
                elapsedTimeFireBall = Time.time + attackIntervalFireBallSec;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {  
            if(Time.time > elapsedTimeIceBall)//regulate attack speed of the player
            {
                PlayAttackAnim(2);
                elapsedTimeIceBall = Time.time + attackIntervalIceBallSec;
            }
        }

    }

    public async void TakingDamage(float damageTaken)
    {
        isPlayerTakingDamage = true;
        animator.SetBool("isTakeHit", true);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        await Task.Delay(400);
        animator.SetBool("isTakeHit", false);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        isPlayerTakingDamage = false;
        

        currentHealth -= damageTaken;
        
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Death();
        }
    }
    
    public void Heals(float amount)
    {
        currentHealth += amount;
        if(currentHealth > maxHealth)
            currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
    }

    async void PlayAttackAnim(int num)
    {   
        if(num==1)
        {
            animator.SetTrigger("Attack1"); 
        }
        else if(num==2)
        {
            animator.SetTrigger("Attack2"); 
        }
            
        if(parent_PlayerBehaviorScript.isGrounded == true){
            parent_PlayerBehaviorScript.playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
            await Task.Delay(650);//so that player can't move while shooting arrow
            parent_PlayerBehaviorScript.playerRB.constraints = RigidbodyConstraints2D.None;
        }
    }

    public void Attack()//Attack funciton is linked to attack1 and attack2 event as animation event and trigger when anim is played
    {   
        fireBall = Instantiate(fireBall_playerPrefab, shootingPoint.position, Quaternion.identity);

        if(facingRight == true){
            fireBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.right * ballSpeed);     
        }else if (facingRight == false){
            fireBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.left * ballSpeed);
            fireBall.GetComponent<FireBall_Player>().Flip();
        }
    }

    public void Attack2()
    {
        iceBall = Instantiate(iceBall_playerPrefab, shootingPoint.position, Quaternion.identity);

        if(facingRight == true){
            iceBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.right * ballSpeed);     
        }else if (facingRight == false){
            iceBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.left * ballSpeed);
            iceBall.GetComponent<IceBall_Player>().Flip();
        }
    }

    async public void Death()
    {
        currentHealth = 0;
        animator.SetTrigger("Die");
        parent_PlayerBehaviorScript.isMageAlive = false;

        parent_PlayerBehaviorScript.playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        parent_Player.GetComponent<Collider2D>().enabled = false;
        await Task.Delay(2500);//finish playing death animation
        parent_PlayerBehaviorScript.playerRB.constraints = RigidbodyConstraints2D.None;
        parent_Player.GetComponent<Collider2D>().enabled = true;
        parent_PlayerBehaviorScript.AliveList[3] = false;
        parent_PlayerBehaviorScript.SwitchToAlive();

        mageCoolDownBarFire.SetActive(false);
        mageCoolDownbarIce.SetActive(false);
        

        this.enabled = false;   
    }

    public void PlayerRunning()
    {
        animator.SetFloat("Speed", Mathf.Abs(parent_PlayerBehaviorScript.moveInput));

        //Flip player facing direction based on key pressing
        if (facingRight == false && parent_PlayerBehaviorScript.moveInput > 0){
            Flip();
            healthBar.Flip();
        }else if (facingRight == true && parent_PlayerBehaviorScript.moveInput < 0){
            Flip();
            healthBar.Flip();
        }   
    }

    /*
    *   This funciton regulates anim-bool values for playing animations of player-archer
    *       - while the player is not taking damage, allow the anim-bool values to be affected by parent-player's y velocity val
    *       - when the player is taking damage, the anim-bool values are forced to 'false' to deactivate the current jumping/falling anim
    *       if the player is still airborned, allowing the "gethit" animation to be played mid-air
    */
    public void PlayerJumping()
    {   
        if (isPlayerTakingDamage == false){
            //Check vertical velocity to play falling animation
            if (parent_PlayerBehaviorScript.playerRB.velocity.y > 0){
                animator.SetBool("IsFalling", false);
            }else if (parent_PlayerBehaviorScript.playerRB.velocity.y < 0){
                animator.SetBool("IsFalling", true);
            }

            //Check to see player on ground to play jump animation
            if (parent_PlayerBehaviorScript.isGrounded == false){
                animator.SetBool("IsJumping", true);
            }else if (parent_PlayerBehaviorScript.isGrounded == true){
                animator.SetBool("IsJumping", false);
            }
        }
        else if (isPlayerTakingDamage == true){
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);
        }
    }

    //Flip function to change player-archer look direction responding to the key pressed for movement.
    void Flip(){
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
