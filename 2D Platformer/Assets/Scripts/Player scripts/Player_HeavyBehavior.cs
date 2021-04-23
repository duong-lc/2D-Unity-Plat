using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Player_HeavyBehavior : MonoBehaviour
{
    public float speed; //movement speed (left and right)
    public float jumpForce; //jump force (up)
    public Animator animator;//getting animator to set conditions for animation transitions

    private bool facingRight = true; //bool value to determine the direction the player sprite is facing

    private int count = 0;
    private bool isPlayerTakingDamage = false;

    //Vars for checking attack zone
    public Transform attackZone1;
    public Vector2 attackBox1;
    public Transform attackZone2;
    public Vector2 attackBox2;
    public Transform attackZone3;
    public Vector2 attackBox3;
    public LayerMask EnemiesLayer;
    private Collider2D[] hitArray1;
    private Collider2D[] hitArray2;
    private Collider2D[] hitArray3;
    public float attackDamage1;
    public float attackDamage2;
    public float attackDamage3;

    public float currentHealth;
    public float maxHealth;
    public HealthBar healthBar;

    public float elapsedTime = 0;
    public float attackIntervalSec;
    //public bool actuallyAttack = false;

    private PlayerBehavior parent_PlayerBehaviorScript;
    private GameObject parent_Player;

    public GameObject heavyCoolDownBar;
    void Start(){
        parent_PlayerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        parent_Player = GameObject.Find("Player");

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }


    void Update(){
        hitArray1 = Physics2D.OverlapBoxAll(attackZone1.position, attackBox1, 0f, EnemiesLayer);
        hitArray2 = Physics2D.OverlapBoxAll(attackZone2.position, attackBox2, 0f, EnemiesLayer);
        hitArray3 = Physics2D.OverlapBoxAll(attackZone3.position, attackBox3, 0f, EnemiesLayer);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {  
            if(Time.time > elapsedTime)
            {
                PlayAttackAnim();
                elapsedTime = Time.time + attackIntervalSec;
            }
        }
    }

    public async void TakingDamage(float damageTaken)
    {
        
        isPlayerTakingDamage = true;
        animator.SetBool("isTakeHit", true);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        await Task.Delay(300);
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
    
    void PlayAttackAnim()
    {    
        animator.SetTrigger("Attack1");
        animator.SetTrigger("Attack2");
        animator.SetTrigger("Attack3");
    }

    public void Attack1()//Attack funciton is linked to attack1 and attack2 event as animation event and trigger when anim is played
    {   
        foreach(Collider2D enemy in hitArray1)
        {
            if(enemy.gameObject.tag == "Enemy-Skeleton")
                enemy.GetComponent<NPC_EnemyBehavior>().TakeDamage(attackDamage1, false);
            else if(enemy.gameObject.tag == "Enemy-FireWorm")
                enemy.GetComponent<NPC_Enemy_FireWormBehavior>().TakeDamage(attackDamage1);
            else if(enemy.gameObject.tag == "Enemy-Goblin")
                enemy.GetComponent<NPC_Enemy_GoblinBehavior>().TakeDamage(attackDamage1, true);
            else if (enemy.gameObject.tag == "Enemy-Slime")
                enemy.GetComponent<NPC_Enemy_SlimeBehavior>().TakeDamage(attackDamage1);
            else if (enemy.gameObject.tag == "Enemy-FlyingEye")
                enemy.GetComponent<NPC_Enemy_FlyingEyeBehavior>().TakeDamage(attackDamage1);
            else if (enemy.gameObject.tag == "Enemy-Mushroom")
                enemy.GetComponent<NPC_Enemy_MushroomBehavior>().TakeDamage(attackDamage1);
            else if (enemy.gameObject.tag == "Enemy-Demon")
                enemy.GetComponent<NPC_Enemy_DemonBehavior>().TakeDamage(attackDamage1);

            KnockBackEnemy(enemy, 1f);
        }
    }

    public void Attack2()//Attack funciton is linked to attack1 and attack2 event as animation event and trigger when anim is played
    {   
        foreach(Collider2D enemy in hitArray2)
        {
            if(enemy.gameObject.tag == "Enemy-Skeleton")
                enemy.GetComponent<NPC_EnemyBehavior>().TakeDamage(attackDamage2, false);
            else if(enemy.gameObject.tag == "Enemy-FireWorm")
                enemy.GetComponent<NPC_Enemy_FireWormBehavior>().TakeDamage(attackDamage2);
            else if(enemy.gameObject.tag == "Enemy-Goblin")
                enemy.GetComponent<NPC_Enemy_GoblinBehavior>().TakeDamage(attackDamage2, true);
            else if (enemy.gameObject.tag == "Enemy-Slime")
                enemy.GetComponent<NPC_Enemy_SlimeBehavior>().TakeDamage(attackDamage2);
            else if (enemy.gameObject.tag == "Enemy-FlyingEye")
                enemy.GetComponent<NPC_Enemy_FlyingEyeBehavior>().TakeDamage(attackDamage2);
            else if (enemy.gameObject.tag == "Enemy-Mushroom")
                enemy.GetComponent<NPC_Enemy_MushroomBehavior>().TakeDamage(attackDamage2);
            else if (enemy.gameObject.tag == "Enemy-Demon")
                enemy.GetComponent<NPC_Enemy_DemonBehavior>().TakeDamage(attackDamage2);

            KnockBackEnemy(enemy, 1.3f);
        }
    }

    public void Attack3()//Attack funciton is linked to attack1 and attack2 event as animation event and trigger when anim is played
    {   
        foreach(Collider2D enemy in hitArray3)
        {
            if(enemy.gameObject.tag == "Enemy-Skeleton")
                enemy.GetComponent<NPC_EnemyBehavior>().TakeDamage(attackDamage3, false);
            else if(enemy.gameObject.tag == "Enemy-FireWorm")
                enemy.GetComponent<NPC_Enemy_FireWormBehavior>().TakeDamage(attackDamage3);
            else if(enemy.gameObject.tag == "Enemy-Goblin")
                enemy.GetComponent<NPC_Enemy_GoblinBehavior>().TakeDamage(attackDamage3, true);
            else if (enemy.gameObject.tag == "Enemy-Slime")
                enemy.GetComponent<NPC_Enemy_SlimeBehavior>().TakeDamage(attackDamage3);
            else if (enemy.gameObject.tag == "Enemy-FlyingEye")
                enemy.GetComponent<NPC_Enemy_FlyingEyeBehavior>().TakeDamage(attackDamage3);
            else if (enemy.gameObject.tag == "Enemy-Mushroom")
                enemy.GetComponent<NPC_Enemy_MushroomBehavior>().TakeDamage(attackDamage3);
            else if (enemy.gameObject.tag == "Enemy-Demon")
                enemy.GetComponent<NPC_Enemy_DemonBehavior>().TakeDamage(attackDamage3);
 
            KnockBackEnemy(enemy, 1.5f);
            
        }
    }

    async private void KnockBackEnemy(Collider2D enemy, float multiplier){
        Vector2 difference = enemy.transform.position - parent_Player.transform.position;
        if(enemy.gameObject.tag == "Enemy-FlyingEye")
            enemy.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.5f;
        
            
        enemy.gameObject.GetComponent<Rigidbody2D>().AddForce(70f * multiplier * difference.normalized, ForceMode2D.Impulse);
        enemy.gameObject.GetComponent<Rigidbody2D>().AddForce(enemy.transform.up * 80f * multiplier, ForceMode2D.Impulse);
    
        

        await Task.Delay(1000);
        if(enemy.gameObject.tag == "Enemy-FlyingEye")
            enemy.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        

    }

    async public void Death()
    {
        currentHealth = 0;
        animator.SetTrigger("Die");
        parent_PlayerBehaviorScript.isHeavyAlive = false;

        parent_PlayerBehaviorScript.playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        parent_Player.GetComponent<Collider2D>().enabled = false;
        await Task.Delay(2500);//finish playing death animation
        parent_PlayerBehaviorScript.playerRB.constraints = RigidbodyConstraints2D.None;
        parent_Player.GetComponent<Collider2D>().enabled = true;
        parent_PlayerBehaviorScript.AliveList[2] = false;
        parent_PlayerBehaviorScript.SwitchToAlive();
        
        heavyCoolDownBar.SetActive(false);
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

    void Flip(){
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void OnDrawGizmosSelected()
    {    
        if (attackZone1 == null || attackZone2 == null || attackZone3 == null)
        {
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(attackZone1.position, attackBox1);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackZone2.position, attackBox2);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackZone3.position, attackBox3);
    }

    
}
