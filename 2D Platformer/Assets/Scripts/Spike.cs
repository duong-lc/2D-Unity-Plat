using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public GameObject player;
    public GameObject katana;
    public GameObject archer;
    public GameObject heavy;
    public GameObject mage;

    private PlayerBehavior playerScript;
    void Start()
    {
        playerScript = player.GetComponent<PlayerBehavior>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerScript.Contact();
            player.GetComponent<PlayerBehavior>().CallDamage(playerScript.SpikeDamageToPlayer);
        }
    }
}
