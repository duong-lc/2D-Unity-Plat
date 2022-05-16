using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class NPC_Enemy_MushroomBehavior : NPC_Enemy_Base
{
    private bool _cycleAtkAnim = false;

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
           // print($"ahhaha");
            if (playerCol.gameObject.CompareTag("Player"))
            {
               // print($"ahhaha");
                if(_cycleAtkAnim)
                {
                    Animator.SetTrigger("Attack1");
                }
                else
                {
                    Animator.SetTrigger("Attack2");
                }
                _cycleAtkAnim = !_cycleAtkAnim;
                    
            }
        }
    }

    public void AttackPlayer()//Set up as an event in the attack animation in animation
    {
        if(attackPatternList[0].playerCollider != null)
        {
            PlayerBehaviorScript.CallDamage(attackPatternList[0].attackDamage);
        }
    }
    

    private void OnDrawGizmosSelected()
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
