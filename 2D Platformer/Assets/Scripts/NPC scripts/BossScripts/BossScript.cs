using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
public class BossScript : NPC_Enemy_Base
{
	[Serializable]
	public enum avoidState{
		block, roll
	}
	[Serializable]
	public enum currentAttackState{
		attack1, attack2, attack3, attackSpecial
	}

    // public float runSpeed;//4.5
    public Transform currentTarget;
	[SerializeField] private float jumpForce;//9
	// private Animator animator;
	// private bool isFlipped = true;
	// private Rigidbody2D rb;
	[SerializeField] private Transform transformGround;
	[SerializeField] private float radiusGroundCheck;//0.1
	[SerializeField] private LayerMask groundLayer;
	private Collider2D _groundCollider;
	private bool _isGrounded;

	[SerializeField] private Transform avoidAnchor;
		//, attackZone, attackZone2, attackZone3, attackZoneSpecial;
	//dodging detection 3.5 - 0
	//attack box 2 -1
	//attack box2 2 - 2
	//attack box3 5 - 2
	//attack box special 3.5 - 3
	[SerializeField] private Vector2 dodgingDetection;
		//, attackBox, attackBox2, attackBox3, attackBoxSpecial;
    // public LayerMask PlayerLayer;
    //private Collider2D hitCollider, hitCollider2, hitCollider3, hitColliderSpecial;

	public currentAttackState attackState;

	//[SerializeField] private Transform playerPos;
	//private PlayerBehavior playerParentScript;

	//public float avoidCooldown;//2
	//public float avoidCooldownElapsedTime;
	//attack speed and damage
    public float attackIntervalSec;//3
    //private float elapsedTime = 0;
    //damage1 - 10
    //damage2 - 5
    //damage3 - 22
    //damage special - 13
    public float damage1, damage2, damage3, damageSpecial;

	//public GameObject cooldown;
	public avoidState dodgeState = avoidState.roll;

	//private float currentHealth, maxHealth;

	public GameObject bossStats;
	public int rageCounter, maxRageCounter;//10
	public float rageDurationSec;//10
	public bool isRage = false;
	public float rageModeElapsedTime = 0;

	//public float time;
	//private bool hasReleased;
	private bool isPlayerLocked = false;
	public GameObject PortalSpawner;
    // Start is called before the first frame update
    void Start()
    {
		rageCounter = 0;

		attackState = currentAttackState.attack1;
    }

    // Update is called once per frame
    void Update()
	{
		print($"{isRage}");
		CheckIsGrounded();
		
		if(rageCounter >= maxRageCounter){
			rageModeElapsedTime = rageDurationSec + Time.time;
			rageCounter = 0;
		}


		if(rageModeElapsedTime > Time.time)
		{
			isRage = true;
			rageCounter = 0;
		}
		else
		{
			isRage = false;
			// if(hasReleased == true)
			// 	hasReleased = false;
			if(isPlayerLocked == true)
				ReleasePosition();
		}
		
		BasicBehaviorUpdate();
	}

    protected override void BasicBehaviorUpdate()
    {
	    GenerateAttackZone();
       
	    if (!VitalityHandler.isStagger && !VitalityHandler.isFrozen && !VitalityHandler.isDead)
	    {
		    LookAtPlayer();
		    
		    if(isRage)
			    FollowAndAttackPlayer(enemyData.runSpeed * 1.5f);
		    else
			    FollowAndAttackPlayer(enemyData.runSpeed);

			
		    CheckJumpAnimation();
	    }
    }
    
    private void CheckIsGrounded()
    {
	    _groundCollider = Physics2D.OverlapCircle(transformGround.position, radiusGroundCheck, groundLayer);
	    _isGrounded = _groundCollider != null;
    }

	private void UpdateCurrentAttackState(){
		//Debug.Log("curr:" + currentHealth + "max: " + maxHealth);

		if(isRage /*&& hasReleased == false*/)
		{
			attackState = currentAttackState.attackSpecial;
		}
		else
		{
			var currentHealth = VitalityHandler.currentHealth;
			var maxHealth = VitalityHandler.maxHealth;

			if(currentHealth <= maxHealth * 0.5)
			{
				attackState = currentAttackState.attack3;
			}
			else if (currentHealth <= maxHealth * 0.75)
			{
				attackState = currentAttackState.attack2;
			}
			else
			{
				attackState = currentAttackState.attack1;
			}
		}
	}

