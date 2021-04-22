using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCombatScript : MonoBehaviour
{
    public GameObject blocker;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
            blocker.SetActive(true);
    }
}
