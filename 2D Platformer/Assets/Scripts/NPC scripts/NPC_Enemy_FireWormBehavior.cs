using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class NPC_Enemy_FireWormBehavior : NPC_Enemy_Base
{
    private Rigidbody2D rb;
    public Animator animator;

    public Transform player;
	private bool isFlipped = true;
    public float runSpeed;//0.6
    //private bool isStagger = false;

    public Transform attackZone;
    public Vector2 attackBox;
    public LayerMask PlayerLayer;
    private Collider2D playerCollider;

    //Attack speed
    public float attackIntervalSec;
    private float elapsedTime = 0;
    private float attackDamage = 25;

    private PlayerBehavior playerBehaviorScript;
    private Player_KatanaBehavior player_KatanaBehaviorScript;
    private Player_ArcherBehavior player_ArcherBehaviorScript;

    public GameObject fireBallPrefab;
    private GameObject fireBall;
    public Transform shootingPoint;
    public int fireBallSpeed;
    public float fireBallDamage;

    // private bool isDead;
    // private bool isFrozen;
    private bool keepMoving = false;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();

        fireBallPrefab.GetComponent<FireBall>().attackDamage = fireBallDamage;

        rb = this.gameObject.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;//freezing rotation

        // currentHealth = maxHealth;
        // healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        playerCollider = Physics2D.OverlapBox(attackZone.position, attackBox, 0f, PlayerLayer);

        if (this.gameObject.GetComponent<NPCVitalityHandler>().isStagger == false && this.gameObject.GetComponent<NPCVitalityHandler>().isFrozen == false && this.gameObject.GetComponent<NPCVitalityHandler>().isDead == false)
        {
            LookAtPlayer();
            FollowAndAttackPlayer();
        }

        // if (isFrozen ==true){
        //     this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        // }
    }

    public void Attack()
    {
        fireBall = Instantiate(fireBallPrefab, shootingPoint.position, Quaternion.identity);

        if(isFlipped == true){
            fireBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.right * fireBallSpeed);
        }else if (isFlipped == false){
            fireBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.left * -fireBallSpeed);
            fireBall.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    void AttackPlayerAnim()
    {
        if (playerCollider != null)
        {
            keepMoving = false;
            animator.SetTrigger("Attack");
        }
        else if (playerCollider == null)
        {
            keepMoving = true;
        }
    }

    void FollowAndAttackPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) >= attackBox.x || keepMoving == true)//Keep closing in until player in attack zone
        {
            transform.position += transform.right * runSpeed * Time.deltaTime;
            animator.SetBool("isWalking", true);
        }
        if (Vector2.Distance(transform.position, player.position) < attackBox.x)//if player is in, attack animation
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
        Gizmos.DrawWireCube(attackZone.position, attackBox);
    }

    
}
