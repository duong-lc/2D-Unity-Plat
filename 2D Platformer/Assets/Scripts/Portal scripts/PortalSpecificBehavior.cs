using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpecificBehavior : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float attackIntervalSec;
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private GameObject particleOnDeath;
    public float swordSpeed;
    private float elapsedTime = 0;
    public float damageToPlayer = 0;
    
    private void Awake(){
        player =  GameObject.FindWithTag("Player").transform;
        elapsedTime = Time.time + elapsedTime+3;
    }

    private void Update()
    {
        LookAtPlayer(this.gameObject);
        FireProjectile();
    }

    private void FireProjectile(){
        if(Time.time > elapsedTime)
        {   
            GameObject sword = Instantiate(swordPrefab, transform.position, Quaternion.identity);
            var proj = sword.GetComponent<PortalProjectileScript>();
            
            proj.parentPortal = gameObject;
            proj.damage = damageToPlayer;
            
            proj.OnHit(10.0f, false);
            
            sword.transform.rotation = transform.rotation;
            sword.transform.Rotate(new Vector3 (sword.transform.rotation.x, sword.transform .rotation.y, sword.transform.rotation.z+90), Space.Self);

            
            sword.GetComponent<Rigidbody2D>().velocity = (player.position - transform.position).normalized * swordSpeed;
                
            
            //Debug.Log("Attack");
            elapsedTime = Time.time + attackIntervalSec;
        }
    }

    private void LookAtPlayer(GameObject objectToLookAtPlayer){

        Vector3 dir = player.position - objectToLookAtPlayer.transform.position; 
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg; 
        transform.rotation = Quaternion.AngleAxis(angle -180, Vector3.forward);
    }
    private void SwitchToIdle(){
        gameObject.GetComponent<Animator>().SetBool("isIdle", true);
    }

    public void DestroySelf(){
        Instantiate(particleOnDeath, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
