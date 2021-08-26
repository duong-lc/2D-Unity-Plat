using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
public class BossScript : MonoBehaviour
{
	[Serializable]
	public enum avoidState{
		block, roll
	}
	[Serializable]
	public enum currentAttackState{
		attack1, attack2, attack3, attackSpecial
	}

    public float runSpeed;
    public Transform currentTarget;
	[SerializeField] private float jumpForce;
	private Animator animator;
	private bool isFlipped = true;
	private Rigidbody2D rb;
	[SerializeField] private Transform transformGround;
	[SerializeField] private float radiusGroundCheck;
	[SerializeField] private LayerMask groundLayer;
	private Collider2D groundCollider;
	private bool isGrounded;
	[SerializeField] private Transform avoidAnchor, attackZone, attackZone2, attackZone3, attackZoneSpecial;
	[SerializeField] private Vector2 dodgingDetection, attackBox, attackBox2, attackBox3, attackBoxSpecial;
    public LayerMask PlayerLayer;
    private Collider2D hitCollider, hitCollider2, hitCollider3, hitColliderSpecial;

	public currentAttackState attackState;

	[SerializeField] private Transform playerPos;
	private PlayerBehavior playerParentScript;

	public float avoidCooldown;
	public float avoidCooldownElapsedTime;
	//attack speed and damage
    public float attackIntervalSec;
    private float elapsedTime = 0;
    public float damage1, damage2, damage3, damageSpecial;

	public GameObject cooldown;
	public avoidState dodgeState = avoidState.roll;

	private float currentHealth, maxHealth;

	public GameObject bossStats;
	public int rageCounter, maxRageCounter;
	public float rageDurationSec;
	public bool isRage = false;
	public float rageModeElapsedTime = 0;

	public float time;
	private bool hasReleased, isPlayerLocked = false;
	public GameObject PortalSpawner;
    // Start is called before the first frame update
    void Start()
    {
		rageCounter = 0;
		playerParentScript = playerPos.gameObject.GetComponent<PlayerBehavior>();
		rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();

		attackState = currentAttackState.attack1;
    }

    // Update is called once per frame
    void Update()
    {
		time = Time.time;
		//UpdateCurrentAttackState();

		groundCollider = Physics2D.OverlapCircle(transformGround.position, radiusGroundCheck, groundLayer);
		if(groundCollider != null){
			isGrounded = true;
		}else{
			isGrounded = false;
		}


		if(attackState == currentAttackState.attack1)
			hitCollider = Physics2D.OverlapBox(attackZone.position, attackBox, 0f, PlayerLayer);
		else if (attackState == currentAttackState.attack2)
			hitCollider2 = Physics2D.OverlapBox(attackZone2.position, attackBox2, 0f, PlayerLayer);
		else if (attackState == currentAttackState.attack3)
			hitCollider3 = Physics2D.OverlapBox(attackZone3.position, attackBox3, 0f, PlayerLayer);

		if(isRage)
			hitColliderSpecial = Physics2D.OverlapBox(attackZoneSpecial.position, attackBoxSpecial, 0f, PlayerLayer);
		

		if(rageCounter >= maxRageCounter){
			rageModeElapsedTime = rageDurationSec + Time.time;
			rageCounter = 0;
		}


		if(rageModeElapsedTime > Time.time){
			isRage = true;
			rageCounter = 0;
		}else{
			isRage = false;
			if(hasReleased == true)
				hasReleased = false;
			if(isPlayerLocked == true)
				ReleasePosition();
		}
		

		if (this.gameObject.GetComponent<NPCVitalityHandler>().isStagger == false 
        && this.gameObject.GetComponent<NPCVitalityHandler>().isFrozen == false 
        && this.gameObject.GetComponent<NPCVitalityHandler>().isDead == false){
			LookAtPlayer();
			if(isRage)
				FollowAndAttackPlayer(runSpeed*2f);
			else
				FollowAndAttackPlayer(runSpeed);

			
			CheckJumpAnimation();
		}	
		//if(Input.GetKeyDown(KeyCode.N))
    }


	private void UpdateCurrentAttackState(){
		//Debug.Log("curr:" + currentHealth + "max: " + maxHealth);

		if(isRage && hasReleased == false){
			attackState = currentAttackState.attackSpecial;
		}else{
			currentHealth = gameObject.GetComponent<NPCVitalityHandler>().currentHealth;
			maxHealth = gameObject.GetComponent<NPCVitalityHandler>().maxHealth;

			if(currentHealth <= maxHealth * 0.5){
				attackState = currentAttackState.attack3;
			}else if (currentHealth <= maxHealth * 0.75){
				attackState = currentAttackState.attack2;
			}else{
				attackState = currentAttackState.attack1;
			}
		}

		

	}

