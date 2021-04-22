using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class NPC_Enemy_GoblinBehavior : MonoBehaviour
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
    public LayerMask PlayerLayer;
    private Collider2D playerCollider;
    private Collider2D playerCollider2;

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
    // Start is called before the first frame update
    void Start()
    {
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

    void FollowAndAttackPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) >= attackBox.x)//Keep closing in until player in attack zone
        {
            transform.position += transform.right * runSpeed * Time.deltaTime;
            animator.SetBool("isWalking", true);
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

    public async void TakeDamage(float DamageTaken, bool canBackFlipAttack)
    {
        if(isDead == false)
        {   
            if (canBackFlipAttack == false || isFrozen == true) 
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
            else if (canBackFlipAttack == true && isFrozen == false)
            {
                isStagger = false;
                if(Time.time > elapsedTime2)
                {
                    cooldown.GetComponent<CooldownBar_GoblinAtk>().StartCoolDown();
                    AttackPlayerAnim(2);
                    elapsedTime2 = Time.time + attackInterval2Sec;
                }
                else
                {
                    TakeDamage(DamageTaken, false);
                }
                    
            }
            
        }

        if (currentHealth <= 0)
        {
            healthBar.gameObject.SetActive(false);
            cooldown.SetActive(false);
            Death();
        }   
    }

    public void AttackPlayerSurprise()//Set up as an event in the attack animation in animation
    {
        if(playerCollider2 != null)
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
            cooldown.GetComponent<CooldownBar_GoblinAtk>().Flip();
		}
		else if (transform.position.x < player.position.x && !isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = !isFlipped;
            healthBar.Flip();
            cooldown.GetComponent<CooldownBar_GoblinAtk>().Flip();
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
    }

    public void Take_Spell_Damage_Burn(float burnInterval, float burnDamage, float totalBurnTime_Loop){
        StartCoroutine(WaitAndBurn(burnInterval, burnDamage, totalBurnTime_Loop));
    }
    
    private IEnumerator WaitAndBurn(float burnInterval, float burnDamage, float totalBurnTime_Loop)
    {
        for(int i = 0; i < totalBurnTime_Loop; i++)
        {
            TakeDamage(burnDamage, false);
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
