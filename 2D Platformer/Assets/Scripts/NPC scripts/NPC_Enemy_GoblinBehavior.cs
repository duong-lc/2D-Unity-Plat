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
    //private bool isStagger = false;

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
    // public float currentHealth;
    // public float maxHealth;
    // public HealthBar healthBar;

    private PlayerBehavior playerBehaviorScript;

    // private bool isDead = false;
    // private bool isFrozen = false;
    public GameObject cooldown;
    // Start is called before the first frame update
    void Awake()
    {
        playerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        rb = this.gameObject.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;//freezing rotation

        // currentHealth = maxHealth;
        // healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //generating attackable zone for AI to attack the player
        playerCollider = Physics2D.OverlapBox(attackZone.position, attackBox, 0f, PlayerLayer);
        playerCollider2 = Physics2D.OverlapBox(attackZone2.position, attackBox2, 0f, PlayerLayer);

        if (this.gameObject.GetComponent<NPCVitalityHandler>().isStagger == false && 
        this.gameObject.GetComponent<NPCVitalityHandler>().isFrozen == false && 
        this.gameObject.GetComponent<NPCVitalityHandler>().isDead == false)
        {
            LookAtPlayer();
            FollowAndAttackPlayer();
        }
    }

    public void AttackPlayerAnim(int num)
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
            playerBehaviorScript.CallDamage(attackDamage);
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

  
    public void AttackPlayerSurprise()//Set up as an event in the attack animation in animation
    {
        if(playerCollider2 != null)
        {
            playerBehaviorScript.CallDamage(attackDamage2);
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
            cooldown.GetComponent<CooldownBar_NPC>().Flip();
		}
		else if (transform.position.x < player.position.x && !isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = !isFlipped;
            this.gameObject.GetComponent<NPCVitalityHandler>().healthBar.Flip();
            cooldown.GetComponent<CooldownBar_NPC>().Flip();
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
    

}
