using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalProjectileScript : MonoBehaviour
{
    [SerializeField] private GameObject impactParticle;
    public GameObject parentPortal;
    public float damage;
    public bool isReflected = false;

    private void Awake(){

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "ground" || other.gameObject.tag == "Player"){
            if(other.gameObject.tag == "Player" && isReflected == false){
                //damagePLayer
                other.gameObject.GetComponent<PlayerBehavior>().CallDamage(damage);
            }
            OnHit(0f, true);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("enemies") && other.gameObject.tag != "portalProjectile" && isReflected == true){
            //damage enemy
            Debug.Log("hit enemy");
            other.gameObject.GetComponent<NPCVitalityHandler>().TakeDamage(damage, false);
            OnHit(0f, true);
        }
        else if(other.gameObject.tag == "portal" && isReflected == true){
            other.gameObject.GetComponent<PortalSpecificBehavior>().DestroySelf();
            OnHit(0f, true);
        }
        OnHit(5f, false);
            
    }

    private void OnHit(float delay, bool isSpawnParticle){
        if(isSpawnParticle)
            Instantiate(impactParticle, transform.position, Quaternion.identity);

        Destroy(gameObject, delay);
        
    }

    
}
