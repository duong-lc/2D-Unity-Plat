using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Player_ArcherBehavior : Player_BaseBehavior
{
    
        // public float speed; //movement speed (left and right)//8
        // public float jumpForce; //jump force (up)//10.5
        // public Animator animator;//getting animator to set conditions for animation transitions
        //private bool facingRight = true; //bool value to determine the direction the player sprite is facing

        //Attack speed of player-archer
        private float elapsedTime = 0;
        public float attackIntervalSec;

        //reference to parent player game obj
        // private PlayerBehavior parent_PlayerBehaviorScript;
        // private GameObject parent_Player;

        //shooting arrow mechanics configurations
        public GameObject arrowPrefab;
        public Transform shootingPoint;
        public int arrowSpeed;
        public float arrowDamage;

        void Start(){
            //referencing the variables
            parent_PlayerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();
            parent_Player = GameObject.Find("Player");
            //setup the damage of arrow from archer-player to the prefab
            arrowPrefab.GetComponent<Arrow>().attackDamage = arrowDamage;
        }

        void Update()
        {
            //Attack mechanic of player-archer
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {  
                if(Time.time > elapsedTime)//regulate attack speed of the player
                {
                    PlayAttackAnim();
                    elapsedTime = Time.time + attackIntervalSec;
                }
            }   
        }
        
        async void PlayAttackAnim()
        {      
            animator.SetTrigger("Attack");   
            if(parent_PlayerBehaviorScript.isGrounded == true){
                parent_PlayerBehaviorScript.isShootingArrow = true;
                await Task.Delay(650);//so that player can't move while shooting arrow
                parent_PlayerBehaviorScript.isShootingArrow = false;
            }
        }

        public void Attack()//Attack funciton is linked to attack1 and attack2 event as animation event and trigger when anim is played
        {   
            GameObject arrow = Instantiate(arrowPrefab, shootingPoint.position, Quaternion.identity);

            if(this.gameObject.GetComponent<PlayerMovementAnimHandler>().facingRight == true){
                arrow.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.right * arrowSpeed);
            }else if (this.gameObject.GetComponent<PlayerMovementAnimHandler>().facingRight == false){
                arrow.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.left * arrowSpeed);
                arrow.GetComponent<SpriteRenderer>().flipX = true;
            }
        }

}
