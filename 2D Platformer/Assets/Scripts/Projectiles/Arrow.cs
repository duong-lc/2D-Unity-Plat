using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**************************************************
 * Object(s) holding the script: 
 * Author(s): Le Chi Duong 
 * Date Made: 
 * Last Updated: 
 * Summary: 
 * **********************************************/

public class Arrow : MonoBehaviour
{
    public Transform attackZone;//center of attack box
    public Vector3 attackBox;//attack box where the arrow would deal damage
    //public LayerMask EnemiesLayer;
    private Collider2D[] hitArray;//collider 3d array of what the arrow have hit (updated every tick)
    public float attackDamage;//attack damage, 

    void Update()
    {   
        //initializing hit array every frame
        hitArray = Physics2D.OverlapBoxAll(attackZone.position, attackBox, 0f/*, EnemiesLayer*/);
        DamageEnemy();//update damaging enemy every tick with the array
    }

    /*
    *   For loop that checks for every encounter in the hit array
    *       - Damage enemy if arrow come in contact with one
    *       - is Destroyed if it hits solid ground
    *       - disappear after 3 seconds if doesn't come in contact with anything.
    */
    private void DamageEnemy()
    {   
        foreach(Collider2D contact in hitArray)
        {
            if(contact.gameObject.GetComponent<NPCVitalityHandler>()){
                if(contact.gameObject.tag == "Enemy-Skeleton" || contact.gameObject.tag == "Enemy-Goblin"){
                    contact.gameObject.GetComponent<NPCVitalityHandler>().TakeDamage(attackDamage, true);
                    Destroy(this.gameObject);
                }
                else if (contact.gameObject.tag == "Enemy-Boss"){
                    contact.gameObject.GetComponent<NPCVitalityHandler>().TakeDamage(attackDamage, true);
                    if(contact.gameObject.GetComponent<BossScript>().dodgeState != BossScript.avoidState.roll ||
                    contact.gameObject.GetComponent<NPCVitalityHandler>().isFrozen==true){
                        Destroy(this.gameObject);
                    }
                }
                else{
                    contact.gameObject.GetComponent<NPCVitalityHandler>().TakeDamage(attackDamage, false);
                    Destroy(this.gameObject);
                }
                    
                
            }
            else if (contact.gameObject.tag == "ground")
            {
                Destroy(this.gameObject);
            }
            
        }
        Destroy(this.gameObject, 3f);
    }
    
    void OnDrawGizmosSelected()//Drawing attack box for arrow
    {    
        if (attackZone == null)
        {
            return;
        }
        Gizmos.DrawWireCube(attackZone.position, attackBox);
        
    }
}
