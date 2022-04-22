using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class PlayerVitalityHandler : Player_BaseBehavior
{
    public float currentHealth, maxHealth;
    public HealthBar healthBar;
    public bool isPlayerTakingDamage = false;
    //public int katanaTakeDamageDelayMS, archerTakeDamageDelayMS, heavyTakeDamageDelayMS, mageTakeDamageDelayMS, katanaDeathDelayMS, archerDeathDelayMS, heavyDeathDelayMS, mageDeathDelayMS;

    private Animator _animator;
    private PlayerBehavior parent_PlayerBehaviorScript;
    //public GameObject popUpText;
    
    private void Start(){
        parent_PlayerBehaviorScript = this.gameObject.transform.parent.GetComponent<PlayerBehavior>();
        _animator = this.gameObject.GetComponent<Animator>();

        //currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    public async void TakingDamage(float damageTaken){
        isPlayerTakingDamage = true;
        //_animator.SetBool("isTakeHit", true);
        _animator.SetBool(TakeHit, true);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        

        // switch (parent_PlayerBehaviorScript.currentCharacter){
        //     case 1:
        //         //await Task.Delay(parent_PlayerBehaviorScript.katanaTakeDamageDelayMS);
        //         await Task.Delay(GetComponent<Player_BaseBehavior>().TakeDamageDelayMS);
        //         break;
        //     case 2:
        //         await Task.Delay(parent_PlayerBehaviorScript.archerTakeDamageDelayMS);
        //         break;
        //     case 3:
        //         await Task.Delay(parent_PlayerBehaviorScript.heavyTakeDamageDelayMS);
        //         break;
        //     case 4:
        //         await Task.Delay(parent_PlayerBehaviorScript.mageTakeDamageDelayMS);
        //         break;
        // }
        await Task.Delay(GetComponent<Player_BaseBehavior>().TakeDamageDelayMS);
        
        //_animator.SetBool("isTakeHit", false);
        _animator.SetBool(TakeHit, true);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        isPlayerTakingDamage = false;

        currentHealth -= damageTaken;
        
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0){
            OnDeath();
        }
    }
    async public void OnDeath(){
        currentHealth = 0;
        //_animator.SetTrigger("Die");
        _animator.SetTrigger(Death);

        parent_PlayerBehaviorScript.isInDeathAnim = true;
        parent_PlayerBehaviorScript.playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        parent_PlayerBehaviorScript.gameObject.GetComponent<Collider2D>().enabled = false;
        
        try{
            await Task.Delay(GetComponent<Player_BaseBehavior>().DeathDelayMS);
            GetComponent<PlayerMovementAnimHandler>().enabled = false;
           
            switch (parent_PlayerBehaviorScript.currentCharacter){
                case CurrentCharacter.Katana:
                    GetComponent<Player_KatanaBehavior>().enabled = false;
                    parent_PlayerBehaviorScript.isKatanaAlive = false;
                    break;
                case CurrentCharacter.Archer:
                    //await Task.Delay(GetComponent<Player_BaseBehavior>().DeathDelayMS);
                    GetComponent<Player_ArcherBehavior>().enabled = false;
                    parent_PlayerBehaviorScript.isArcherAlive = false;
                    break;
                case CurrentCharacter.Heavy:
                    //await Task.Delay(GetComponent<Player_BaseBehavior>().DeathDelayMS);
                    GetComponent<Player_HeavyBehavior>().enabled = false;
                    GetComponent<Player_HeavyBehavior>().heavyCoolDownBar.SetActive(false);
                    parent_PlayerBehaviorScript.isHeavyAlive = false;
                    break;
                case CurrentCharacter.Mage:
                    //await Task.Delay(GetComponent<Player_BaseBehavior>().DeathDelayMS);
                    GetComponent<Player_MageBehavior>().enabled = false;
                    GetComponent<Player_MageBehavior>().mageCoolDownBarFire.SetActive(false);
                    GetComponent<Player_MageBehavior>().mageCoolDownbarIce.SetActive(false);
                    parent_PlayerBehaviorScript.isMageAlive = false;
                    break;
            }
            
            //OnCharacterDeath();
            parent_PlayerBehaviorScript.playerRB.constraints = RigidbodyConstraints2D.None;
            parent_PlayerBehaviorScript.gameObject.GetComponent<Collider2D>().enabled = true;

        }
        catch(Exception e){
            ;
        } 

        

        // switch (parent_PlayerBehaviorScript.currentCharacter){
        //     case 1:
        //         parent_PlayerBehaviorScript.AliveList[0] = false;
        //         break;
        //     case 2:
        //         parent_PlayerBehaviorScript.AliveList[1] = false;
        //         break;
        //     case 3:
        //         parent_PlayerBehaviorScript.AliveList[2] = false;
        //         break;
        //     case 4:
        //         parent_PlayerBehaviorScript.AliveList[3] = false;
        //         break;
        // }
        
        parent_PlayerBehaviorScript.SwitchToAlive();
        parent_PlayerBehaviorScript.isInDeathAnim = false;

        this.enabled = false;
    }

    public void Heals(float amount){
        currentHealth += amount;
        if(currentHealth > maxHealth)
            currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
    }
}
