using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireBall : MonoBehaviour
{
    public Animator animator;

    public Transform attackZone;//center of attack box
    public Vector2 attackBox;//attack box where the arrow would deal damage
    public LayerMask PlayerLayer;
    private Collider2D playerCollider;
    public float attackDamage = 20f;//attack damage

    private PlayerBehavior playerBehaviorScript;
    private Player_KatanaBehavior player_KatanaBehaviorScript;
    private Player_ArcherBehavior player_ArcherBehaviorScript;
    private Player_HeavyBehavior player_HeavyBehaviorScript;
    private Player_MageBehavior player_MageBehaviorScript;
    
    void Start(){
        playerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        
    }
    // Update is called once per frame
    void Update()
    {
        try{
            player_ArcherBehaviorScript = GameObject.Find("Player").transform.Find("Player-Archer").GetComponent<Player_ArcherBehavior>();
            player_KatanaBehaviorScript = GameObject.Find("Player").transform.Find("Player-Katana").GetComponent<Player_KatanaBehavior>();
            player_HeavyBehaviorScript = GameObject.Find("Player").transform.Find("Player-Heavy").GetComponent<Player_HeavyBehavior>();
            player_MageBehaviorScript = GameObject.Find("Player").transform.Find("Player-Mage").GetComponent<Player_MageBehavior>();
        }catch(Exception e){
            ;
        }

        playerCollider = Physics2D.OverlapBox(attackZone.position, attackBox, 0f, PlayerLayer);
        DamagePlayer();//update damaging enemy every tick with the array
    }

    private void DamagePlayer()
    {
        if(playerCollider != null)
        {
            this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            if (playerBehaviorScript.currentCharacter == 2)
            {
                player_ArcherBehaviorScript.TakingDamage(attackDamage);
                animator.SetTrigger("Hit");
                attackDamage = 0;//set atkdamage to 0 to avoid update() keep damaging player whilst playing explode anim of fireball
            }
            else if (playerBehaviorScript.currentCharacter == 1)
            {
                player_KatanaBehaviorScript.TakingDamage(attackDamage);
                animator.SetTrigger("Hit");
                attackDamage = 0;
            }
            else if (playerBehaviorScript.currentCharacter == 3)
            {
                player_HeavyBehaviorScript.TakingDamage(attackDamage);
                animator.SetTrigger("Hit");
                attackDamage = 0;
            } 
            else if(playerBehaviorScript.currentCharacter == 4)
            {
                player_MageBehaviorScript.TakingDamage(attackDamage);
                animator.SetTrigger("Hit");
                attackDamage = 0;
            }  
        }
        Destroy(this.gameObject, 5f);
    }

    public void DestroyObject(){
        Destroy(this.gameObject);
    }
    void OnDrawGizmosSelected()//Drawing attack box for arrow
    {    
        if (attackZone == null)
        {
            return;
        }
        Gizmos.DrawWireCube(attackZone.position, attackBox);
        
    }
}
