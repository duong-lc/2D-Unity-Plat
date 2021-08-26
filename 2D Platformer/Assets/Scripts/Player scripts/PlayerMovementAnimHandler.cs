using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAnimHandler : MonoBehaviour
{
    private Animator animator;
    private PlayerBehavior parent_PlayerBehaviorScript;
    public bool facingRight = true;
    private void Start() {
        animator = this.gameObject.GetComponent<Animator>();
        parent_PlayerBehaviorScript = this.gameObject.transform.parent.GetComponent<PlayerBehavior>();
    }

    public void PlayerRunning(){
        animator.SetFloat("Speed", Mathf.Abs(parent_PlayerBehaviorScript.moveInput));

        if(!parent_PlayerBehaviorScript.isInDeathAnim){
                //Flip player facing direction based on key pressing
            if (facingRight == false && parent_PlayerBehaviorScript.moveInput > 0){
                Flip();
                this.gameObject.GetComponent<PlayerVitalityHandler>().healthBar.Flip();
            }else if (facingRight == true && parent_PlayerBehaviorScript.moveInput < 0){
                Flip();
                this.gameObject.GetComponent<PlayerVitalityHandler>().healthBar.Flip();
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
            if (parent_PlayerBehaviorScript.playerRB.velocity.y > 0){
                animator.SetBool("IsFalling", false);
            }else if (parent_PlayerBehaviorScript.playerRB.velocity.y < 0){
                animator.SetBool("IsFalling", true);
            }

            //Check to see player on ground to play jump animation
            if (parent_PlayerBehaviorScript.isGrounded == false){
                animator.SetBool("IsJumping", true);
            }else if (parent_PlayerBehaviorScript.isGrounded == true){
                animator.SetBool("IsJumping", false);
            }
        }
        else if (this.gameObject.GetComponent<PlayerVitalityHandler>().isPlayerTakingDamage == true){
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsJumping", false);
        }
    }
    public void Flip(){
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
