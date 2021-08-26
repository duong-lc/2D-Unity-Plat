using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class NPC_Enemy_DemonBehavior : MonoBehaviour
{   
    private Rigidbody2D rb;
    public Animator animator;
    
    public Transform player;
	private bool isFlipped = true;
    public float runSpeed;
   // private bool isStagger = false;

    public Transform attackZone;
    public Vector2 attackBox;
    public LayerMask PlayerLayer;
    private Collider2D playerCollider;

    //attack speed and damage
    public float attackIntervalSec;
    private float elapsedTime = 0;
    public float attackDamage;

    //Health system and health UI
    // public float currentHealth;
    // public float maxHealth;
    // public HealthBar healthBar;

    private PlayerBehavior playerBehaviorScript;

    // private bool isDead = false;
    // private bool isFrozen = false;

    public GameObject Fire_GameObj;
    public Animator Fire_Animator;

    public bool isRedTint = false, isCyanTint = false;


    void Awake()
    {
        //groundCheck = Physics2D.OverlapCircleAll(groundCollider.transform.position, colliderRadius, 0f, GroundLayer);
        //Physics2D.IgnoreCollision(groundCollider.GetComponent<CircleCollider2D>(), GameObject.Find("Player").GetComponent<CapsuleCollider2D>());
        
        playerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        rb = this.gameObject.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;//freezing rotation

        // currentHealth = maxHealth;
        // healthBar.SetMaxHealth(maxHealth);
    }

    private void LateUpdate() {
        //this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        if(isRedTint)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        if(isCyanTint)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        if(!isCyanTint && !isRedTint)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    private void Update() 
    {
        playerCollider = Physics2D.OverlapBox(attackZone.position, attackBox, 0f, PlayerLayer);
        
        if (this.gameObject.GetComponent<NPCVitalityHandler>().isStagger == false && 
        this.gameObject.GetComponent<NPCVitalityHandler>().isFrozen == false && 
        this.gameObject.GetComponent<NPCVitalityHandler>().isDead == false)
        {
            LookAtPlayer();
            FollowAndAttackPlayer();
        }
    }

    void FollowAndAttackPlayer()
    {
        //player height +5
        Vector2 anchor = new Vector2(player.position.x, player.position.y + 3);
        if(Vector2.Distance(anchor, transform.position) >= 0)
        {
            if(transform.position.y > anchor.y)
                transform.position += -transform.up * runSpeed * Time.deltaTime/3;
            if(transform.position.y < anchor.y)
                transform.position -= -transform.up * runSpeed * Time.deltaTime/3;
        }



        if (Vector2.Distance(transform.position, player.position) >= attackBox.x)//Keep closing in until player in attack zone
        {
            transform.position += transform.right * runSpeed * Time.deltaTime;
            animator.SetTrigger("isIdle");
        }else if (Vector2.Distance(transform.position, player.position) < attackBox.x)//if player is in, attack animation
        {
            if(Time.time > elapsedTime)
            {
                AttackPlayerAnim();
                elapsedTime = Time.time + attackIntervalSec;
            }
            
            animator.SetTrigger("isIdle");
        }   
    }

    void AttackPlayerAnim()
    {
        if (playerCollider != null)
        {
            if (playerCollider.gameObject.tag == "Player")
            {
                animator.SetTrigger("isAttack");
            }
        }
        
    }

    void ShootOutFire()
    {
        Fire_GameObj.SetActive(true);
        Fire_Animator.Play("Demon-Fire", 0, 0f);
    }

    void StopFire()
    {
        Fire_GameObj.SetActive(false);
        //Fire_Animator.Play("Demon-Fire", 0, 0f);
    }

    public void AttackPlayer()//Set up as an event in the attack animation in animation
    {
        if(playerCollider != null)
        {
            playerBehaviorScript.CallDamage(attackDamage);
        }
    }

    void LookAtPlayer()
	{
		Vector3 flipped = transform.localScale;
		flipped.z *= -1f;

		if (transform.position.x > player.position.x && isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = !isFlipped;
            this.gameObject.GetComponent<NPCVitalityHandler>().healthBar.Flip();
		}
		else if (transform.position.x < player.position.x && !isFlipped)
		{   
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = !isFlipped;
            this.gameObject.GetComponent<NPCVitalityHandler>().healthBar.Flip();
		}
	}

    // public async void TakeDamage(float DamageTaken)
    // {
    //     if(isDead == false)
    //     {
    //         //isStagger = true;
    //         currentHealth -= DamageTaken;
    //         //animator.SetBool("isWalking", false);
    //         //maybe a set trigger reset here
    //         //animator.SetTrigger("TakeHit");
    //         if(isFrozen == false)//For some reason the demon does not change to red tint then back to red, Debugging say
    //         //the sprite is red then white after 0.3s but no changes were made on the actual sprite.
    //         {
    //             //this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    //             isRedTint = true;
    //             await Task.Delay(300);
    //             //this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    //             isRedTint = false;

    //         }
    //         else if (isFrozen == true)
    //         {
    //             //this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
    //             isCyanTint = true;
    //         }
            

    //         healthBar.SetHealth(currentHealth);

    //         //await Task.Delay(500);
    //         //isStagger = false; 
    //     }
        
    //     if (currentHealth <= 0)
    //     {
    //         healthBar.gameObject.SetActive(false);
    //         Death();
    //     }
    // }

    // void Death()
    // {
    //     isDead = true;

    //     animator.SetTrigger("Death");

    //     rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    //     GetComponent<Collider2D>().enabled = false;
        
    //     isStagger = true;
    //     Destroy(this.gameObject, 5f);
    // }

    // public void Take_Spell_Damage_Burn(float burnInterval, float burnDamage, float totalBurnTime_Loop){
    //     StartCoroutine(WaitAndBurn(burnInterval, burnDamage, totalBurnTime_Loop));
    // }
    
    // private IEnumerator WaitAndBurn(float burnInterval, float burnDamage, float totalBurnTime_Loop)
    // {
    //     for(int i = 0; i < totalBurnTime_Loop; i++)
    //     {
    //         TakeDamage(burnDamage);
    //         yield return new WaitForSeconds(burnInterval);
    //     }
        
    // }

    // async public void Take_Spell_Frozen(float freezePeriod)
    // {
    //     //StartCoroutine(WaitAndFreeze(freezePeriod));    

    //     isStagger = true;
    //     isFrozen = true;
    //     //this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
    //     isCyanTint = true;
    //     animator.speed = 0.001f;

    //     await Task.Delay((int)(freezePeriod*1000f));

    //     isStagger = false;
    //     isFrozen = false;
    //     animator.speed = 1f;
    //     //this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    //     isCyanTint = false;
    // }

    void OnDrawGizmosSelected()
    {    
        if (attackZone == null)
        {
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(attackZone.position, attackBox);
    }
}
