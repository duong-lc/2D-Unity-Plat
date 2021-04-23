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

        public bool facingRight = true; //bool value to determine the direction the player sprite is facing

        private int count = 1;//counter to cycle through attack animation
        private bool isPlayerTakingDamage = false;//bool value to see if player taking damage

        //Vars for checking attack zone
        public Transform attackZone;//center position for attackzone
        public Vector3 attackBox;//dimension for attack box
        public LayerMask EnemiesLayer;//layer mask of enemies (editted in inspector)
        private Collider2D[] hitArray;//collider2d array of objects that are in the attackbox. The collider2d components of those objects are passed in
        public float attackDamage;//attack damage of katana player (editted in inspector)

        public float currentHealth;//current health of katana player
        public float maxHealth;//max health of katana player (editted in inspector)
        public HealthBar healthBar;//healthbar script of healthbar object attached to katana player

        private float elapsedTime = 0;//time stamp since play for next attack to be valid to attack
        public float attackIntervalSec;//cooldown time between each attack of player

        private PlayerBehavior parent_PlayerBehaviorScript;//script of player object that is a parent of all 4 controllable characters 
        private GameObject parent_Player;//game object of parent player


        public GameObject instanceAfterImage;//
        private GameObject instance;//temporary variable that is 
        private bool isDashing;
        public float dashTime;
        public float dashSpeed;
        public float dashCoolDown;
        //public float distanceBetweenImages;

        private float dashTimeLeft;
        private float lastDash = 0;
        //private float lastImageXpos;
        public float ghostDelay;
        private float ghostDelaySeconds;

        void Start(){
            parent_PlayerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();
            parent_Player = GameObject.Find("Player");

            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            ghostDelaySeconds = ghostDelay;
        }


        void Update(){
            hitArray = Physics2D.OverlapBoxAll(attackZone.position, attackBox, 0f, EnemiesLayer);
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

            //PlayerAfterImagePool.Instance.GetFromPool();
            //lastImageXpos = parent_Player.transform.position.x;
        }

        private void CheckDash()
        {
            float direction = 1f;

            if(facingRight == true)
                direction = 1f;
            else if (facingRight == false)
                direction = -1f;

            /*Debug.Log(isDashing);
            Debug.Log(dashTimeLeft);
            Debug.Log(direction);*/

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
                        if(facingRight == false)
                            instance.GetComponent<SpriteRenderer>().flipX = true;
                            
                        ghostDelaySeconds = ghostDelay;
                    }  
                    /*if (Mathf.Abs(parent_Player.transform.position.x - lastImageXpos) > distanceBetweenImages)
                    {
                        PlayerAfterImagePool.Instance.GetFromPool();
                        lastImageXpos = parent_Player.transform.position.x;
                    }*/
                }
                
                if(dashTimeLeft <= 0)
                {
                    isDashing = false;
                }
            }
        }

        public async void TakingDamage(float damageTaken)
        {
            
            isPlayerTakingDamage = true;
            animator.SetBool("isTakeHit", true);
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            await Task.Delay(300);
            animator.SetBool("isTakeHit", false);
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            isPlayerTakingDamage = false;
            

            currentHealth -= damageTaken;

            healthBar.SetHealth(currentHealth);

            if (currentHealth <= 0)
            {
                Death();
            }
        }

        public void Heals(float amount)
        {
            currentHealth += amount;
            if(currentHealth > maxHealth)
                currentHealth = maxHealth;
            healthBar.SetHealth(currentHealth);
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
                if(enemy.gameObject.tag == "Enemy-Skeleton")
                    enemy.GetComponent<NPC_EnemyBehavior>().TakeDamage(attackDamage, true);
                else if(enemy.gameObject.tag == "Enemy-FireWorm")
                    enemy.GetComponent<NPC_Enemy_FireWormBehavior>().TakeDamage(attackDamage);
                else if(enemy.gameObject.tag == "Enemy-Goblin")
                    enemy.GetComponent<NPC_Enemy_GoblinBehavior>().TakeDamage(attackDamage, true);
                else if (enemy.gameObject.tag == "Enemy-Slime")
                    enemy.GetComponent<NPC_Enemy_SlimeBehavior>().TakeDamage(attackDamage);
                else if (enemy.gameObject.tag == "Enemy-FlyingEye")
                    enemy.GetComponent<NPC_Enemy_FlyingEyeBehavior>().TakeDamage(attackDamage);
                else if (enemy.gameObject.tag == "Enemy-Mushroom")
                    enemy.GetComponent<NPC_Enemy_MushroomBehavior>().TakeDamage(attackDamage);
                else if (enemy.gameObject.tag == "Enemy-Demon")
                {
                    Debug.Log("lmaoxd");
                    enemy.GetComponent<NPC_Enemy_DemonBehavior>().TakeDamage(attackDamage);
                }
                    
            
            }
        }

        async public void Death()
        {
            currentHealth = 0;
            animator.SetTrigger("Die");
            parent_PlayerBehaviorScript.isKatanaAlive = false;

            parent_PlayerBehaviorScript.playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            parent_Player.GetComponent<Collider2D>().enabled = false;
            await Task.Delay(2500);//finish playing death animation
            parent_PlayerBehaviorScript.playerRB.constraints = RigidbodyConstraints2D.None;
            parent_Player.GetComponent<Collider2D>().enabled = true;
            parent_PlayerBehaviorScript.AliveList[0] = false;
            parent_PlayerBehaviorScript.SwitchToAlive();
            

            this.enabled = false;   
        }

        public void PlayerRunning()
        {
            animator.SetFloat("Speed", Mathf.Abs(parent_PlayerBehaviorScript.moveInput));

            //Flip player facing direction based on key pressing
            if (facingRight == false && parent_PlayerBehaviorScript.moveInput > 0){
                Flip();
                healthBar.Flip();
            }else if (facingRight == true && parent_PlayerBehaviorScript.moveInput < 0){
                Flip();
                healthBar.Flip();
            }   
        }

        public void PlayerJumping()
        {   
            if (isPlayerTakingDamage == false){
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
            else if (isPlayerTakingDamage == true){
                animator.SetBool("IsFalling", false);
                animator.SetBool("IsJumping", false);
            }
            
        }

        void Flip(){
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
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
