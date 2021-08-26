using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class Player_KatanaBehavior : MonoBehaviour
{
        public float speed; //movement speed (left and right)
        public float jumpForce; //jump force (up)
        public Animator animator;//getting animator to set conditions for animation transitions

        //public bool facingRight = true; //bool value to determine the direction the player sprite is facing

        private int count = 1;//counter to cycle through attack animation
        //private bool isPlayerTakingDamage = false;//bool value to see if player taking damage

        //Vars for checking attack zone
        public Transform attackZone;//center position for attackzone
        public Vector3 attackBox;//dimension for attack box
        public LayerMask EnemiesLayer, portalProjectileLayer;//layer mask of enemies (editted in inspector)
        private Collider2D[] hitArray, hitArray2;//collider2d array of objects that are in the attackbox. The collider2d components of those objects are passed in
        public float attackDamage;//attack damage of katana player (editted in inspector)

        private float elapsedTime = 0;//time stamp since play for next attack to be valid to attack
        public float attackIntervalSec;//cooldown time between each attack of player

        private PlayerBehavior parent_PlayerBehaviorScript;//script of player object that is a parent of all 4 controllable characters 
        private GameObject parent_Player;//game object of parent player


        public GameObject instanceAfterImage;
        private GameObject instance;//temporary variable that is 
        private bool isDashing;
        public float dashTime, dashSpeed, dashCoolDown, ghostDelay;
        private float dashTimeLeft, lastDash = 0, ghostDelaySeconds;


        void Start(){
            parent_PlayerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();
            parent_Player = GameObject.Find("Player");

            ghostDelaySeconds = ghostDelay;
        }


        void Update(){
            hitArray = Physics2D.OverlapBoxAll(attackZone.position, attackBox, 0f, EnemiesLayer);
            hitArray2 = Physics2D.OverlapBoxAll(attackZone.position, attackBox, 0f, portalProjectileLayer);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {  
                if(Time.time > elapsedTime)
                {
                    PlayAttackAnim();
                    elapsedTime = Time.time + attackIntervalSec;
                }
            }

            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                if(Time.time >=(lastDash + dashCoolDown))
                    AttemptToDash();
            }
            CheckDash();
            

        }

        private void AttemptToDash()
        {
            isDashing = true;
            dashTimeLeft = dashTime;
            lastDash = Time.time;
        }

        private void CheckDash()
        {
            float direction = 1f;

            if(this.gameObject.GetComponent<PlayerMovementAnimHandler>().facingRight == true)
                direction = 1f;
            else if (this.gameObject.GetComponent<PlayerMovementAnimHandler>().facingRight == false)
                direction = -1f;


            if(isDashing)
            {
                if(dashTimeLeft > 0)
                {
                    parent_PlayerBehaviorScript.playerRB.velocity = new Vector2(dashSpeed * direction, parent_PlayerBehaviorScript.playerRB.velocity.y);
                    dashTimeLeft -= Time.deltaTime;

                    if (ghostDelaySeconds > 0)
                    {
                        ghostDelaySeconds -= Time.deltaTime;
                    }else{
                        
                        instance = Instantiate(instanceAfterImage, transform.position, Quaternion.identity);
                        instance.GetComponent<SpriteRenderer>().sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
                        if(this.gameObject.GetComponent<PlayerMovementAnimHandler>().facingRight == false)
                            instance.GetComponent<SpriteRenderer>().flipX = true;
                            
                        ghostDelaySeconds = ghostDelay;
                    }  
                }
                
                if(dashTimeLeft <= 0)
                {
                    isDashing = false;
                }
            }
        }

        void PlayAttackAnim()
        {      
            count++;
            if (count % 2 == 0)
            {
                animator.SetTrigger("Attack1"); 
            }
            else if (count % 2 != 0)
            {
                animator.SetTrigger("Attack2"); 
            }     
        }

        public void Attack()//Attack funciton is linked to attack1 and attack2 event as animation event and trigger when anim is played
        {   
            foreach(Collider2D enemy in hitArray)
            {
                if((enemy.gameObject.tag != "Enemy-Goblin" && enemy.gameObject.tag != "Enemy-Skeleton") 
                && enemy.gameObject.tag != "portalProjectile" && enemy.gameObject.tag != "Enemy-Boss")
                    enemy.gameObject.GetComponent<NPCVitalityHandler>().TakeDamage(attackDamage, false); 
                else//for enemies that can block/deflect attack frorm player
                {
                    enemy.gameObject.GetComponent<NPCVitalityHandler>().TakeDamage(attackDamage, true);
                }
                    
            
            }
            foreach(Collider2D enemy in hitArray2){
                if (enemy.gameObject.tag == "portalProjectile"){
                    //velocity = (ParentPortalPos - playerPos )*speedsetbyparentPortal
                    try{
                        enemy.gameObject.GetComponent<PortalProjectileScript>().isReflected = true;
                        Vector3 dir = enemy.gameObject.GetComponent<PortalProjectileScript>().parentPortal.transform.position - parent_PlayerBehaviorScript.gameObject.transform.position;
                        enemy.gameObject.GetComponent<Rigidbody2D>().velocity = (dir).normalized * enemy.gameObject.GetComponent<PortalProjectileScript>().parentPortal.GetComponent<PortalSpecificBehavior>().swordSpeed;
                        enemy.gameObject.transform.Rotate(new Vector3 (enemy.gameObject.transform.rotation.x, enemy.gameObject.transform .rotation.y, enemy.gameObject.transform.rotation.z+180), Space.Self);
                        enemy.gameObject.GetComponent<ProjectileIndicator>().DeactivateIndicator();
                    }
                    catch(Exception e){
                        ;
                    }
                }
            }
        }


        void OnDrawGizmosSelected()
        {    
            if (attackZone == null)
            {
                return;
            }
            Gizmos.DrawWireCube(attackZone.position, attackBox);
            
        }
}
