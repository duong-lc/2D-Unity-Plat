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
    private Collider2D[] Collider;

    public float freezePeriod;

    // Update is called once per frame
    void Update()
    {
        subjectsToBePurgedArray = Physics2D.OverlapCircleAll(anchor.position, freezeRadius, EnemyLayer);
        Collider = Physics2D.OverlapCircleAll(anchor.position, colliderRadius);

        foreach (Collider2D col in Collider)
        {
            var canImpact = col != null && !col.gameObject.CompareTag("_TutCollider") &&
                            !col.gameObject.CompareTag("PickUp-Heavy") &&
                            !col.gameObject.CompareTag("PickUp-Health") &&
                            !col.gameObject.CompareTag("PickUp-Mage") &&
                            !col.gameObject.CompareTag("ignoreCol");
            
            if(canImpact)
            {
                animator.SetTrigger("Impact");
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }

    private void DoYouWantToBuildASnowMan()//ref in animation
    {
        foreach(Collider2D enemy in subjectsToBePurgedArray)
        {
            if(enemy.gameObject.GetComponent<NPCVitalityHandler>()){
                enemy.gameObject.GetComponent<NPCVitalityHandler>().Take_Spell_Frozen(freezePeriod);
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
