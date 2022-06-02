using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGeneralBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject portal;
    [SerializeField] private Vector2[] portalArray;
    

    private void Awake() {
        // for (int i = 0; i < portalArray.Length; i++){
        //     portalArray[i].x += transform.position.x;
        //     portalArray[i].y += transform.position.y;
        // }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        gameObject.GetComponent<Collider2D>().enabled = false;
        //portal.SetActive(true);
        SpawnPortal();
       
    }

    public void SpawnPortal(){
        
        foreach(Transform child in transform){
            if(child != null){
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < portalArray.Length; i++){
            print($"{portalArray[i]}'");
            Instantiate(portal, portalArray[i], Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        for (int i = 0; i < portalArray.Length; i++){
            Gizmos.DrawWireSphere(portalArray[i], 0.2f);
        }
    }
}
