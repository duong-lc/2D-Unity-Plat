using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall_Player : MonoBehaviour
{
    public Animator animator;

    public Transform anchor;//center of attack box
    public float burnRadius;//attack box where the arrow would deal damage
    public float colliderRadius;
    public LayerMask EnemyLayer;
    private Collider2D[] subjectsToBePurgedArray;
    private Collider2D Collider;
    public float directHitDamage;//attack damage

    public float totalBurnTime_Loop;
    public float burnInterval;
    public float burnDamage;

    private bool callOnce;
    private bool callOnce_1;

    // Start is called before the first frame update
    void Start()
    {
        callOnce = true;
        callOnce_1 = true;
    }

    // Update is called once per frame
    void Update()
    {
        subjectsToBePurgedArray = Physics2D.OverlapCircleAll(anchor.position, burnRadius, EnemyLayer);
        Collider = Physics2D.OverlapCircle(anchor.position, colliderRadius);

        if(Collider != null && Collider.gameObject.tag != "_TutCollider" && Collider.gameObject.tag != "PickUp-Heavy" && Collider.gameObject.tag != "PickUp-Health" && Collider.gameObject.tag != "PickUp-Mage")
        {
            //Directhit damaging
            if(callOnce_1 == true){
                DirectHit();
            } 
            animator.SetTrigger("Explosion");
            this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            if (callOnce == true){
                ScaleUp();
            }    
        }
    }

    private void DirectHit(){
        if (Collider.tag == "Enemy-FireWorm")
            Collider.GetComponent<NPC_Enemy_FireWormBehavior>().TakeDamage(directHitDamage);
        else if(Collider.tag == "Enemy-Skeleton")
            Collider.GetComponent<NPC_EnemyBehavior>().TakeDamage(directHitDamage, false);
        else if(Collider.tag == "Enemy-Goblin")
            Collider.GetComponent<NPC_Enemy_GoblinBehavior>().TakeDamage(directHitDamage, false);
        else if(Collider.tag == "Enemy-Slime")
            Collider.GetComponent<NPC_Enemy_SlimeBehavior>().TakeDamage(directHitDamage);
        else if (Collider.gameObject.tag == "Enemy-FlyingEye")
            Collider.GetComponent<NPC_Enemy_FlyingEyeBehavior>().TakeDamage(directHitDamage);
        else if (Collider.gameObject.tag == "Enemy-Mushroom")
            Collider.GetComponent<NPC_Enemy_MushroomBehavior>().TakeDamage(directHitDamage);
        
        callOnce_1 = false;
    }

    private void BlessingFromTheOrthodoxChurch()//ref in animation
    {
        foreach(Collider2D enemy in subjectsToBePurgedArray)
        {
            if (enemy.tag == "Enemy-FireWorm"){
                enemy.GetComponent<NPC_Enemy_FireWormBehavior>().Take_Spell_Damage_Burn(burnInterval, burnDamage, totalBurnTime_Loop);
                Destroy(this.gameObject);
            }   
            else if(enemy.tag == "Enemy-Skeleton"){
                enemy.GetComponent<NPC_EnemyBehavior>().Take_Spell_Damage_Burn(burnInterval, burnDamage, totalBurnTime_Loop);
                Destroy(this.gameObject);
            }
            else if(enemy.tag == "Enemy-Goblin"){
                enemy.GetComponent<NPC_Enemy_GoblinBehavior>().Take_Spell_Damage_Burn(burnInterval, burnDamage, totalBurnTime_Loop);
                Destroy(this.gameObject);
            }
            else if(enemy.tag == "Enemy-Slime"){
                enemy.GetComponent<NPC_Enemy_SlimeBehavior>().Take_Spell_Damage_Burn(burnInterval, burnDamage, totalBurnTime_Loop);
                Destroy(this.gameObject);
            }
            else if(enemy.tag == "Enemy-Mushroom"){
                enemy.GetComponent<NPC_Enemy_MushroomBehavior>().Take_Spell_Damage_Burn(burnInterval, burnDamage, totalBurnTime_Loop);
                Destroy(this.gameObject);
            }
                
        }
        Destroy(this.gameObject);
    }
        
    public void Flip(){
          //flip the gameobject on x axis
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        this.gameObject.transform.localScale = Scaler;
        
    }

    private void ScaleUp(){
        Vector3 Scaler = transform.localScale;
        Scaler.x *= 2f;
        Scaler.y *= 2f;
        this.gameObject.transform.localScale = Scaler;
        callOnce = false;
    }

    void OnDrawGizmosSelected()//Drawing attack box for arrow
    {    
        if (anchor == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(anchor.position, burnRadius);   
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(anchor.position, colliderRadius);   
    }
}
