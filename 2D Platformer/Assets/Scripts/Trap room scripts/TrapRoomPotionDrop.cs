using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoomPotionDrop : MonoBehaviour
{
    private bool callOnce = false;
    private bool callOnce1 = false;
    public GameObject spellPotionPrefab;
    // Update is called once per frame

    void LateUpdate()
    {
        if(!callOnce)
        {
            spellPotionPrefab = GameObject.Find("_Spawner").GetComponent<TrapRoomSpawner>().spellPrefab;
            callOnce = true;
        }
        if(this.gameObject.GetComponent<NPCVitalityHandler>().isDead)
        {
            if(!callOnce1)
            {
                Instantiate(spellPotionPrefab, this.gameObject.transform.position, Quaternion.identity);
                callOnce1 = true;
            }
        }
    }
}
