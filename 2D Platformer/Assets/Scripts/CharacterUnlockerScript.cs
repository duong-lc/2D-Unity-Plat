using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class CharacterUnlockerScript : MonoBehaviour
{
    public GameObject playerArcher, playerHeavy, playerMage;
    public BoxCollider2D colliderArcherActivate, colliderHeavyActivate,colliderMageActivate;
    private PlayerBehavior parentPlayerScript;

    // Start is called before the first frame update
    async void Awake()
    {
        parentPlayerScript = playerArcher.transform.GetComponentInParent<PlayerBehavior>();

        await Task.Delay(300);
        playerArcher.GetComponent<PlayerVitalityHandler>().currentHealth = 0;
        playerHeavy.GetComponent<PlayerVitalityHandler>().currentHealth = 0;
        playerMage.GetComponent<PlayerVitalityHandler>().currentHealth = 0;

        parentPlayerScript.isArcherAlive = false;
        parentPlayerScript.isHeavyAlive = false;
        parentPlayerScript.isMageAlive = false;
        // parentPlayerScript.AliveList[1] = false;
        // parentPlayerScript.AliveList[2] = false;
        // parentPlayerScript.AliveList[3] = false; 

        playerHeavy.GetComponent<Player_HeavyBehavior>().heavyCoolDownBar.SetActive(false);
        playerMage.GetComponent<Player_MageBehavior>().mageCoolDownBarFire.SetActive(false);
        playerMage.GetComponent<Player_MageBehavior>().mageCoolDownbarIce.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        //declaring local vars
        Player_ArcherBehavior archerScript = playerArcher.GetComponent<Player_ArcherBehavior>();
        Player_HeavyBehavior heavyScript = playerHeavy.GetComponent<Player_HeavyBehavior>();
        Player_MageBehavior mageScript = playerMage.GetComponent<Player_MageBehavior>();

        if(other.tag == "Player")
        {
            if(other.IsTouching(colliderArcherActivate) )
            {
                other.GetComponent<PlayerBehavior>().SpawnDamageText("Archer Unlocked", Color.white, 1.0f);
                //activate the archer character
                playerArcher.GetComponent<PlayerVitalityHandler>().currentHealth = playerArcher.GetComponent<PlayerVitalityHandler>().maxHealth;
                playerArcher.GetComponent<PlayerVitalityHandler>().healthBar.SetHealth(playerArcher.GetComponent<PlayerVitalityHandler>().maxHealth);
                parentPlayerScript.isArcherAlive = true;
                // parentPlayerScript.AliveList[1] = true;
                //after that, disable collider to prevent multiple activations
                colliderArcherActivate.enabled = false;
            }else if(other.IsTouching(colliderHeavyActivate) )
            {
                other.GetComponent<PlayerBehavior>().SpawnDamageText("Berserker Unlocked", Color.white, 1.0f);
                playerHeavy.GetComponent<PlayerVitalityHandler>().currentHealth = playerHeavy.GetComponent<PlayerVitalityHandler>().maxHealth;
                playerHeavy.GetComponent<PlayerVitalityHandler>().healthBar.SetHealth(playerHeavy.GetComponent<PlayerVitalityHandler>().maxHealth);
                parentPlayerScript.isHeavyAlive = true;
                // parentPlayerScript.AliveList[2] = true;
                colliderHeavyActivate.enabled = false;
                playerHeavy.GetComponent<Player_HeavyBehavior>().heavyCoolDownBar.SetActive(true);
            }else if(other.IsTouching(colliderMageActivate) )
            {
                other.GetComponent<PlayerBehavior>().SpawnDamageText("Caster Unlocked", Color.white, 1.0f);
                playerMage.GetComponent<PlayerVitalityHandler>().currentHealth = playerMage.GetComponent<PlayerVitalityHandler>().maxHealth;
                playerMage.GetComponent<PlayerVitalityHandler>().healthBar.SetHealth(playerMage.GetComponent<PlayerVitalityHandler>().maxHealth);
                parentPlayerScript.isMageAlive = true;
                // parentPlayerScript.AliveList[3] = true;
                colliderMageActivate.enabled = false;
                playerMage.GetComponent<Player_MageBehavior>().mageCoolDownBarFire.SetActive(true);
                playerMage.GetComponent<Player_MageBehavior>().mageCoolDownbarIce.SetActive(true);
            }
        }
         
    }
}
