using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerZoneTriggerScript : MonoBehaviour
{
    public struct enemyTypeNum
    {
        public string name;
        public int num;
        public GameObject type;
    }

    public GameObject demon;
    public GameObject fireWorm;
    public GameObject flyingEye;
    public GameObject goblin;
    public GameObject mushroom;
    public GameObject skeleton;
    public GameObject slime;

    public int demonNum;
    public int fireWormNum;
    public int flyingEyeNum;
    public int goblinNum;
    public int mushroomNum;
    public int skeletonNum;
    public int slimeNum;

    private enemyTypeNum demonStruct;
    private enemyTypeNum fireWormStruct;
    private enemyTypeNum flyingEyeStruct;
    private enemyTypeNum goblinStruct;
    private enemyTypeNum mushroomStruct;
    private enemyTypeNum skeletonStruct;
    private enemyTypeNum slimeStruct;

    private int numEnemyToSpawn;
    private enemyTypeNum[] enemiesArray;
    public Transform spawnerTransform;
    private bool callOnce = true;

    private void Awake() {
        //numEnemyToSpawn = demonNum + fireWormNum + flyingEyeNum + goblinNum + mushroomNum + skeletonNum + slimeNum;
        enemiesArray = new enemyTypeNum[7];

        demonStruct.name = "demon";
        demonStruct.num = demonNum;
        demonStruct.type = demon;
        enemiesArray[0] = demonStruct; 

        fireWormStruct.name = "fireworm";
        fireWormStruct.num = fireWormNum;
        fireWormStruct.type = fireWorm;
        enemiesArray[1] = fireWormStruct;

        flyingEyeStruct.name = "flyingeye";
        flyingEyeStruct.num = flyingEyeNum;
        flyingEyeStruct.type = flyingEye;
        enemiesArray[2] = flyingEyeStruct;

        goblinStruct.name = "goblin";
        goblinStruct.num = goblinNum;
        goblinStruct.type = goblin;
        enemiesArray[3] = goblinStruct;

        mushroomStruct.name = "mushroom";
        mushroomStruct.num = mushroomNum;
        mushroomStruct.type = mushroom;
        enemiesArray[4] = mushroomStruct;

        skeletonStruct.name = "skeleton";
        skeletonStruct.num = skeletonNum;
        skeletonStruct.type = skeleton;
        enemiesArray[5] = skeletonStruct;

        slimeStruct.name = "slime";
        slimeStruct.num = slimeNum;
        slimeStruct.type = slime;
        enemiesArray[6] = slimeStruct;

    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (callOnce == true)
        {
            if(other.CompareTag("Player"))
            {
                foreach (enemyTypeNum enemy in enemiesArray)
                {
                    for (int i = 0; i < enemy.num; i++)
                    {
                        Instantiate(enemy.type, new Vector3(spawnerTransform.position.x + Random.Range(-3.0f, 3.0f), spawnerTransform.position.y + Random.Range(-1.0f, 1.0f),spawnerTransform.position.z), Quaternion.identity);
                        //Debug.Log(enemy.type);
                        //i++;
                    }
                }
                callOnce = false;
                Destroy(gameObject);
            }
            
        }  
    }
}
