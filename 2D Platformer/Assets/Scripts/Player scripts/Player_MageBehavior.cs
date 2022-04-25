using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Player_MageBehavior : PlayerBaseBehavior
{
    // public float speed; //movement speed (left and right)//7.5
    // public float jumpForce; //jump force (up)///8.5
    // public Animator animator;//getting animator to set conditions for animation transitions

    //Attack speed of player-archer
    public float elapsedTimeFireBall = 0, elapsedTimeIceBall = 0, attackIntervalFireBallSec, attackIntervalIceBallSec;
    //reference to parent player game obj
    // private PlayerBehavior parent_PlayerBehaviorScript;
    // private GameObject parent_Player;

    //shooting arrow mechanics configurations
    public GameObject fireBall_playerPrefab;
    public float directHitDamage, burnIntervalSec, totalBurnTime_Loop, burnDamage;

    public GameObject iceBall_playerPrefab;
    public float freezePeriod;

    public Transform shootingPoint;
    public int ballSpeed;
    public GameObject mageCoolDownBarFire, mageCoolDownbarIce;

    void Start(){
        //setup the damage of arrow from archer-player to the prefab
        var mageFireBall = fireBall_playerPrefab.GetComponent<FireBall_Player>();
        
        mageFireBall.burnInterval = burnIntervalSec;
        mageFireBall.burnDamage = burnDamage;
        mageFireBall.directHitDamage = directHitDamage;
        mageFireBall.totalBurnTime_Loop = totalBurnTime_Loop;

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
            Animator.SetTrigger(Attacks[0]); 
        }
        else if(num==2)
        {
            Animator.SetTrigger(Attacks[1]); 
        }
            
        if(ParentPlayerBehaviorScript.isGrounded)
        {
            ParentPlayerBehaviorScript.playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
            await Task.Delay(650);//so that player can't move while shooting arrow
            ParentPlayerBehaviorScript.playerRB.constraints = RigidbodyConstraints2D.None;
        }
    }

    public void Attack()//Attack funciton is linked to attack1 and attack2 event as animation event and trigger when anim is played
    {   
        GameObject fireBall = Instantiate(fireBall_playerPrefab, shootingPoint.position, Quaternion.identity);
        var facingRight = GetComponent<PlayerMovementAnimHandler>().facingRight;
        
        if(facingRight){
            fireBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.right * ballSpeed);     
        }
        else
        {
            fireBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.left * ballSpeed);
            fireBall.GetComponent<FireBall_Player>().Flip();
        }
    }

    public void Attack2()
    {
        GameObject iceBall = Instantiate(iceBall_playerPrefab, shootingPoint.position, Quaternion.identity);
        var facingRight = GetComponent<PlayerMovementAnimHandler>().facingRight;
        if(facingRight){
            iceBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.right * ballSpeed);     
        }
        else
        {
            iceBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.left * ballSpeed);
            iceBall.GetComponent<IceBall_Player>().Flip();
        }
    }
}
