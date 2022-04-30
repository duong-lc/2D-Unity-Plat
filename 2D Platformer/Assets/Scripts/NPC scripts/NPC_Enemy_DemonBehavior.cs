using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using UnityEngine.Serialization;

public class NPC_Enemy_DemonBehavior : NPC_Enemy_Base
{
    private SpriteRenderer _spriteRenderer => GetComponent<SpriteRenderer>();
    
    [SerializeField] private GameObject _fireGameObj;
    private Animator _fireAnimator;

    public bool isRedTint = false, isCyanTint = false;

    private void Start()
    {
        _fireAnimator = _fireGameObj.GetComponent<Animator>();
        Physics2D.IgnoreCollision(PlayerTransform.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    private void LateUpdate() {
        //this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        if(isRedTint)
        {
            _spriteRenderer.color = Color.red;
        }
        if(isCyanTint)
        {
            _spriteRenderer.color = Color.cyan;
        }
        if(!isCyanTint && !isRedTint)
        {
            _spriteRenderer.color = Color.white;
        }
    }
    private void Update() 
    {
        BasicBehaviorUpdate();
    }

    protected override void FollowAndAttackPlayer()
    {
        var attackBox = attackPatternList[0].attackBox;
        var distToPlayer = Vector2.Distance(transform.position, PlayerTransform.position);
        var attackIntervalSec = attackPatternList[0].attackIntervalSec;
        
        //player height +5
        Vector2 anchor = new Vector2(PlayerTransform.position.x, PlayerTransform.position.y + 3);
        if(Vector2.Distance(anchor, transform.position) >= 0)
        {
            if(transform.position.y > anchor.y)
                transform.position += -transform.up * (enemyData.runSpeed * (Time.deltaTime/3));
            if(transform.position.y < anchor.y)
                transform.position -= -transform.up * (enemyData.runSpeed * (Time.deltaTime/3));
        }
        
        if (distToPlayer >= attackBox.x)//Keep closing in until player in attack zone
        {
            transform.position += transform.right * (enemyData.runSpeed * Time.deltaTime);
            Animator.SetTrigger("isIdle");
        }
        else if (distToPlayer < attackBox.x)//if player is in, attack animation
        {
            if(Time.time > ElapsedTime)
            {
                AttackPlayerAnim();
                ElapsedTime = Time.time + attackIntervalSec;
            }
            
            Animator.SetTrigger("isIdle");
        }   
    }

    void AttackPlayerAnim()
    {
        var playerCol = attackPatternList[0].playerCollider;
        if (playerCol)
        {
            if (playerCol.gameObject.CompareTag("Player"))
            {
                Animator.SetTrigger("isAttack");
            }
        }
        
    }

    void ShootOutFire()
    {
        _fireGameObj.SetActive(true);
        _fireAnimator.Play("Demon-Fire", 0, 0f);
    }

    void StopFire()
    {
        _fireGameObj.SetActive(false);
        //Fire_Animator.Play("Demon-Fire", 0, 0f);
    }

    public void AttackPlayer()//Set up as an event in the attack animation in animation
    {
        if(attackPatternList[0].playerCollider != null)
        {
            PlayerBehaviorScript.CallDamage(attackPatternList[0].attackDamage);
        }
    }
    
    void OnDrawGizmosSelected()
    {    
        if (attackPatternList.Count == 0) return;
        
        var attackZone = attackPatternList[0].attackZone;
        var attackBox = attackPatternList[0].attackBox;
        if (attackZone == null)
        {
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(attackZone.position, attackBox);
    }
}
