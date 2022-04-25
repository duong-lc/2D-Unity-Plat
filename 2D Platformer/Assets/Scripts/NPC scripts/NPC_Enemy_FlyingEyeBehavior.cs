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
    //private bool isStagger = false;

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
    // public float currentHealth;
    // public float maxHealth;
    // public HealthBar healthBar;

    private PlayerBehavior playerBehaviorScript;

    //private bool isDead = false;
    //private bool isFrozen = false;
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
    void Awake()
    {
        //groundCheck = Physics2D.OverlapCircleAll(groundCollider.transform.position, colliderRadius, 0f, GroundLayer);
        //Physics2D.IgnoreCollision(groundCollider.GetComponent<CircleCollider2D>(), GameObject.Find("Player").GetComponent<CapsuleCollider2D>());
        
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
        damageCollider2 = Physics2D.OverlapCircle(damageZone2.position, damageRadius, PlayerLayer);
        //groundCheck = Physics2D.OverlapCircleAll(groundCollider.position, colliderRadius, GroundLayer);
        

        if (this.gameObject.GetComponent<NPCVitalityHandler>().isStagger == false && 
        this.gameObject.GetComponent<NPCVitalityHandler>().isFrozen == false && 
        this.gameObject.GetComponent<NPCVitalityHandler>().isDead == false)
        {
            LookAtPlayer();
            FollowAndAttackPlayer();
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
        if(playerCollider != null){
            playerBehaviorScript.CallDamage(attackDamage);
        }
    }

    public void AttackPlayerSurprise()//Set up as an event in the attack animation in animation
    {
        //Debug.Log(damageCollider2.gameObject);
        if(damageCollider2 != null){
            playerBehaviorScript.CallDamage(attackDamage2);
        }  
    }
    
    public async void LockOnPlayerPosition()
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
        try{
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        catch (Exception e)
        {
            ;
        }
        
        //groundCollider.GetComponent<CircleCollider2D>().enabled = true;
    }

    private IEnumerator TranslateToPlayer()
    {
        cooldown.GetComponent<CooldownBar_NPC>().StartCoolDown();
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(damageZone2.position, damageRadius);
    }

   

}