	private void CheckJumpAnimation(){
		if (VitalityHandler.isStagger == false){
			//Debug.Log(isGrounded);
            //Check vertical velocity to play falling animation
            if (Rb2D.velocity.y > 0 && _isGrounded == false){
				Animator.SetBool("isJumping", true);
				Animator.SetBool("isFalling", false);
            }else if (Rb2D.velocity.y < -0.01 && _isGrounded == false){
	            Animator.SetBool("isJumping", true);
	            Animator.SetBool("isFalling", true);
            }else if (Rb2D.velocity.y >= -0.01 || _isGrounded == true){
	            Animator.SetBool("isJumping", false);
	            Animator.SetBool("isFalling", false);
			}

        }
        else if (VitalityHandler.isStagger){
			Animator.SetBool("isFalling", false);
			Animator.SetBool("isJumping", false);
        }
	}

	public void Jump(){
		if(gameObject.GetComponent<NPCVitalityHandler>().isFrozen == false)
			gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpForce;  
	}
	
	private void FollowAndAttackPlayer(float speed)
	{
		var distToPlayer = Vector2.Distance(transform.position, PlayerTransform.position);
		var distToTarget = Vector2.Distance(transform.position, currentTarget.position);
		// if(distToPlayer < attackPatternList[0].attackBox.x)
		// {
		// 	if(Time.time > ElapsedTime)
  //           {
  //               AttackPlayerAnim();
  //               ElapsedTime = Time.time + attackIntervalSec;
  //           }
		// 	
		// 	Animator.SetBool("isWalking", false);
		// 	dodgeState = avoidState.block;
		// }
		// else 
		if (distToTarget >= 2)//Keep closing in until player in attack zone
        {
            transform.position += transform.right * speed * Time.deltaTime;
            Animator.SetBool("isWalking", true);
			dodgeState = avoidState.roll;
        }
		
	}

	protected override void AttackPlayerAnim(){
		UpdateCurrentAttackState();
	
		switch(attackState){
			case currentAttackState.attack1:
				if(attackPatternList[0].playerCollider != null){
					Animator.SetTrigger("Attack1");
				}
				break;
			case currentAttackState.attack2:
				if(attackPatternList[1].playerCollider != null){
					Animator.SetTrigger("Attack2");
				}
				break;
			case currentAttackState.attack3:
				if(attackPatternList[2].playerCollider != null){
					Animator.SetTrigger("Attack3");
				}
				break;
			case currentAttackState.attackSpecial:
				if(attackPatternList[3].playerCollider != null /*&& hasReleased ==false*/)
				{
					Animator.SetTrigger("AttackSpecial");
					//rageCounter = 0;
				}
				break;
		}
	}
	
	/*
	The attack calling functions are called as events in the animation when played
	*/
	public void Attack1(){
		if(attackPatternList[0].playerCollider != null)
        {
            PlayerBehaviorScript.CallDamage(damage1);
        }
	}

	public void Attack2(){
		if(attackPatternList[1].playerCollider != null){
			PlayerBehaviorScript.CallDamage(damage2);
		}
	}

	public void Attack3(){
		if(attackPatternList[2].playerCollider != null){
			PlayerBehaviorScript.CallDamage(damage3);
		}
	}

	public void AttackSpecial(){
		if(attackPatternList[3].playerCollider != null){
			PlayerBehaviorScript.CallDamage(damageSpecial);
		}
	}

	//Locking player's position when performing special attack, called as event in animation
	public void LockPosition(){
		isPlayerLocked = true;
		//PlayerBehaviorScript.canMove = false;
		//PlayerBehaviorScript.HaltMoveInput();
		//PlayerBehaviorScript.RigidFreezeAll();

	}

	public void ReleasePosition(){
		//PlayerBehaviorScript.Unfreeze();
		isPlayerLocked = false;
		//PlayerBehaviorScript.canMove = true;
		rageModeElapsedTime = Time.time;
		isRage = false;
		UpdateCurrentAttackState();
		//hasReleased = true;
		Debug.Log("Released");
	}

	void OnDrawGizmosSelected()
    {    
        Gizmos.color = Color.red;
        Gizmos.DrawRay(avoidAnchor.position, dodgingDetection);
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(attackPatternList[0].attackZone.position, attackPatternList[0].attackBox);
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(attackPatternList[1].attackZone.position, attackPatternList[1].attackBox);
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireCube(attackPatternList[2].attackZone.position, attackPatternList[2].attackBox);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(attackPatternList[3].attackZone.position, attackPatternList[3].attackBox);
		Gizmos.color = Color.grey;
		Gizmos.DrawWireSphere(transformGround.position, radiusGroundCheck);
    }
}
