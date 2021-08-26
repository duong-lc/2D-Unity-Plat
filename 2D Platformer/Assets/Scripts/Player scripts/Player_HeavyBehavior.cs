using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Player_HeavyBehavior : MonoBehaviour
{
    public float speed; //movement speed (left and right)
    public float jumpForce; //jump force (up)
    public Animator animator;//getting animator to set conditions for animation transitions

    private bool facingRight = true; //bool value to determine the direction the player sprite is facing

    //Vars for checking attack zone
    public Transform attackZone1, attackZone2, attackZone3;
    public Vector2 attackBox1, attackBox2, attackBox3;
    public LayerMask EnemiesLayer;
    private Collider2D[] hitArray1, hitArray2, hitArray3;
    public float attackDamage1, attackDamage2, attackDamage3;

    public float elapsedTime = 0, attackIntervalSec;
    private PlayerBehavior parent_PlayerBehaviorScript;
    private GameObject parent_Player;

    public GameObject heavyCoolDownBar;
    void Start(){
        parent_PlayerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        parent_Player = GameObject.Find("Player");
    }


    void Update(){
        hitArray1 = Physics2D.OverlapBoxAll(attackZone1.position, attackBox1, 0f, EnemiesLayer);
        hitArray2 = Physics2D.OverlapBoxAll(attackZone2.position, attackBox2, 0f, EnemiesLayer);
        hitArray3 = Physics2D.OverlapBoxAll(attackZone3.position, attackBox3, 0f, EnemiesLayer);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {  
            if(Time.time > elapsedTime)
            {
                PlayAttackAnim();
                elapsedTime = Time.time + attackIntervalSec;
            }
        }
    }
    
    void PlayAttackAnim()
    {    
        animator.SetTrigger("Attack1");
        animator.SetTrigger("Attack2");
        animator.SetTrigger("Attack3");
    }

    public void Attack1()//Attack funciton is linked to attack1 and attack2 event as animation event and trigger when anim is played
    {   
        foreach(Collider2D enemy in hitArray1)
        {
            if(enemy.gameObject.tag == "Enemy-Goblin")
                enemy.gameObject.GetComponent<NPCVitalityHandler>().TakeDamage(attackDamage1, true);
            else
                enemy.gameObject.GetComponent<NPCVitalityHandler>().TakeDamage(attackDamage1, false);

            KnockBackEnemy(enemy, 1f);
        }
    }

    public void Attack2()//Attack funciton is linked to attack1 and attack2 event as animation event and trigger when anim is played
    {   
        foreach(Collider2D enemy in hitArray2)
        {
            if(enemy.gameObject.tag == "Enemy-Goblin")
                enemy.gameObject.GetComponent<NPCVitalityHandler>().TakeDamage(attackDamage2, true);
            else
                enemy.gameObject.GetComponent<NPCVitalityHandler>().TakeDamage(attackDamage2, false);

            KnockBackEnemy(enemy, 1.3f);
        }
    }

    public void Attack3()//Attack funciton is linked to attack1 and attack2 event as animation event and trigger when anim is played
    {   
        foreach(Collider2D enemy in hitArray3)
        {
            if(enemy.gameObject.tag == "Enemy-Goblin")
                enemy.gameObject.GetComponent<NPCVitalityHandler>().TakeDamage(attackDamage3, true);
            else
                enemy.gameObject.GetComponent<NPCVitalityHandler>().TakeDamage(attackDamage3, false);
 
            KnockBackEnemy(enemy, 1.5f);
            
        }
    }

    async private void KnockBackEnemy(Collider2D enemy, float multiplier){
        if(enemy.gameObject.tag != "Enemy-Demon")
        {
            Vector2 difference = enemy.transform.position - parent_Player.transform.position;
            if(enemy.gameObject.tag == "Enemy-FlyingEye")
                enemy.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.5f;
            
                
            enemy.gameObject.GetComponent<Rigidbody2D>().AddForce(70f * multiplier * difference.normalized, ForceMode2D.Impulse);
            enemy.gameObject.GetComponent<Rigidbody2D>().AddForce(enemy.transform.up * 80f * multiplier, ForceMode2D.Impulse);
        
            

            await Task.Delay(1000);
            if(enemy.gameObject.tag == "Enemy-FlyingEye")
                enemy.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;
        }
        
        

    }


    void OnDrawGizmosSelected()
    {    
        if (attackZone1 == null || attackZone2 == null || attackZone3 == null)
        {
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(attackZone1.position, attackBox1);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackZone2.position, attackBox2);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackZone3.position, attackBox3);
    }

    
}
