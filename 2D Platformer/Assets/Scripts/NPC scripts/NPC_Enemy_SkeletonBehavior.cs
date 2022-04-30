using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using UnityEngine.Serialization;

public class NPC_Enemy_SkeletonBehavior : NPC_Enemy_Base
{
    // Update is called once per frame
    private void Update()
    {
        BasicBehaviorUpdate();
    }
    
    
    protected override void AttackPlayerAnim()
    {
        var playerCol = attackPatternList[0].playerCollider;
        if (playerCol)
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