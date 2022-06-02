    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileIndicator : MonoBehaviour
{
    public GameObject indicator;
    [SerializeField] private GameObject target;
    [SerializeField] private LayerMask hitLayer;

    Renderer rd;
    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindWithTag("Player");
        rd = GetComponent<Renderer>();
    }

    // Update is called once per frame  
    void Update()
    {
        //ensuring when the projectile goes past the player, the indicator won't get turned on on the screen
        if(Mathf.Floor(transform.position.x) == Mathf.Floor(target.transform.position.x)){
            DeactivateIndicator();
        }

        if(rd.isVisible == false){
            if(indicator.activeSelf == false){
                indicator.SetActive(true);
            }
            Vector2 dir = target.transform.position - transform.position;
            RaycastHit2D ray = Physics2D.Raycast(transform.position, dir, 500, hitLayer);
            if(ray.collider != null){
                indicator.transform.position = ray.point;
            }
        }
        else{
            if(indicator.activeSelf == true){
                indicator.SetActive(false);
            }
        }
    }

    public void DeactivateIndicator() {
        indicator.SetActive(false);
        this.enabled = false;
    }
}
