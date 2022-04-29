using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class NPC_Enemy_SkeletonBehavior : NPC_Enemy_Base
{
    // private Rigidbody2D rb;
    // public Animator animator;
    
    //public Transform player;
	// private bool isFlipped = true;
    //public float runSpeed;//1
    //private bool isStagger = false;

    // public Transform attackZone;
    // public Vector2 attackBox;
    // public LayerMask PlayerLayer;
    // private Collider2D playerCollider;

    //attack speed and damage
    // public float attackIntervalSec;//1
    // private float elapsedTime = 0;//0
    // public float attackDamage = 25;//12

    //private PlayerBehavior playerBehaviorScript;
    // Start is called before the first frame update
    void Awake()
    {
        //playerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        
        
        //rb = this.gameObject.GetComponent<Rigidbody2D>();
        Rb2D.freezeRotation = true;//freezing rotation

        // currentHealth = maxHealth;
        // healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    private void Update()
    {
        //generating attackable zone for AI to attack the player
       // playerCollider = Physics2D.OverlapBox(attackZone.position, attackBox, 0f, PlayerLayer);

       //GenerateAttackZone();
       var attackPattern =  attackPatternList[0];
       attackPattern.playerCollider = Physics2D.OverlapBox(
           attackPattern.attackZone.position, 
           attackPattern.attackBox, 
           0f, 
           enemyData.PlayerLayer);
        if (!VitalityHandler.isStagger && !VitalityHandler.isFrozen && !VitalityHandler.isDead)
        {
            LookAtPlayer();
            FollowAndAttackPlayer();
        }
        //BasicBehaviorUpdate();

    }

    protected override void AttackPlayerAnim()
    {
        var playerCol = attackPatternList[0].playerCollider;
        if (playerCol != null)
        {
            if (playerCol.gameObject.CompareTag("Player"))
            {
                Animator.SetTrigger("Attack");
            }
        }
    }

    public void AttackPlayer()//Set up as an event in the attack animation in animation
    {
        var playerCol = attackPatternList[0].playerCollider;
        if(playerCol != null)
        {
            PlayerBehaviorScript.CallDamage(attackPatternList[0].attackDamage);
        }
    }

    // protected override void FollowAndAttackPlayer()
    // {
    //     var attackBox = enemyData.AttackPatternList[0].attackBox;
    //     var distToPlayer = Vector2.Distance(transform.position, PlayerTransform.position);
    //     
    //     if (distToPlayer >= attackBox.x)//Keep closing in until player in attack zone
    //     {
    //         transform.position += transform.right * RunSpeed * Time.deltaTime;
    //         Animator.SetBool("isWalking", true);
    //     }
    //     else if (distToPlayer < attackBox.x)//if player is in, attack animation
    //     {
    //         if(Time.time > elapsedTime)
    //         {
    //             AttackPlayerAnim();
    //             elapsedTime = Time.time + attackIntervalSec;
    //         }
    //         
    //         Animator.SetBool("isWalking", false);
    //     }
    //     
    // }

    // void LookAtPlayer()
	// {
	// 	Vector3 flipped = transform.localScale;
	// 	flipped.z *= -1f;
 //        var playerXPos = PlayerTransform.position.x;
 //        
 //        
	// 	if (transform.position.x > playerXPos && IsFlipped)
	// 	{
	// 		transform.localScale = flipped;
	// 		transform.Rotate(0f, 180f, 0f);
 //            IsFlipped = !IsFlipped;
 //            VitalityHandler.healthBar.Flip();
	// 	}
	// 	else if (transform.position.x < playerXPos && !IsFlipped)
	// 	{
	// 		transform.localScale = flipped;
	// 		transform.Rotate(0f, 180f, 0f);
 //            IsFlipped = !IsFlipped;
 //            VitalityHandler.healthBar.Flip();
	// 	}
	// }

    void OnDrawGizmosSelected()
    {
        if (attackPatternList.Count == 0) return;
        
        var attackZone = attackPatternList[0].attackZone;
        var attackBox = attackPatternList[0].attackBox;
        if (attackZone == null)
        {
            return;
        }
        Gizmos.DrawWireCube(attackZone.position, attackBox);
    }

   
}
