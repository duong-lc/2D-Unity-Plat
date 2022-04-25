using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAnimHandler : PlayerBaseBehavior
{
    //private PlayerBehavior parent_PlayerBehaviorScript;
    public bool facingRight = true;

    public void PlayerRunning(){
        //Animator.SetFloat("Speed", Mathf.Abs(ParentPlayerBehaviorScript.moveInput));
        Animator.SetFloat(Run, Mathf.Abs(ParentPlayerBehaviorScript.moveInput));
        
        if(!ParentPlayerBehaviorScript.isInDeathAnim){
                //Flip player facing direction based on key pressing
            if (!facingRight && ParentPlayerBehaviorScript.moveInput > 0){
                Flip();
                GetComponent<PlayerVitalityHandler>().healthBar.Flip();
            }else if (facingRight && ParentPlayerBehaviorScript.moveInput < 0){
                Flip();
                GetComponent<PlayerVitalityHandler>().healthBar.Flip();
            } 
        }
          
    }

    /*
    *   This funciton regulates anim-bool values for playing animations of player-*insert role*
    *       - while the player is not taking damage, allow the anim-bool values to be affected by parent-player's y velocity val
    *       - when the player is taking damage, the anim-bool values are forced to 'false' to deactivate the current jumping/falling anim
    *       if the player is still airborned, allowing the "gethit" animation to be played mid-air
    */
    public void PlayerJumping(){
        if (this.gameObject.GetComponent<PlayerVitalityHandler>().isPlayerTakingDamage == false){
            //Check vertical velocity to play falling animation
            if (ParentPlayerBehaviorScript.playerRB.velocity.y > 0){
                Animator.SetBool(Fall, false);
            }else if (ParentPlayerBehaviorScript.playerRB.velocity.y < 0){
                Animator.SetBool(Fall, true);
            }

            //Check to see player on ground to play jump animation
            if (ParentPlayerBehaviorScript.isGrounded == false){
                Animator.SetBool(Jump, true);
            }else if (ParentPlayerBehaviorScript.isGrounded == true){
                Animator.SetBool(Jump, false);
            }
        }
        else if (this.gameObject.GetComponent<PlayerVitalityHandler>().isPlayerTakingDamage == true){
            Animator.SetBool(Fall, false);
            Animator.SetBool(Jump, false);
        }
    }
    public void Flip(){
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