	private void CheckJumpAnimation(){
		if (this.gameObject.GetComponent<NPCVitalityHandler>().isStagger == false){
			//Debug.Log(isGrounded);
            //Check vertical velocity to play falling animation
            if (rb.velocity.y > 0 && isGrounded == false){
				animator.SetBool("isJumping", true);
                animator.SetBool("isFalling", false);
            }else if (rb.velocity.y < -0.01 && isGrounded == false){
				animator.SetBool("isJumping", true);
                animator.SetBool("isFalling", true);
            }else if (rb.velocity.y >= -0.01 || isGrounded == true){
				animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", false);
			}

        }
        else if (this.gameObject.GetComponent<NPCVitalityHandler>().isStagger == true){
            animator.SetBool("isFalling", false);
            animator.SetBool("isJumping", false);
        }
	}

	public void Jump(){
		if(gameObject.GetComponent<NPCVitalityHandler>().isFrozen == false)
			gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpForce;  
	}

    void LookAtPlayer()
	{
		Vector3 flipped = transform.localScale;
		flipped.z *= -1f;

		if (transform.position.x > playerPos.position.x && isFlipped || Mathf.Abs(transform.position.x - playerPos.position.x) <= 0.1)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = !isFlipped;
            this.gameObject.GetComponent<NPCVitalityHandler>().healthBar.Flip();
			cooldown.GetComponent<CooldownBar_NPC>().Flip();
		}
		else if (transform.position.x < playerPos.position.x && !isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = !isFlipped;
            this.gameObject.GetComponent<NPCVitalityHandler>().healthBar.Flip();
			cooldown.GetComponent<CooldownBar_NPC>().Flip();
		}
	}

	void FollowAndAttackPlayer(float speed){

		if(Vector2.Distance(transform.position, playerPos.position) < attackBox.x){
			
			if(Time.time > elapsedTime)
            {
                AttackPlayerAnim();
                elapsedTime = Time.time + attackIntervalSec;
            }
			
			
			animator.SetBool("isWalking", false);
			dodgeState = avoidState.block;
		}
		else if (Vector2.Distance(transform.position, currentTarget.position) >= 0.5)//Keep closing in until player in attack zone
        {
            transform.position += transform.right * speed * Time.deltaTime;
            animator.SetBool("isWalking", true);
			dodgeState = avoidState.roll;
        }
		
	}

	private void AttackPlayerAnim(){
		UpdateCurrentAttackState();
	
		switch(attackState){
			case currentAttackState.attack1:
				if(hitCollider != null){
					animator.SetTrigger("Attack1");
				}
				break;
			case currentAttackState.attack2:
				if(hitCollider2 != null){
					animator.SetTrigger("Attack2");
				}
				break;
			case currentAttackState.attack3:
				if(hitCollider3 != null){
					animator.SetTrigger("Attack3");
				}
				break;
			case currentAttackState.attackSpecial:
				if(hitColliderSpecial != null && hasReleased ==false){
					//Debug.Log("isRage = " + isRage);
					animator.SetTrigger("AttackSpecial");
				}
				break;
		}
	}
	
	/*
	The attack calling functions are called as events in the animation when played
	*/
	public void Attack1(){
		if(hitCollider != null)
        {
            playerParentScript.CallDamage(damage1);
        }
	}

	public void Attack2(){
		if(hitCollider2 != null){
			playerParentScript.CallDamage(damage2);
		}
	}

	public void Attack3(){
		if(hitCollider3 != null){
			playerParentScript.CallDamage(damage3);
		}
	}

	public void AttackSpecial(){
		if(hitColliderSpecial != null){
			playerParentScript.CallDamage(damageSpecial);
		}
	}

	//Locking player's position when performing special attack, called as event in animation
	public void LockPosition(){
		isPlayerLocked = true;
		//hasSpecialed = true;
		//gameObject.transform.position.Set(playerPos.position.x, playerPos.position.y, playerPos.position.z);
		//playerPos.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;	
	}

	public void ReleasePosition(){
		isPlayerLocked = false;
		//playerPos.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		//playerPos.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

		rageModeElapsedTime = Time.time;
		UpdateCurrentAttackState();
		hasReleased = true;
		isRage = false;
		Debug.Log("Released");
	}

	void OnDrawGizmosSelected()
    {    
        Gizmos.color = Color.red;
        Gizmos.DrawRay(avoidAnchor.position, dodgingDetection);
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(attackZone.position, attackBox);
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(attackZone2.position, attackBox2);
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireCube(attackZone3.position, attackBox3);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(attackZoneSpecial.position, attackBoxSpecial);
		Gizmos.color = Color.grey;
		Gizmos.DrawWireSphere(transformGround.position, radiusGroundCheck);
    }
}
