using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoomSpawner : MonoBehaviour
{
    public GameObject skeletonPrefab;
    private TrapRoomScript trapRoomScript;
    public GameObject trapRoom;
    private bool isAlive;
    private GameObject spawned;
    public GameObject spellPrefab;

    private void Awake()
    {
        trapRoomScript = trapRoom.GetComponent<TrapRoomScript>();
        isAlive = false;
    }

    private void LateUpdate() {
        if(trapRoomScript.spawnRoomActivated)
        {
            if(!isAlive)
            {
                spawned = Instantiate(skeletonPrefab, this.transform.position, Quaternion.identity);
                spawned.AddComponent<TrapRoomPotionDrop>();
                isAlive = true;
            }
            if(spawned == null)
            {
                isAlive = false;
            }
        }
    }
}
