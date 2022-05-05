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
        public int isLeftSided;//this only applies to ground layer. -1 is left, 0 is null, 1 is right
    }

    [SerializeField] private BossScript boss; 
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Collider2D platformLeft, platformRight, ground;
    [SerializeField] private float jumpPointRadius;
    [SerializeField] private jumpPointHitCollider[] jumpPointArray;

    

    private surfaceContact playerSurface, bossSurface;

    private void Awake() {
        // for(int i = 0; i < jumpPointArray.Length; i++){
        //     jumpPointArray[i].collider = Physics2D.OverlapCircle(jumpPointArray[i].jumpPointTransform.position, jumpPointRadius);
        // }
    }

    private void Update(){
        if(playerSurface != bossSurface){
            switch (playerSurface){
                case surfaceContact.Ground:
                    if(bossSurface == surfaceContact.RPlat || bossSurface == surfaceContact.LPlat){
                        int index = GetClosestGroundJumpPointConditional(0);
                        boss.currentTarget = jumpPointArray[index].jumpPointTransform;
                        
                    }
                    break;
                case surfaceContact.LPlat:
                    if(bossSurface == surfaceContact.Ground){
                        int index = GetClosestGroundJumpPointConditional(-1);
                        boss.currentTarget = jumpPointArray[index].jumpPointTransform;
                        if(Vector2.Distance(jumpPointArray[index].jumpPointTransform.position, boss.currentTarget.position) <= 0.001){
                            //Debug.Log("jump");
                            boss.Jump();
                            //boss.currentTarget = playerPosition;
                        }
                    }
                    break;
                case surfaceContact.RPlat:
                    if(bossSurface == surfaceContact.Ground){
                        int index = GetClosestGroundJumpPointConditional(1);
                        boss.currentTarget = jumpPointArray[index].jumpPointTransform;
                        if(Vector2.Distance(jumpPointArray[index].jumpPointTransform.position, boss.currentTarget.position) <= 0.001){
                            //Debug.Log("jump");
                            boss.Jump();
                            //boss.currentTarget = playerPosition;
                        }
                    }
                    break;
            }  
        }else{
            boss.currentTarget = playerPosition;
        }
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

    private int GetClosestGroundJumpPointConditional(int isLeftSided){
        int index = -1;
        switch(isLeftSided){
            case 0: //this means just loop through all 4 jumpppoints and get the closest whatever
                for(int i = 0; i < jumpPointArray.Length; i++){
                    if(index == -1)
                        index = i;
                    else if (Vector2.Distance(jumpPointArray[index].jumpPointTransform.position, boss.gameObject.transform.position) >=
                        Vector2.Distance(jumpPointArray[i].jumpPointTransform.position, boss.gameObject.transform.position)){
                            index = i;
                    }
                }
                break;
            case 1://the 2 right sided ground jump points
                for(int i = 0; i < jumpPointArray.Length; i++){
                    if(jumpPointArray[i].isLeftSided == 1){
                        if(index == -1){
                            index = i;
                        }else if (Vector2.Distance(jumpPointArray[index].jumpPointTransform.position, boss.gameObject.transform.position) >=
                        Vector2.Distance(jumpPointArray[i].jumpPointTransform.position, boss.gameObject.transform.position)){
                            index = i;
                        }
                    }
                }
                break;
            case -1://the 2 left sided ground jump points
                for(int i = 0; i < jumpPointArray.Length; i++){
                    if(jumpPointArray[i].isLeftSided == -1){
                        if(index == -1){
                            index = i;
                        }else if (Vector2.Distance(jumpPointArray[index].jumpPointTransform.position, boss.gameObject.transform.position) >=
                        Vector2.Distance(jumpPointArray[i].jumpPointTransform.position, boss.gameObject.transform.position)){
                            index = i;
                        }
                    }
                }
                break;
        }
        return index;
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            playerSurface = GetCurrentSurfaceContact(other);  
        }
        
    }
    
    //return current pos of boss based on collider it's in
    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy-Boss"){
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
        for(int i = 0; i < jumpPointArray.Length; i++){
            Gizmos.DrawWireSphere(jumpPointArray[i].jumpPointTransform.position, jumpPointRadius);
        }
       
        
    }
}
