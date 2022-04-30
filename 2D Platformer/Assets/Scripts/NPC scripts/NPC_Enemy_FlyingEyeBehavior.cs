using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class NPC_Enemy_FlyingEyeBehavior : NPC_Enemy_Base
{
    private float _elapsedTime2 = 0;

    private bool _isContactWithPlayer = false;
    private bool _isContactWithGround = false;
    
    private void Update()
    {
        BasicBehaviorUpdate();
    }
    
    // protected override void GenerateAttackZone()
    // {
    //     //generating attackable zone for AI to attack the player
    //     for(int i = 0; i < attackPatternList.Count; i++)
    //     {
    //         Collider2D playerCollider;
    //         //prioritize getting circular damage zones if radius exists
    //         if (attackPatternList[i].attackRadius > 0)
    //         {
    //             playerCollider = Physics2D.OverlapCircle(
    //                 attackPatternList[i].attackZone.position, 
    //                 attackPatternList[i].attackRadius,
    //                 enemyData.PlayerLayer);
    //         }
    //         //if radius == 0 then just get box
    //         else
    //         {
    //                 playerCollider = Physics2D.OverlapBox(
    //                 attackPatternList[i].attackZone.position, 
    //                 attackPatternList[i].attackBox, 
    //                 0f, 
    //                 enemyData.PlayerLayer);
    //         }
    //         attackPatternList[i] = new AttackPattern()
    //         {
    //             attackZone = attackPatternList[i].attackZone,
    //             attackBox =  attackPatternList[i].attackBox,
    //             attackRadius =  attackPatternList[i].attackRadius,
    //             playerCollider = playerCollider,
    //             attackIntervalSec = attackPatternList[i].attackIntervalSec,
    //             attackDamage = attackPatternList[i].attackDamage
    //         };
    //     }
    // }
    
    private void AttackPlayerAnim(int num)
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
        if(attackPatternList[0].playerCollider != null){
            PlayerBehaviorScript.CallDamage(attackPatternList[0].attackDamage);
        }
    }

    public void AttackPlayerSurprise()
    {
        //Debug.Log(damageCollider2.gameObject);
        if(attackPatternList[2].playerCollider != null){
            PlayerBehaviorScript.CallDamage(attackPatternList[2].attackDamage);
        }  
    }

    public async void LockOnPlayerPosition()//Set up as an event in the attack animation in animation
    {
        //groundCollider.GetComponent<CircleCollider2D>().enabled = false;
        Rb2D.constraints = RigidbodyConstraints2D.FreezePosition;
        //currHeight = this.gameObject.transform.position.y;
        StartCoroutine(TranslateToPlayer());
        await Task.Delay((int)(attackPatternList[2].attackIntervalSec*1000 - 1000));
        TranslateBack();
    }

    private void TranslateBack()
    {
        try{
            Rb2D.constraints = RigidbodyConstraints2D.None;
            Rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        catch (Exception e)
        {
            ;
        }
        
        //groundCollider.GetComponent<CircleCollider2D>().enabled = true;
    }

    private IEnumerator TranslateToPlayer()
    {
        cooldown.StartCoolDown();
        //groundColliderTrigger.SetActive(false);
        Vector2 difference = PlayerTransform.position - this.gameObject.transform.position;
        float dist = Mathf.Sqrt(difference.x*difference.x + difference.y*difference.y);

        float temp = dist/70;
        for(int i = 0; i < 20; i++)
        {
            if(_isContactWithGround == true)
            {
                _isContactWithGround = false;
                break;
            }

            if(_isContactWithPlayer == true)
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
        if(other.gameObject.CompareTag("Player"))
        {
            _isContactWithPlayer = true;
        }
        if(other.gameObject.CompareTag("ground"))
        {
            _isContactWithGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            _isContactWithPlayer = false;
        }
        if(other.gameObject.CompareTag("ground"))
        {
            _isContactWithGround = false;
        }
    }

    protected override void FollowAndAttackPlayer()
    {
        var distToPlayer = Vector2.Distance(transform.position, PlayerTransform.position);
        if (distToPlayer >= attackPatternList[0].attackBox.x)//Keep closing in until player in attack zone
        {
            transform.position += transform.right * (enemyData.runSpeed * Time.deltaTime);
            Animator.SetBool("isWalking", true);

            if (Time.time > _elapsedTime2)
            {  
                if(attackPatternList[1].playerCollider != null)
                {
                    AttackPlayerAnim(2);
                    _elapsedTime2 = Time.time + attackPatternList[2].attackIntervalSec;
                }
                Animator.SetBool("isWalking", false);
            }

        }
        else if (distToPlayer < attackPatternList[0].attackBox.x)//if player is in, attack animation
        {
            if(Time.time > ElapsedTime)
            {
                AttackPlayerAnim(1);
                ElapsedTime = Time.time + attackPatternList[0].attackIntervalSec;
            }
            
            Animator.SetBool("isWalking", false);
        }   
    }

    void OnDrawGizmosSelected()
    {    
        if (attackPatternList.Count == 0) return;
        
        var attackZone = attackPatternList[0].attackZone;
        var attackBox = attackPatternList[0].attackBox;
        var attackZone2 = attackPatternList[1].attackZone;
        var attackBox2 = attackPatternList[1].attackBox;
        var damageZone2 = attackPatternList[2].attackZone;
        var damageRadius = attackPatternList[2].attackRadius;
        
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

    public float GetAttackInterval2()
    {
        return attackPatternList[2].attackIntervalSec;
    }

}
