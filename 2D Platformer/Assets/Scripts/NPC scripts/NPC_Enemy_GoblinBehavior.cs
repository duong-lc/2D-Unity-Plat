using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using UnityEngine.Serialization;

public class NPC_Enemy_GoblinBehavior : NPC_Enemy_Base
{
   
 //    private Rigidbody2D rb;
 //    public Animator animator;
 //    
 //    public Transform player;
	// private bool isFlipped = true;
 //    public float runSpeed;//3
    //private bool isStagger = false;

    // public Transform attackZone;
    // public Vector2 attackBox;//1.3 - 1.3
    // public Transform attackZone2;
    // public Vector2 attackBox2;//1.3 - 1
    // public LayerMask PlayerLayer;
    // private Collider2D playerCollider;
    // private Collider2D playerCollider2;

    //attack speed and damage
    // public float attackIntervalSec;//1
    // public float attackInterval2Sec;//6
    // private float elapsedTime = 0;
    public float elapsedTime2 = 0;
    // public float attackDamage;//10
    // public float attackDamage2;//18
    // Start is called before the first frame update
    // void Awake()
    // {
    //     playerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();
    //     player = GameObject.FindGameObjectWithTag("Player").transform;
    //
    //     rb = this.gameObject.GetComponent<Rigidbody2D>();
    //     rb.freezeRotation = true;//freezing rotation
    //     
    // }

    // Update is called once per frame
    void Update()
    {
        BasicBehaviorUpdate();
        // //generating attackable zone for AI to attack the player
        // playerCollider = Physics2D.OverlapBox(attackZone.position, attackBox, 0f, PlayerLayer);
        // playerCollider2 = Physics2D.OverlapBox(attackZone2.position, attackBox2, 0f, PlayerLayer);
        //
        // if (this.gameObject.GetComponent<NPCVitalityHandler>().isStagger == false && 
        // this.gameObject.GetComponent<NPCVitalityHandler>().isFrozen == false && 
        // this.gameObject.GetComponent<NPCVitalityHandler>().isDead == false)
        // {
        //     LookAtPlayer();
        //     FollowAndAttackPlayer();
        // }
    }

    public void AttackPlayerAnim(int num)
    {
        var playerCol = attackPatternList[0].playerCollider;
        if (playerCol && num == 1)
        {
            if (playerCol.gameObject.CompareTag("Player"))
            {
                Animator.SetTrigger("Attack1");
            }
        }
        else if (num == 2)
        {
            Animator.SetTrigger("Attack2");
        }
    }

    public void AttackPlayer()//Set up as an event in the attack animation in animation
    {
        if(attackPatternList[0].playerCollider  != null)
        {
            PlayerBehaviorScript.CallDamage(attackPatternList[0].attackDamage );
        }
    }

    protected override void FollowAndAttackPlayer()
    {
        var attackBox = attackPatternList[0].attackBox;
        var distToPlayer = Vector2.Distance(transform.position, PlayerTransform.position);
        var attackIntervalSec = attackPatternList[0].attackIntervalSec;
    
        if (distToPlayer >= attackBox.x)//Keep closing in until player in attack zone
        {
            transform.position += transform.right * (enemyData.runSpeed * Time.deltaTime);
            Animator.SetBool("isWalking", true);
        }
        else if (distToPlayer < attackBox.x)//if player is in, attack animation
        {
            if(Time.time > ElapsedTime)
            {
                AttackPlayerAnim(1);
                ElapsedTime = Time.time + attackIntervalSec;
            }
            
            Animator.SetBool("isWalking", false);
        }
        
    }

  
    public void AttackPlayerSurprise()//Set up as an event in the attack animation in animation
    {
        
        if(attackPatternList[1].playerCollider != null)
        {
            PlayerBehaviorScript.CallDamage(attackPatternList[1].attackDamage);
        }
    }

    
 //    void LookAtPlayer()
	// {
	// 	Vector3 flipped = transform.localScale;
	// 	flipped.z *= -1f;
 //
	// 	if (transform.position.x > player.position.x && isFlipped)
	// 	{
	// 		transform.localScale = flipped;
	// 		transform.Rotate(0f, 180f, 0f);
	// 		isFlipped = !isFlipped;
 //            this.gameObject.GetComponent<NPCVitalityHandler>().healthBar.Flip();
 //            cooldown.GetComponent<CooldownBar_NPC>().Flip();
	// 	}
	// 	else if (transform.position.x < player.position.x && !isFlipped)
	// 	{
	// 		transform.localScale = flipped;
	// 		transform.Rotate(0f, 180f, 0f);
	// 		isFlipped = !isFlipped;
 //            this.gameObject.GetComponent<NPCVitalityHandler>().healthBar.Flip();
 //            cooldown.GetComponent<CooldownBar_NPC>().Flip();
	// 	}
	// }

    void OnDrawGizmosSelected()
    {
        if (attackPatternList.Count == 0) return;
        
        var attackZone = attackPatternList[0].attackZone;
        var attackZone2 = attackPatternList[1].attackZone;
        var attackBox = attackPatternList[0].attackBox;
        var attackBox2 = attackPatternList[1].attackBox;
        if (attackZone == null)
        {
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(attackZone.position, attackBox);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackZone2.position, attackBox2);
    }
    public float GetAttackInterval2()
    {
        return attackPatternList[1].attackIntervalSec;
    }

}
