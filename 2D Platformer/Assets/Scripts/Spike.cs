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
        if(other.gameObject.tag == "Player")
        {
            playerScript.Contact();

            if(playerScript.currentCharacter == 1)
                katana.GetComponent<Player_KatanaBehavior>().TakingDamage(playerScript.SpikeDamageToPlayer);
            else if(playerScript.currentCharacter == 2)
                archer.GetComponent<Player_ArcherBehavior>().TakingDamage(playerScript.SpikeDamageToPlayer);
            else if(playerScript.currentCharacter == 3)
                heavy.GetComponent<Player_HeavyBehavior>().TakingDamage(playerScript.SpikeDamageToPlayer);
            else if(playerScript.currentCharacter == 4)
                mage.GetComponent<Player_MageBehavior>().TakingDamage(playerScript.SpikeDamageToPlayer);
        }
    }
}
