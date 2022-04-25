using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class NPC_Enemy_MushroomBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator animator;
    
    public Transform player;
	private bool isFlipped = true;
    public float runSpeed;
    //private bool isStagger = false;

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
    private bool cycleAtkAnim = false;
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
        
        //Debug.Log(playerCollider);
        //generating attackable zone for AI to attack the player
        playerCollider = Physics2D.OverlapBox(attackZone.position, attackBox, 0f, PlayerLayer);

        if (this.gameObject.GetComponent<NPCVitalityHandler>().isStagger == false && 
        this.gameObject.GetComponent<NPCVitalityHandler>().isFrozen == false && 
        this.gameObject.GetComponent<NPCVitalityHandler>().isDead == false)
        {
            LookAtPlayer();
            FollowAndAttackPlayer();
        }
    }

    void AttackPlayerAnim()
    {
        if (playerCollider != null)
        {
            if (playerCollider.gameObject.tag == "Player")
            {
                if(cycleAtkAnim)
                {
                    animator.SetTrigger("Attack1");
                    cycleAtkAnim = !cycleAtkAnim;
                }else if (!cycleAtkAnim)
                {
                    animator.SetTrigger("Attack2");
                    cycleAtkAnim = !cycleAtkAnim;
                }
                    
            }
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
        //Debug.Log(Vector2.Distance(transform.position, player.position));
        //Debug.Log(attackBox.x);
        if (Vector2.Distance(transform.position, player.position) >= attackBox.x)//Keep closing in until player in attack zone
        {
            transform.position += transform.right * runSpeed * Time.deltaTime;
            animator.SetBool("isWalking", true);
        }else if (Vector2.Distance(transform.position, player.position) < attackBox.x)//if player is in, attack animation
        {
            if(Time.time > elapsedTime)
            {
                AttackPlayerAnim();
                elapsedTime = Time.time + attackIntervalSec;
            }
            
            animator.SetBool("isWalking", false);
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
