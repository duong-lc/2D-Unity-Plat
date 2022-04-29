using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NPCVitalityHandler))]

public class NPC_Enemy_Base : MonoBehaviour
{
    [SerializeField] protected NPCEnemyVariantsSO enemyData;
    [SerializeField] protected List<AttackPattern> attackPatternList;
    
    protected Rigidbody2D Rb2D => GetComponent<Rigidbody2D>();
    protected Animator Animator => GetComponent<Animator>();
    protected Transform PlayerTransform => PlayerBehavior.Instance.gameObject.transform;
    protected PlayerBehavior PlayerBehaviorScript => PlayerBehavior.Instance;
    protected NPCVitalityHandler VitalityHandler => GetComponent<NPCVitalityHandler>();

    protected bool IsFlipped = true;
    protected float RunSpeed;
    
   
    
    protected void BasicBehaviorUpdate()
    {
        GenerateAttackZone();
       
        if (!VitalityHandler.isStagger && !VitalityHandler.isFrozen && !VitalityHandler.isDead)
        {
            LookAtPlayer();
            FollowAndAttackPlayer();
        }
    }

    protected virtual void SetUpAttackSpeedAndDamage()
    {
        
    }
    
    protected void GenerateAttackZone()
    {
        //generating attackable zone for AI to attack the player
        for (int i = 0; i < attackPatternList.Count; i++)
        {
            print($"hello");
            var attackPattern =  attackPatternList[i];
            attackPattern.playerCollider = Physics2D.OverlapBox(
                attackPattern.attackZone.position, 
                attackPattern.attackBox, 
                0f, 
                enemyData.PlayerLayer);
        }
    }
    
    protected virtual void FollowAndAttackPlayer()
    {
        var attackBox = attackPatternList[0].attackBox;
        var distToPlayer = Vector2.Distance(transform.position, PlayerTransform.position);
        var elapsedTime = attackPatternList[0].elapsedTime;
        var attackIntervalSec = attackPatternList[0].attackIntervalSec;

        if (distToPlayer >= attackBox.x)//Keep closing in until player in attack zone
        {
            transform.position += transform.right * RunSpeed * Time.deltaTime;
            Animator.SetBool("isWalking", true);
        }
        else if (distToPlayer < attackBox.x)//if player is in, attack animation
        {
            if(Time.time > elapsedTime)
            {
                AttackPlayerAnim();
                elapsedTime = Time.time + attackIntervalSec;
            }
            
            Animator.SetBool("isWalking", false);
        }
    }

    protected virtual void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        var playerXPos = PlayerTransform.position.x;
        
        
        if (transform.position.x > playerXPos && IsFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            IsFlipped = !IsFlipped;
            VitalityHandler.healthBar.Flip();
        }
        else if (transform.position.x < playerXPos && !IsFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            IsFlipped = !IsFlipped;
            VitalityHandler.healthBar.Flip();
        }
    }

    protected virtual void AttackPlayerAnim()
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

    protected virtual void AttackPlayer()
    {
        var playerCol = attackPatternList[0].playerCollider;
        var attackDamage = attackPatternList[0].attackDamage;
        
        if(playerCol != null)
        {
            PlayerBehaviorScript.CallDamage(attackDamage);
        }
    }
    
}
