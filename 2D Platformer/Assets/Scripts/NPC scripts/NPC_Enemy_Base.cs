using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NPCVitalityHandler))]

public class NPC_Enemy_Base : MonoBehaviour//, IEnemyBehavior
{
    [SerializeField] protected NPCEnemyVariantsSO enemyData;

    [SerializeField] protected List<AttackPattern> attackPatternList;
    public CooldownBar_NPC cooldown;
    protected Rigidbody2D Rb2D => GetComponent<Rigidbody2D>();
    protected Animator Animator => GetComponent<Animator>();
    protected Transform PlayerTransform => PlayerBehavior.Instance.gameObject.transform;
    protected PlayerBehavior PlayerBehaviorScript => PlayerBehavior.Instance;
    protected NPCVitalityHandler VitalityHandler => GetComponent<NPCVitalityHandler>();
    
    protected bool IsFlipped = true;
    //[SerializeField] protected float RunSpeed;
    protected float ElapsedTime = 0;

    private void Awake()
    {
        Rb2D.freezeRotation = true;//freezing rotation
    }
    
    protected virtual void BasicBehaviorUpdate()
    {
        GenerateAttackZone();
       
        if (!VitalityHandler.isStagger && !VitalityHandler.isFrozen && !VitalityHandler.isDead)
        {
            LookAtPlayer();
            FollowAndAttackPlayer();
        }
    }

    protected virtual void GenerateAttackZone()
    {
        //generating attackable zone for AI to attack the player
        for(int i = 0; i < attackPatternList.Count; i++)
        {
            Collider2D playerCollider;
            //prioritize getting circular damage zones if radius exists
            if (attackPatternList[i].attackRadius > 0)
            {
                playerCollider = Physics2D.OverlapCircle(
                    attackPatternList[i].attackZone.position, 
                    attackPatternList[i].attackRadius,
                    enemyData.PlayerLayer);
            }
            //if radius == 0 then just get box
            else
            {
                playerCollider = Physics2D.OverlapBox(
                    attackPatternList[i].attackZone.position, 
                    attackPatternList[i].attackBox, 
                    0f, 
                    enemyData.PlayerLayer);
            }
            attackPatternList[i] = new AttackPattern()
            {
                attackZone = attackPatternList[i].attackZone,
                attackBox =  attackPatternList[i].attackBox,
                attackRadius =  attackPatternList[i].attackRadius,
                playerCollider = playerCollider,
                attackIntervalSec = attackPatternList[i].attackIntervalSec,
                attackDamage = attackPatternList[i].attackDamage
            };
        }
    }
    
    protected virtual void FollowAndAttackPlayer()
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
                print($"calling from base");
                AttackPlayerAnim();
                ElapsedTime = Time.time + attackIntervalSec;
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
            if(cooldown) cooldown.Flip();//flip cooldown bar if exists
        }
        else if (transform.position.x < playerXPos && !IsFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            IsFlipped = !IsFlipped;
            VitalityHandler.healthBar.Flip();
            if(cooldown) cooldown.Flip();//flip cooldown bar if exists
        }
    }

    protected virtual void AttackPlayerAnim()
    {
        print($"parent");
    }
}
