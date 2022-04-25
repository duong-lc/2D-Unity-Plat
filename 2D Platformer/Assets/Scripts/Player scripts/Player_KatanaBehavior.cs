using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class Player_KatanaBehavior : PlayerBaseBehavior
{
        // public float speed; //movement speed (left and right)//7.5
        // public float jumpForce; //jump force (up)//9.5
        // public Animator animator;//getting animator to set conditions for animation transitions

        private int _count = 1;//counter to cycle through attack animation
        //private bool isPlayerTakingDamage = false;//bool value to see if player taking damage
        private PlayerMovementAnimHandler MovementComponent => GetComponent<PlayerMovementAnimHandler>();
        
        //Vars for checking attack zone
        public Transform attackZone;//center position for attackzone
        public Vector3 attackBox;//dimension for attack box
        public LayerMask EnemiesLayer, portalProjectileLayer;//layer mask of enemies (editted in inspector)
        private Collider2D[] _hitArray, _hitArray2;//collider2d array of objects that are in the attackbox. The collider2d components of those objects are passed in
        public float attackDamage;//attack damage of katana player (editted in inspector)

        private float _elapsedTime = 0;//time stamp since play for next attack to be valid to attack
        public float attackIntervalSec;//cooldown time between each attack of player

        // private PlayerBehavior parent_PlayerBehaviorScript;//script of player object that is a parent of all 4 controllable characters 
        // private GameObject parent_Player;//game object of parent player


        public GameObject instanceAfterImage;
        private GameObject _instance;//temporary variable that is 
        private bool isDashing;
        public float dashTime, dashSpeed, dashCoolDown, ghostDelay;
        private float dashTimeLeft, lastDash = 0, ghostDelaySeconds;


        void Start(){
            ghostDelaySeconds = ghostDelay;
        }


        void Update(){
            _hitArray = Physics2D.OverlapBoxAll(attackZone.position, attackBox, 0f, EnemiesLayer);
            _hitArray2 = Physics2D.OverlapBoxAll(attackZone.position, attackBox, 0f, portalProjectileLayer);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {  
                if(Time.time > _elapsedTime)
                {
                    PlayAttackAnim();
                    _elapsedTime = Time.time + attackIntervalSec;
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

            if(MovementComponent.facingRight)
                direction = 1f;
            else
                direction = -1f;


            if(isDashing)
            {
                if(dashTimeLeft > 0)
                {
                    ParentPlayerBehaviorScript.playerRB.velocity = new Vector2(dashSpeed * direction, ParentPlayerBehaviorScript.playerRB.velocity.y);
                    dashTimeLeft -= Time.deltaTime;

                    if (ghostDelaySeconds > 0)
                    {
                        ghostDelaySeconds -= Time.deltaTime;
                    }else{
                        
                        _instance = Instantiate(instanceAfterImage, transform.position, Quaternion.identity);
                        _instance.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                        if(MovementComponent.facingRight == false)
                            _instance.GetComponent<SpriteRenderer>().flipX = true;
                            
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
            _count++;
            if (_count % 2 == 0)
            {
                //Animator.SetTrigger("Attack1"); 
                Animator.SetTrigger(Attacks[0]); 
            }
            else if (_count % 2 != 0)
            {
                //Animator.SetTrigger("Attack2"); 
                Animator.SetTrigger(Attacks[1]); 
            }     
        }

        public void Attack()//Attack funciton is linked to attack1 and attack2 event as animation event and trigger when anim is played
        {   
            foreach(Collider2D enemy in _hitArray)
            {
                if((!enemy.gameObject.CompareTag("Enemy-Goblin") && !enemy.gameObject.CompareTag("Enemy-Skeleton")) 
                && !enemy.gameObject.CompareTag("portalProjectile") && !enemy.gameObject.CompareTag("Enemy-Boss"))
                    enemy.gameObject.GetComponent<NPCVitalityHandler>().TakeDamage(attackDamage, false); 
                else//for enemies that can block/deflect attack frorm player
                {
                    enemy.gameObject.GetComponent<NPCVitalityHandler>().TakeDamage(attackDamage, true);
                }
                    
            
            }
            foreach(Collider2D enemy in _hitArray2)
            {
                var obj = enemy.gameObject;
                if (obj.CompareTag("portalProjectile")){
                    try{
                        obj.GetComponent<PortalProjectileScript>().isReflected = true;
                        Vector3 dir = enemy.gameObject.GetComponent<PortalProjectileScript>().parentPortal.transform.position - ParentPlayerBehaviorScript.gameObject.transform.position;
                        obj.GetComponent<Rigidbody2D>().velocity = (dir).normalized * enemy.gameObject.GetComponent<PortalProjectileScript>().parentPortal.GetComponent<PortalSpecificBehavior>().swordSpeed;
                        var objRot = obj.transform.rotation;
                        obj.transform.Rotate(new Vector3 (objRot.x, objRot.y, objRot.z+180), Space.Self);
                        obj.GetComponent<ProjectileIndicator>().DeactivateIndicator();
                    }
                    catch(Exception e){
                        ;
                    }
                }
            }
        }

        // public override void OnCharacterDeath()
        // {
        //     DeathDelay();
        //     ParentPlayerBehaviorScript.isKatanaAlive = false;
        //     MovementComponent.enabled = false;
        //     this.enabled = false;
        // }
        
        void OnDrawGizmosSelected()
        {    
            if (attackZone == null)
            {
                return;
            }
            Gizmos.DrawWireCube(attackZone.position, attackBox);
            
        }
}
