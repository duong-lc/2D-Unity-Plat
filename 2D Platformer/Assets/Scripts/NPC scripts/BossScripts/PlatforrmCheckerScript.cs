using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlatforrmCheckerScript : MonoBehaviour
{
    [Serializable]
    public enum surfaceContact{
        LPlat, RPlat, Ground
    }
    [Serializable]
    public struct jumpPointHitCollider{
        public Transform jumpPointTransform;
        //public Collider2D collider;
        public surfaceContact surface;
        //public int isLeftSided;//this only applies to ground layer. -1 is left, 0 is null, 1 is right
    }

    [SerializeField] private BossScript boss; 
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Collider2D platformLeft, platformRight, ground;
    [SerializeField] private float jumpPointRadius;
    [SerializeField] private jumpPointHitCollider leftJumpPoint;
    [SerializeField] private jumpPointHitCollider rightJumpPoint;

    

    private surfaceContact playerSurface, bossSurface;
    

    private void Update()
    {
        UpdateCurrentTarget();
    }


    //player == ground
        //boss == L_PLat || boss == R_Plat
            //go to clostest ground jumppoint = findclosestjumppoint(ground)
    
    //player == L_Plat
        //boss lvl1
            //go to closest lvl1_left_jumppoint
            //force jump at that jumppoint
    
    //player == R_Plat
        //boss lvl1
            //go to closest lvl1_right_jumppoint
            //force jump at that jumppoint
    
    //---------------------------------------------------------------------------//
            
    //if player == left plat
        // if boss != left plat
            //goto left point, when reach, jump and follow player
            
    //if player == right plat
        //if boss != right plat
            //goto right point, when reach, jump and follow player

    //else
        //follow player
            
    private void UpdateCurrentTarget(/*int isLeftSided*/)
    {
        if (playerSurface == surfaceContact.LPlat)
        {
            if (bossSurface != surfaceContact.LPlat)
            {
                boss.currentTarget = leftJumpPoint.jumpPointTransform;
                print($"Hellow");
                UpdateShouldJump();
            }
            else
            {
                boss.currentTarget = playerPosition;
            }
        }
        else if (playerSurface == surfaceContact.RPlat)
        {
            if (bossSurface != surfaceContact.RPlat)
            {
                boss.currentTarget = rightJumpPoint.jumpPointTransform;
                UpdateShouldJump();
            }
            else
            {
                boss.currentTarget = playerPosition;
            }
        }
        else if (playerSurface == surfaceContact.Ground)
        {
            if (bossSurface == surfaceContact.LPlat)
            {
                boss.currentTarget = leftJumpPoint.jumpPointTransform;
            }
            else  if (bossSurface == surfaceContact.RPlat)
            {
                boss.currentTarget = rightJumpPoint.jumpPointTransform;
            }
        }
        else
        {
            boss.currentTarget = playerPosition;
        }
    }

    private void UpdateShouldJump()
    {
        var distance = Mathf.Abs(boss.transform.position.x - boss.currentTarget.position.x);
        print($"{distance}");
        if (distance <= 1f)//at that position
        {
            print($"hellowwwww");
            boss.Jump();
        }
    }
    
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            playerSurface = GetCurrentSurfaceContact(other);  
        }
        
    }
    
    //return current pos of boss based on collider it's in
    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.CompareTag("Enemy-Boss")){
            bossSurface = GetCurrentSurfaceContact(other);
        }
    }

    private surfaceContact GetCurrentSurfaceContact(Collider2D other){
        if(other.IsTouching(platformLeft)){
            return surfaceContact.LPlat;
        }
        else if (other.IsTouching(platformRight)){
            return surfaceContact.RPlat;
        }
        else{
            return surfaceContact.Ground;
        }
    }

    void OnDrawGizmosSelected()
    {    
        Gizmos.color = Color.red;
        // for(int i = 0; i < jumpPointArray.Length; i++)
        // {
        //     Gizmos.DrawWireSphere(jumpPointArray[i].jumpPointTransform.position, jumpPointRadius);
        // }
    }
}
