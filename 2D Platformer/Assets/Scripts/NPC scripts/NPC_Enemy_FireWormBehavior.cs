using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class NPC_Enemy_FireWormBehavior : NPC_Enemy_Base
{
    [SerializeField] private GameObject fireBallPrefab;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private int fireBallSpeed;//10
    
    private bool _keepMoving = false;
    // Start is called before the first frame update
    private void Start()
    {
        fireBallPrefab.GetComponent<FireBall>().attackDamage = attackPatternList[0].attackDamage;
    }
    
    void Update()
    {
        BasicBehaviorUpdate();
    }

    public void Attack()
    {
        var fireBall = Instantiate(fireBallPrefab, shootingPoint.position, Quaternion.identity);

        if(IsFlipped){
            fireBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.right * fireBallSpeed);
        }
        else
        {
            fireBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.left * -fireBallSpeed);
            fireBall.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    void AttackPlayerAnim()
    {
        if (attackPatternList[0].playerCollider)
        {
            _keepMoving = false;
            Animator.SetTrigger("Attack");
        }
        else
        {
            _keepMoving = true;
        }
    }

    protected override void FollowAndAttackPlayer()
    {
        var attackBox = attackPatternList[0].attackBox;
        var distToPlayer = Vector2.Distance(transform.position, PlayerTransform.position);
        var attackIntervalSec = attackPatternList[0].attackIntervalSec;
        
        if (distToPlayer >= attackBox.x || _keepMoving == true)//Keep closing in until player in attack zone
        {
            transform.position += transform.right * (enemyData.runSpeed * Time.deltaTime);
            Animator.SetBool("isWalking", true);
        }
        if (distToPlayer < attackBox.x)//if player is in, attack animation
        {
            if(Time.time > ElapsedTime)
            {
                AttackPlayerAnim();
                ElapsedTime = Time.time + attackIntervalSec;
            }
            
            Animator.SetBool("isWalking", false);
        }
        
    }

    void OnDrawGizmosSelected()
    {
        var attackZone = attackPatternList[0].attackZone;
        var attackBox = attackPatternList[0].attackBox;
        if (attackZone == null)
        {
            return;
        }
        Gizmos.DrawWireCube(attackZone.position, attackBox);
    }

    
}
