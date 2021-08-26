using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Player_MageBehavior : MonoBehaviour
{
    public float speed; //movement speed (left and right)
    public float jumpForce; //jump force (up)
    public Animator animator;//getting animator to set conditions for animation transitions

    //Attack speed of player-archer
    public float elapsedTimeFireBall = 0, elapsedTimeIceBall = 0, attackIntervalFireBallSec, attackIntervalIceBallSec;
    //reference to parent player game obj
    private PlayerBehavior parent_PlayerBehaviorScript;
    private GameObject parent_Player;

    //shooting arrow mechanics configurations
    public GameObject fireBall_playerPrefab;
    public float directHitDamage, burnIntervalSec, totalBurnTime_Loop, burnDamage;

    public GameObject iceBall_playerPrefab;
    public float freezePeriod;

    public Transform shootingPoint;
    public int ballSpeed;
    public GameObject mageCoolDownBarFire, mageCoolDownbarIce;

    void Start(){
        //referencing the variables
        parent_PlayerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        parent_Player = GameObject.Find("Player");
        //setup the damage of arrow from archer-player to the prefab
        fireBall_playerPrefab.GetComponent<FireBall_Player>().burnInterval = burnIntervalSec;
        fireBall_playerPrefab.GetComponent<FireBall_Player>().burnDamage = burnDamage;
        fireBall_playerPrefab.GetComponent<FireBall_Player>().directHitDamage = directHitDamage;
        fireBall_playerPrefab.GetComponent<FireBall_Player>().totalBurnTime_Loop = totalBurnTime_Loop;

        iceBall_playerPrefab.GetComponent<IceBall_Player>().freezePeriod = freezePeriod;
    }

    void Update()
    {
        //Attack mechanic of player-archer
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {  
            if(Time.time > elapsedTimeFireBall)//regulate attack speed of the player
            {
                PlayAttackAnim(1);
                elapsedTimeFireBall = Time.time + attackIntervalFireBallSec;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {  
            if(Time.time > elapsedTimeIceBall)//regulate attack speed of the player
            {
                PlayAttackAnim(2);
                elapsedTimeIceBall = Time.time + attackIntervalIceBallSec;
            }
        }

    }

    async void PlayAttackAnim(int num)
    {   
        if(num==1)
        {
            animator.SetTrigger("Attack1"); 
        }
        else if(num==2)
        {
            animator.SetTrigger("Attack2"); 
        }
            
        if(parent_PlayerBehaviorScript.isGrounded == true){
            parent_PlayerBehaviorScript.playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
            await Task.Delay(650);//so that player can't move while shooting arrow
            parent_PlayerBehaviorScript.playerRB.constraints = RigidbodyConstraints2D.None;
        }
    }

    public void Attack()//Attack funciton is linked to attack1 and attack2 event as animation event and trigger when anim is played
    {   
        GameObject fireBall = Instantiate(fireBall_playerPrefab, shootingPoint.position, Quaternion.identity);

        if(this.gameObject.GetComponent<PlayerMovementAnimHandler>().facingRight == true){
            fireBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.right * ballSpeed);     
        }else if (this.gameObject.GetComponent<PlayerMovementAnimHandler>().facingRight == false){
            fireBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.left * ballSpeed);
            fireBall.GetComponent<FireBall_Player>().Flip();
        }
    }

    public void Attack2()
    {
        GameObject iceBall = Instantiate(iceBall_playerPrefab, shootingPoint.position, Quaternion.identity);

        if(this.gameObject.GetComponent<PlayerMovementAnimHandler>().facingRight == true){
            iceBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.right * ballSpeed);     
        }else if (this.gameObject.GetComponent<PlayerMovementAnimHandler>().facingRight == false){
            iceBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.left * ballSpeed);
            iceBall.GetComponent<IceBall_Player>().Flip();
        }
    }
}
