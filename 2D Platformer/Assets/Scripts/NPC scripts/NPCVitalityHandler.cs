using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class NPCVitalityHandler : MonoBehaviour
{
    private Animator animator;

    /*health bar and health displayf or mobs*/
    public float currentHealth, maxHealth;
    public HealthBar healthBar;

    public bool isDead, isFrozen, isStagger = false;
    public GameObject bloodEffect, chunkEffect;
   
    private BossScript boss;

    private void Awake() {

        if (this.gameObject.tag == "Enemy-Boss"){
            boss = gameObject.GetComponent<BossScript>();
        }


        animator = this.gameObject.GetComponent<Animator>();
        currentHealth = maxHealth;
        if(healthBar != null)
            healthBar.SetMaxHealth(maxHealth);
    }

    void Update(){
        if (isFrozen ==true){
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        
        if(gameObject.tag == "Enemy-Boss"){
            if(isFrozen == false && isStagger == false && boss.isRage == false){
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }else if(boss.isRage == true){
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                isStagger = false;
                isFrozen = false;
                animator.speed = 1f;
            }
        }
        
        
    }

    async public void TakeDamage(float DamageTaken, bool canAvoid)
    {
        //Debug.Log("aaaa");
        if(isDead == false)
        {
            if(this.gameObject.tag != "Enemy-Goblin" && this.gameObject.tag != "Enemy-Skeleton" && this.gameObject.tag != "Enemy-Boss")
                macroFunction1(DamageTaken, 500, Color.white, false);
            else if (this.gameObject.tag == "Enemy-Goblin"){
                if(canAvoid == false || isFrozen == true){
                    macroFunction1(DamageTaken, 500, Color.white, false);
                }else if (canAvoid == true && isFrozen == false){
                    isStagger = false;
                    NPC_Enemy_GoblinBehavior goblinScript = this.gameObject.GetComponent<NPC_Enemy_GoblinBehavior>();
                    if(Time.time > goblinScript.elapsedTime2){
                        SpawnDamageText("Missed", Color.white, 1.0f);
                        goblinScript.cooldown.GetComponent<CooldownBar_NPC>().StartCoolDown();
                        goblinScript.AttackPlayerAnim(2);
                        goblinScript.elapsedTime2 = Time.time + goblinScript.attackInterval2Sec;
                    }else{
                        macroFunction1(DamageTaken, 500, Color.white, false);
                    }
                }
            }else if(this.gameObject.tag == "Enemy-Skeleton"){
                if(canAvoid == true && isFrozen == false){
                    isStagger = true;
                    animator.SetTrigger("ShieldUp");
                    SpawnDamageText("Blocked", Color.white, 1.0f);
                    await Task.Delay(800);
                    isStagger = false;
                }else if (canAvoid == false||isFrozen ==true){
                    macroFunction1(DamageTaken, 800, Color.white, false);
                }
            }else if (this.gameObject.tag == "Enemy-Boss")//else if (gameobject.tag == boss)
            {
                //Debug.Log(canAvoid + " " + Time.time + " " + boss.avoidCooldownElapsedTime);
                if (canAvoid == false||isFrozen ==true){
                    macroFunction1(DamageTaken, 800, Color.white, false);
                    boss.bossStats.GetComponent<BossStat>().UpdateHealth();
                }else if(canAvoid == true && isFrozen == false){//if frozen then spawn some portals

                    switch(boss.dodgeState){
                        case BossScript.avoidState.block://do bloack animation and not take incoming damage
                            animator.SetTrigger("Block");
                            break;
                        case BossScript.avoidState.roll://do roll animation and not take incomeing damage
                            animator.SetTrigger("Roll");
                            break;
                    }
                }
            }
        }
        
        if (currentHealth <= 0)
        {
            healthBar.gameObject.SetActive(false);
            Death();
        }
        
    }
    
    private async void macroFunction1(float DamageTaken, int staggerDelay, Color damageColor, bool isBurnDamage)
    {    
        if(!isDead){
            isStagger = true;
            currentHealth -= DamageTaken;

            if (this.gameObject.tag == "Enemy-Boss"){
                boss.bossStats.GetComponent<BossStat>().UpdateHealth();

                boss.rageCounter++;
                boss.bossStats.GetComponent<BossStat>().UpdateRageCounter();
            }


            animator.SetBool("isWalking", false);
            animator.SetTrigger("TakeHit");

            if(isFrozen == false)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                if(this.gameObject.tag == "Enemy-Demon")
                    this.gameObject.GetComponent<NPC_Enemy_DemonBehavior>().isRedTint = true;
                await Task.Delay(300);
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                if(this.gameObject.tag == "Enemy-Demon")
                    this.gameObject.GetComponent<NPC_Enemy_DemonBehavior>().isRedTint = false;

                SpawnDamageText(DamageTaken.ToString(), damageColor, 0.7f);
            }
            
            else if (isFrozen == true)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
                if(this.gameObject.tag == "Enemy-Demon")
                    this.gameObject.GetComponent<NPC_Enemy_DemonBehavior>().isCyanTint = true;
                
                if(!isBurnDamage)
                    SpawnDamageText(DamageTaken.ToString(), Color.cyan, 0.85f);
                else if(isBurnDamage)
                    SpawnDamageText(DamageTaken.ToString(), damageColor, 0.85f);
            }
            
            

            healthBar.SetHealth(currentHealth);
            await Task.Delay(staggerDelay);
            isStagger = false; 
        }
    }
    
    private void SpawnDamageText(string DamageText, Color damageColor, float duration){

        GameObject text = this.gameObject.GetComponent<DamagePopUpSpawnScript>().SpawnDamagedText();
        // if(gameObject.tag == "Enemy-Boss"){
        //     text.transform.position.Set(text.transform.position.x, text.transform.position.y - 1, text.transform.position.z); 
        // }
        DamagePopUpTextScript textScript = text.GetComponent<DamagePopUpTextScript>();
        textScript.SetText(DamageText.ToString());
        textScript.colorStart = damageColor;
        textScript.colorEnd = damageColor;
        textScript.colorEnd.a = 0;
        if(duration != 0)   
            textScript.fadeDuration = duration;
        
    }


    public void Death(){
        isDead = true;
        this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        
        if(this.gameObject.tag == "Enemy-FlyingEye"){
            this.gameObject.GetComponent<NPC_Enemy_FlyingEyeBehavior>().cooldown.SetActive(false);
        }else if (this.gameObject.tag == "Enemy-Goblin"){
            this.gameObject.GetComponent<NPC_Enemy_GoblinBehavior>().cooldown.SetActive(false);
        }

        if(this.gameObject.tag == "Enemy-FlyingEye"){
            this.GetComponent<SpriteRenderer>().enabled = false;
            SpawnBloodEffect();
        }else if (this.gameObject.tag == "Enemy-Demon"){
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<NPC_Enemy_DemonBehavior>().enabled = false;
            SpawnBloodEffect();
            SpawnChunkEffect();
        }
        
        animator.SetBool("isWalking", false);
        animator.SetBool("isDead", true);

        this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        GetComponent<Collider2D>().enabled = false;
        
        isStagger = true;
        Destroy(this.gameObject, 5f);
    }

    private void SpawnBloodEffect(){
        Instantiate(bloodEffect, this.transform.position, Quaternion.identity, this.transform);
    }

    private void SpawnChunkEffect(){
        Instantiate(chunkEffect, this.transform.position, Quaternion.identity, this.transform);
    }

    public void Take_Spell_Damage_Burn(float burnInterval, float burnDamage, float totalBurnTime_Loop){
        StartCoroutine(WaitAndBurn(burnInterval, burnDamage, totalBurnTime_Loop));
    }

    async public void Take_Spell_Frozen(float freezePeriod){
        //StartCoroutine(WaitAndFreeze(freezePeriod));    

        isStagger = true;
        isFrozen = true;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        if(this.gameObject.tag == "Enemy-Demon")
            this.gameObject.GetComponent<NPC_Enemy_DemonBehavior>().isCyanTint = true;
        if(gameObject.tag == "Enemy-Boss"){
            boss.PortalSpawner.GetComponent<PortalGeneralBehavior>().SpawnPortal();
        }
            
        animator.speed = 0.001f;

        await Task.Delay((int)(freezePeriod*1000f));

        isStagger = false;
        isFrozen = false;
        animator.speed = 1f;
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        if(this.gameObject.tag == "Enemy-Demon")
            this.gameObject.GetComponent<NPC_Enemy_DemonBehavior>().isCyanTint = false;
    }

    private IEnumerator WaitAndBurn(float burnInterval, float burnDamage, float totalBurnTime_Loop)
    {
        for(float i = 0; i < totalBurnTime_Loop; i++)
        {
            macroFunction1(burnDamage, 500, new Color(255, 140, 0, 1), true);//yellow/orange color to simulate burn damage
            yield return new WaitForSeconds(burnInterval);
        }
    }
}
