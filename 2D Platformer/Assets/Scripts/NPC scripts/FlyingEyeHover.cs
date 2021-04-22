using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlyingEyeHover : MonoBehaviour
{
    public float hoverHeight;
    public float hoverForce = 10.0f;
    public float currentHeight = 0.0f;
    public LayerMask groundLayer;
    public float damping;
    private RaycastHit2D hit;
    
    void FixedUpdate()
    {
        hit = Physics2D.Raycast(this.gameObject.transform.position, -Vector2.up, hoverHeight, groundLayer);
        //hit.y = 0 if too high
        //hit.y != 0 ray touching the ground
        currentHeight = hit.distance;
        float heightError = hoverHeight - currentHeight;

        if((currentHeight < hoverHeight && currentHeight != 0) || (currentHeight == 0 && hit.collider != null))
        {
            float force = hoverForce * heightError - this.gameObject.GetComponent<Rigidbody2D>().velocity.y*damping;
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * force * 9.8f * this.gameObject.GetComponent<Rigidbody2D>().gravityScale);
        }
        

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector2 direction = this.gameObject.transform.TransformDirection(Vector2.down) * hoverHeight;
        Gizmos.DrawRay(this.gameObject.transform.position, direction);

        Gizmos.color = Color.red;
        try{
            Gizmos.DrawWireSphere(hit.collider.transform.position, 0.2f);
        }catch(Exception e)
        {
            ;
        }
        
        
    }
}

