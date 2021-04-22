using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class NPC_Enemy_FlyingEyeBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator animator;
    
    public Transform player;
	private bool isFlipped = true;
    public float runSpeed;
    private bool isStagger = false;

    public Transform attackZone;
    public Vector2 attackBox;
    public Transform attackZone2;
    public Vector2 attackBox2;
    public Transform damageZone2;
    public float damageRadius;
    public LayerMask PlayerLayer;
    private Collider2D playerCollider;
    private Collider2D playerCollider2;
    private Collider2D damageCollider2;

    //attack speed and damage
    public float attackIntervalSec;
    public float attackInterval2Sec;
    private float elapsedTime = 0;
    public float elapsedTime2 = 0;
    public float attackDamage;
    public float attackDamage2;

    //Health system and health UI
    public float currentHealth;
    public float maxHealth;
    public HealthBar healthBar;

    private PlayerBehavior playerBehaviorScript;
    private Player_KatanaBehavior player_KatanaBehaviorScript;
    private Player_ArcherBehavior player_ArcherBehaviorScript;
    private Player_HeavyBehavior player_HeavyBehaviorScript;
    private Player_MageBehavior player_MageBehaviorScript;

    private bool isDead = false;
    private bool isFrozen = false;
    public GameObject cooldown;

    //public GameObject groundCollider;

    private bool isContactWithPlayer = false;
    private bool isContactWithGround = false;

    //public GameObject groundColliderTrigger;
    /*public float colliderRadius;
    public LayerMask GroundLayer;
    private Collider2D[] groundCheck;*/
    private float currHeight;
    // Start is called before the first frame update
    void Start()
    {
        //groundCheck = Physics2D.OverlapCircleAll(groundCollider.transform.position, colliderRadius, 0f, GroundLayer);
        //Physics2D.IgnoreCollision(groundCollider.GetComponent<CircleCollider2D>(), GameObject.Find("Player").GetComponent<CapsuleCollider2D>());
        
        playerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        rb = this.gameObject.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;//freezing rotation

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        try{
            player_ArcherBehaviorScript = GameObject.Find("Player").transform.Find("Player-Archer").GetComponent<Player_ArcherBehavior>();
            player_KatanaBehaviorScript = GameObject.Find("Player").transform.Find("Player-Katana").GetComponent<Player_KatanaBehavior>();
            player_HeavyBehaviorScript = GameObject.Find("Player").transform.Find("Player-Heavy").GetComponent<Player_HeavyBehavior>();
            player_MageBehaviorScript = GameObject.Find("Player").transform.Find("Player-Mage").GetComponent<Player_MageBehavior>();
        }catch(Exception e){
            ;
        }
        

        //generating attackable zone for AI to attack the player
        playerCollider = Physics2D.OverlapBox(attackZone.position, attackBox, 0f, PlayerLayer);
        playerCollider2 = Physics2D.OverlapBox(attackZone2.position, attackBox2, 0f, PlayerLayer);
        damageCollider2 = Physics2D.OverlapCircle(damageZone2.position, damageRadius, PlayerLayer);
        //groundCheck = Physics2D.OverlapCircleAll(groundCollider.position, colliderRadius, GroundLayer);
        

        if (isStagger == false && isFrozen == false && isDead == false)
        {
            LookAtPlayer();
            FollowAndAttackPlayer();
        }

        if (isFrozen ==true){
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        }

        
    }

    void AttackPlayerAnim(int num)
    {
        if (playerCollider != null && num == 1)
        {
            if (playerCollider.gameObject.tag == "Player")
            {
                animator.SetTrigger("Attack1");
            }
        }
        else if (num == 2)
        {   
            /*if (playerCollider2.gameObject.tag == "Player")
            {*/
            animator.SetTrigger("Attack2");
            //}
        }
    }

    public void AttackPlayer()//Set up as an event in the attack animation in animation
    {
        if(playerCollider != null)
        {
            if (playerBehaviorScript.currentCharacter == 2)
            {
                player_ArcherBehaviorScript.TakingDamage(attackDamage);
            }
            else if (playerBehaviorScript.currentCharacter == 1)
            {
                player_KatanaBehaviorScript.TakingDamage(attackDamage);
            }
            else if(playerBehaviorScript.currentCharacter == 3)
            {
                player_HeavyBehaviorScript.TakingDamage(attackDamage);
            }
            else if(playerBehaviorScript.currentCharacter == 4)
            {
                player_MageBehaviorScript.TakingDamage(attackDamage);
            }
        }
    }

    public void AttackPlayerSurprise()//Set up as an event in the attack animation in animation
    {
        Debug.Log(damageCollider2.gameObject);
        if(damageCollider2 != null)
        {
            if (playerBehaviorScript.currentCharacter == 2)
            {
                player_ArcherBehaviorScript.TakingDamage(attackDamage2);
            }
            else if (playerBehaviorScript.currentCharacter == 1)
            {
                player_KatanaBehaviorScript.TakingDamage(attackDamage2);
            }
            else if(playerBehaviorScript.currentCharacter == 3)
            {
                player_HeavyBehaviorScript.TakingDamage(attackDamage2);
            }
            else if(playerBehaviorScript.currentCharacter == 4)
            {
                player_MageBehaviorScript.TakingDamage(attackDamage2);
            }
        }  
    }
    
    async public void LockOnPlayerPosition()
    {
        //groundCollider.GetComponent<CircleCollider2D>().enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        currHeight = this.gameObject.transform.position.y;
        StartCoroutine(TranslateToPlayer());
        await Task.Delay((int)(attackInterval2Sec*1000 - 1000));
        TranslateBack();
    }

    private void TranslateBack()
    {
        //groundColliderTrigger.SetActive(true);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        //groundCollider.GetComponent<CircleCollider2D>().enabled = true;
    }

    private IEnumerator TranslateToPlayer()
    {
        cooldown.GetComponent<CooldownBar_FlyingEyeAtk>().StartCoolDown();
        //groundColliderTrigger.SetActive(false);
        Transform playerPos = player;
        Vector2 difference = playerPos.position - this.gameObject.transform.position;
        float dist = Mathf.Sqrt(difference.x*difference.x + difference.y*difference.y);

        float temp = dist/70;
        for(int i = 0; i < 20; i++)
        {
            if(isContactWithGround == true)
            {
                isContactWithGround = false;
                break;
            }

            if(isContactWithPlayer == true)
            {
                AttackPlayerSurprise(); 
                break;
            }

            this.gameObject.transform.Translate(difference/10,Space.World);
            temp+=temp;

            if(temp >= dist)
                break;

            yield return new WaitForSeconds(0.05f);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            isContactWithPlayer = true;
        }
        if(other.gameObject.tag == "ground")
        {
            isContactWithGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            isContactWithPlayer = false;
        }
        if(other.gameObject.tag == "ground")
        {
            isContactWithGround = false;
        }
    }

    void FollowAndAttackPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) >= attackBox.x)//Keep closing in until player in attack zone
        {
            transform.position += transform.right * runSpeed * Time.deltaTime;
            animator.SetBool("isWalking", true);

            if (Time.time > elapsedTime2)
            {  
                if(playerCollider2 != null)
                {
                    AttackPlayerAnim(2);
                    elapsedTime2 = Time.time + attackInterval2Sec;
                }
                animator.SetBool("isWalking", false);
            }

        }else if (Vector2.Distance(transform.position, player.position) < attackBox.x)//if player is in, attack animation
        {
            if(Time.time > elapsedTime)
            {
                AttackPlayerAnim(1);
                elapsedTime = Time.time + attackIntervalSec;
            }
            
            animator.SetBool("isWalking", false);
        }   
    }

    public async void TakeDamage(float DamageTaken)
    {
        if(isDead == false)
        {
            isStagger = true;
            currentHealth -= DamageTaken;
            animator.SetBool("isWalking", false);
            animator.SetTrigger("TakeHit");

            if(isFrozen == false)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                await Task.Delay(300);
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else if (isFrozen == true)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
            }
            

            healthBar.SetHealth(currentHealth);

            await Task.Delay(500);
            isStagger = false; 
        }
        
        if (currentHealth <= 0)
        {
            healthBar.gameObject.SetActive(false);
            Death();
        }
            
        

        if (currentHealth <= 0)
        {
            healthBar.gameObject.SetActive(false);
            cooldown.SetActive(false);
            Death();
        }   
    }

    void Death()
    {
        isDead = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isDead", true);
        //Debug.Log("dead");

        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        GetComponent<Collider2D>().enabled = false;
        
        isStagger = true;
        Destroy(this.gameObject, 5f);
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
            healthBar.Flip();
            cooldown.GetComponent<CooldownBar_FlyingEyeAtk>().Flip();
		}
		else if (transform.position.x < player.position.x && !isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = !isFlipped;
            healthBar.Flip();
            cooldown.GetComponent<CooldownBar_FlyingEyeAtk>().Flip();
		}
	}

    void OnDrawGizmosSelected()
    {    
        if (attackZone == null)
        {
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(attackZone.position, attackBox);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackZone2.position, attackBox2);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(damageZone2.position, damageRadius);
    }

    public void Take_Spell_Damage_Burn(float burnInterval, float burnDamage, float totalBurnTime_Loop){
        StartCoroutine(WaitAndBurn(burnInterval, burnDamage, totalBurnTime_Loop));
    }
    
    private IEnumerator WaitAndBurn(float burnInterval, float burnDamage, float totalBurnTime_Loop)
    {
        for(int i = 0; i < totalBurnTime_Loop; i++)
        {
            TakeDamage(burnDamage);
            yield return new WaitForSeconds(burnInterval);
        }
        
    }

    async public void Take_Spell_Frozen(float freezePeriod)
    {
        //StartCoroutine(WaitAndFreeze(freezePeriod));    

        isStagger = true;
        isFrozen = true;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        animator.speed = 0.001f;

        await Task.Delay((int)(freezePeriod*1000f));

        isStagger = false;
        isFrozen = false;
        animator.speed = 1f;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

}
