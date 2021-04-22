using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall_Player : MonoBehaviour
{
    public Animator animator;

    public Transform anchor;//center of attack box
    public float freezeRadius;//attack box where the arrow would deal damage
    public float colliderRadius;
    public LayerMask EnemyLayer;
    private Collider2D[] subjectsToBePurgedArray;
    private Collider2D Collider;

    public float freezePeriod;

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
        subjectsToBePurgedArray = Physics2D.OverlapCircleAll(anchor.position, freezeRadius, EnemyLayer);
        Collider = Physics2D.OverlapCircle(anchor.position, colliderRadius);

        if(Collider != null && Collider.gameObject.tag != "_TutCollider" && Collider.gameObject.tag != "PickUp-Heavy" && Collider.gameObject.tag != "PickUp-Health" && Collider.gameObject.tag != "PickUp-Mage")
        {
            animator.SetTrigger("Impact");
            this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void DoYouWantToBuildASnowMan()//ref in animation
    {
        foreach(Collider2D enemy in subjectsToBePurgedArray)
        {
            if (enemy.tag == "Enemy-FireWorm"){
                enemy.GetComponent<NPC_Enemy_FireWormBehavior>().Take_Spell_Frozen(freezePeriod);
                Destroy(this.gameObject);
            }   
            else if(enemy.tag == "Enemy-Skeleton"){
                enemy.GetComponent<NPC_EnemyBehavior>().Take_Spell_Frozen(freezePeriod);
                Destroy(this.gameObject);
            }
            else if(enemy.tag == "Enemy-Goblin"){
                enemy.GetComponent<NPC_Enemy_GoblinBehavior>().Take_Spell_Frozen(freezePeriod);
                Destroy(this.gameObject);
            }
            else if(enemy.tag == "Enemy-Slime"){
                enemy.GetComponent<NPC_Enemy_SlimeBehavior>().Take_Spell_Frozen(freezePeriod);
                Destroy(this.gameObject);
            }
            else if (enemy.tag == "Enemy-FlyingEye"){
                enemy.GetComponent<NPC_Enemy_FlyingEyeBehavior>().Take_Spell_Frozen(freezePeriod);
                Destroy(this.gameObject);
            }
            else if (enemy.gameObject.tag == "Enemy-Mushroom")
            {
                enemy.GetComponent<NPC_Enemy_MushroomBehavior>().Take_Spell_Frozen(freezePeriod);
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

     void OnDrawGizmosSelected()//Drawing attack box for arrow
    {    
        if (anchor == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(anchor.position, freezeRadius);   
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(anchor.position, colliderRadius);   
    }
}
