using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoomScript : MonoBehaviour
{
    public GameObject roof;
    public GameObject door1;
    public GameObject door2;
    public GameObject spawner;
    public float transparency = 1f;
    public bool spawnRoomActivated;

    private void Awake() {
        spawnRoomActivated = false;
        roof.SetActive(false);
        door1.SetActive(false);
        door2.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            spawnRoomActivated = true;
            roof.SetActive(true);
            door1.SetActive(true);
            door2.SetActive(true);
            this.gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
    private void Update() {
        if(door1.GetComponent<TrapRoomDoorScript>().isActivated && door2.GetComponent<TrapRoomDoorScript>().isActivated)
        {
            StartCoroutine(Unlocked());
        }
        if(transparency <= 0)
        {
            roof.SetActive(false);
            door1.SetActive(false);
            door2.SetActive(false);
            spawner.SetActive(false);
        }
    }

    private IEnumerator Unlocked()
    {
        
        while(true)
        {
            transparency = transparency - 0.01f;
            roof.GetComponent<SpriteRenderer>().color = new Color (0f, 255f, 0f, transparency);
            if(transparency == 0)
                break;
            yield return new WaitForSeconds(0.2f);
        }  
 
    }
}
