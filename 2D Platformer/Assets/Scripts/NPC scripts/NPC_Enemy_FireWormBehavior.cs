using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class NPC_Enemy_FireWormBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator animator;

    public Transform player;
	private bool isFlipped = true;
    public float runSpeed;
    private bool isStagger = false;

    public Transform attackZone;
    public Vector2 attackBox;
    public LayerMask PlayerLayer;
    private Collider2D playerCollider;

    //Health system and health UI
    public float currentHealth;
    public float maxHealth;
    public HealthBar healthBar;

    //Attack speed
    public float attackIntervalSec;
    private float elapsedTime = 0;
    private float attackDamage = 25;

    private PlayerBehavior playerBehaviorScript;
    private Player_KatanaBehavior player_KatanaBehaviorScript;
    private Player_ArcherBehavior player_ArcherBehaviorScript;

    public GameObject fireBallPrefab;
    private GameObject fireBall;
    public Transform shootingPoint;
    public int fireBallSpeed;
    public float fireBallDamage;

    private bool isDead;
    private bool isFrozen;
    private bool keepMoving = false;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();

        fireBallPrefab.GetComponent<FireBall>().attackDamage = fireBallDamage;

        rb = this.gameObject.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;//freezing rotation

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        playerCollider = Physics2D.OverlapBox(attackZone.position, attackBox, 0f, PlayerLayer);

        if (isStagger == false && isFrozen == false && isDead == false)
        {
            LookAtPlayer();
            FollowAndAttackPlayer();
        }

        if (isFrozen ==true){
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }

    public void Attack()
    {
        fireBall = Instantiate(fireBallPrefab, shootingPoint.position, Quaternion.identity);

        if(isFlipped == true){
            fireBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.right * fireBallSpeed);
        }else if (isFlipped == false){
            fireBall.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.left * -fireBallSpeed);
            fireBall.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    void AttackPlayerAnim()
    {
        if (playerCollider != null)
        {
            keepMoving = false;
            animator.SetTrigger("Attack");
        }
        else if (playerCollider == null)
        {
            keepMoving = true;
        }
    }

    void FollowAndAttackPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) >= attackBox.x || keepMoving == true)//Keep closing in until player in attack zone
        {
            transform.position += transform.right * runSpeed * Time.deltaTime;
            animator.SetBool("isWalking", true);
        }
        if (Vector2.Distance(transform.position, player.position) < attackBox.x)//if player is in, attack animation
        {
            if(Time.time > elapsedTime)
            {
                AttackPlayerAnim();
                elapsedTime = Time.time + attackIntervalSec;
            }
            
            animator.SetBool("isWalking", false);
        }
        
    }

    void LookAtPlayer()
	{
		Vector3 flipped = transform.localScale;
		flipped.z *= -1f;

		if (transform.position.x > player.position.x && isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = !isFlipped;
            healthBar.Flip();
		}
		else if (transform.position.x < player.position.x && !isFlipped)
		{
			transform.localScale = flipped;
			transform.Rotate(0f, 180f, 0f);
			isFlipped = !isFlipped;
            healthBar.Flip();
		}
	}

    public async void TakeDamage(float DamageTaken)
    {
        if(isDead == false)
        {
            isStagger = true;
            currentHealth -= DamageTaken;
            animator.SetBool("isWalking", false);
            animator.SetTrigger("TakeHit");

            if(isFrozen == false)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                await Task.Delay(300);
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else if (isFrozen == true)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
            }
            

            healthBar.SetHealth(currentHealth);

            await Task.Delay(500);
            isStagger = false; 
        }
        
        if (currentHealth <= 0)
        {
            healthBar.gameObject.SetActive(false);
            Death();
        }
        
    }

    void Death()
    {
        isDead = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isDead", true);
        //Debug.Log("dead");

        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        GetComponent<Collider2D>().enabled = false;
        
        isStagger = true;
        Destroy(this.gameObject, 5f);
    }

    void OnDrawGizmosSelected()
    {    
        if (attackZone == null)
        {
            return;
        }
        Gizmos.DrawWireCube(attackZone.position, attackBox);
    }

    public void Take_Spell_Damage_Burn(float burnInterval, float burnDamage, float totalBurnTime_Loop){
        StartCoroutine(WaitAndBurn(burnInterval, burnDamage, totalBurnTime_Loop));
    }

    async public void Take_Spell_Frozen(float freezePeriod){
        //StartCoroutine(WaitAndFreeze(freezePeriod));    

        isStagger = true;
        isFrozen = true;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        animator.speed = 0.001f;

        await Task.Delay((int)(freezePeriod*1000f));

        isStagger = false;
        isFrozen = false;
        animator.speed = 1f;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    //WaitAndFreeze is inconsistent and seemed to be canceled out by fireball-player attack which is strange
    //so I ended up using task.delay which uses system counter instead
    /*private IEnumerator WaitAndFreeze(float freezePeriod){

        isStagger = true;
        isFrozen = true;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        animator.speed = 0.001f;

        yield return new WaitForSeconds(freezePeriod);

        isStagger = false;
        isFrozen = false;
        animator.speed = 1f;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
    */
    private IEnumerator WaitAndBurn(float burnInterval, float burnDamage, float totalBurnTime_Loop)
    {
        for(float i = 0; i < totalBurnTime_Loop; i++)
        {
            TakeDamage(burnDamage);
            yield return new WaitForSeconds(burnInterval);
        }
    }
}
