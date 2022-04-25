using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Player_HeavyBehavior : PlayerBaseBehavior
{
    // public float speed; //movement speed (left and right)//5.75
    // public float jumpForce; //jump force (up)//7.5
    // public Animator animator;//getting animator to set conditions for animation transitions
    
    //Vars for checking attack zone
    public Transform attackZone1, attackZone2, attackZone3;
    public Vector2 attackBox1, attackBox2, attackBox3;
    public LayerMask EnemiesLayer;
    private Collider2D[] _hitArray1, _hitArray2, _hitArray3;
    public float attackDamage1, attackDamage2, attackDamage3;

    public float elapsedTime = 0, attackIntervalSec;
    // private PlayerBehavior parent_PlayerBehaviorScript;
    // private GameObject parent_Player;

    public GameObject heavyCoolDownBar;


    void Update(){
        _hitArray1 = Physics2D.OverlapBoxAll(attackZone1.position, attackBox1, 0f, EnemiesLayer);
        _hitArray2 = Physics2D.OverlapBoxAll(attackZone2.position, attackBox2, 0f, EnemiesLayer);
        _hitArray3 = Physics2D.OverlapBoxAll(attackZone3.position, attackBox3, 0f, EnemiesLayer);
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
        // print($"{Attacks.Length}");
        // foreach (int i in Attacks)
        // {
        //     print($"number {i}");
        // }
        Animator.SetTrigger(Attacks[0]);
        Animator.SetTrigger(Attacks[1]);
        Animator.SetTrigger(Attacks[2]);
    }

    public void Attack1()//Attack funciton is linked to attack1 and attack2 event as animation event and trigger when anim is played
    {   
        foreach(Collider2D enemy in _hitArray1)
        {
            enemy.gameObject.GetComponent<NPCVitalityHandler>()
                .TakeDamage(attackDamage1, enemy.gameObject.CompareTag("Enemy-Goblin"));

            KnockBackEnemy(enemy, 1f);
        }
    }

    public void Attack2()//Attack funciton is linked to attack1 and attack2 event as animation event and trigger when anim is played
    {   
        foreach(Collider2D enemy in _hitArray2)
        {
            enemy.gameObject.GetComponent<NPCVitalityHandler>()
                .TakeDamage(attackDamage2, enemy.gameObject.CompareTag("Enemy-Goblin"));

            KnockBackEnemy(enemy, 1.3f);
        }
    }

    public void Attack3()//Attack funciton is linked to attack1 and attack2 event as animation event and trigger when anim is played
    {   
        foreach(Collider2D enemy in _hitArray3)
        {
            enemy.gameObject.GetComponent<NPCVitalityHandler>()
                .TakeDamage(attackDamage3, enemy.gameObject.CompareTag("Enemy-Goblin"));

            KnockBackEnemy(enemy, 1.5f);
        }
    }

    private async void KnockBackEnemy(Collider2D enemy, float multiplier)
    {
        if (enemy.gameObject.CompareTag("Enemy-Demon")) return;
        
        Vector2 difference = enemy.transform.position - ParentPlayer.transform.position;
        if(enemy.gameObject.CompareTag("Enemy-FlyingEye"))
            enemy.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.5f;
            
                
        enemy.gameObject.GetComponent<Rigidbody2D>().AddForce(70f * multiplier * difference.normalized, ForceMode2D.Impulse);
        enemy.gameObject.GetComponent<Rigidbody2D>().AddForce(enemy.transform.up * 80f * multiplier, ForceMode2D.Impulse);
        
            

        await Task.Delay(1000);
        if(enemy.gameObject.CompareTag("Enemy-FlyingEye"))
            enemy.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1f;



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
