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
    
    void Start(){
        playerBehaviorScript = GameObject.FindWithTag("Player").GetComponent<PlayerBehavior>();
        
    }
    // Update is called once per frame
    void Update()
    {
        playerCollider = Physics2D.OverlapBox(attackZone.position, attackBox, 0f, PlayerLayer);
        DamagePlayer();//update damaging enemy every tick with the array
    }

    private void DamagePlayer()
    {
        if(playerCollider != null)
        {
            this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            playerBehaviorScript.CallDamage(attackDamage);
            animator.SetTrigger("Hit");
            attackDamage = 0;
            playerCollider = null;
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
