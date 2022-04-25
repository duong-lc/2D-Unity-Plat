using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class PlayerVitalityHandler : PlayerBaseBehavior
{
    public float currentHealth, maxHealth;
    public HealthBar healthBar;
    public bool isPlayerTakingDamage = false;
    //public int katanaTakeDamageDelayMS, archerTakeDamageDelayMS, heavyTakeDamageDelayMS, mageTakeDamageDelayMS, katanaDeathDelayMS, archerDeathDelayMS, heavyDeathDelayMS, mageDeathDelayMS;

    private Animator _animator;
    //private PlayerBehavior parent_PlayerBehaviorScript;
    //public GameObject popUpText;
    
    private void Start(){
        //parent_PlayerBehaviorScript = this.gameObject.transform.parent.GetComponent<PlayerBehavior>();
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
        
        await Task.Delay(GetComponent<PlayerBaseBehavior>().TakeDamageDelayMS);
        
        //_animator.SetBool("isTakeHit", false);
        _animator.SetBool(TakeHit, false);
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

        ParentPlayerBehaviorScript.isInDeathAnim = true;
        ParentPlayerBehaviorScript.playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        ParentPlayerBehaviorScript.gameObject.GetComponent<Collider2D>().enabled = false;
        
        try{
            await Task.Delay(GetComponent<PlayerBaseBehavior>().DeathDelayMS);
            GetComponent<PlayerMovementAnimHandler>().enabled = false;
           
            switch (ParentPlayerBehaviorScript.currentCharacter){
                case CurrentCharacter.Katana:
                    GetComponent<Player_KatanaBehavior>().enabled = false;
                    ParentPlayerBehaviorScript.isKatanaAlive = false;
                    break;
                case CurrentCharacter.Archer:
                    GetComponent<Player_ArcherBehavior>().enabled = false;
                    ParentPlayerBehaviorScript.isArcherAlive = false;
                    break;
                case CurrentCharacter.Heavy:
                    var heavyBehavior = GetComponent<Player_HeavyBehavior>();
                    heavyBehavior.heavyCoolDownBar.SetActive(false);
                    heavyBehavior.enabled = false;
                    ParentPlayerBehaviorScript.isHeavyAlive = false;
                    break;
                case CurrentCharacter.Mage:
                    var mageBehavior = GetComponent<Player_MageBehavior>();
                    mageBehavior.mageCoolDownBarFire.SetActive(false);
                    mageBehavior.mageCoolDownbarIce.SetActive(false);
                    mageBehavior.enabled = false;
                    ParentPlayerBehaviorScript.isMageAlive = false;
                    break;
            }
            
            //OnCharacterDeath();
            ParentPlayerBehaviorScript.playerRB.constraints = RigidbodyConstraints2D.None;
            ParentPlayerBehaviorScript.gameObject.GetComponent<Collider2D>().enabled = true;

        }
        catch(Exception e){
            ;
        }

        ParentPlayerBehaviorScript.SwitchToAlive();
        ParentPlayerBehaviorScript.isInDeathAnim = false;

        this.enabled = false;
    }

    public void Heals(float amount){
        currentHealth += amount;
        if(currentHealth > maxHealth)
            currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
    }
}
