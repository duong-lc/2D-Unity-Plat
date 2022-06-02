using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerInventorySystem : MonoBehaviour
{
    public Text HealthRefillAmount;
    public Text HeavyRefillAmount;
    public Text MageRefillAmount;
    
    public GameObject healingFXPrefab;
    private GameObject healingFX;

    public GameObject heavyFXPrefab;
    private GameObject heavyFX;

    public GameObject magicFXPrefab;
    private GameObject magicFX;


    public float amountHeals;
    private Player_HeavyBehavior heavyScript;
    private Player_MageBehavior mageScript;
    private Player_KatanaBehavior katanaScript;
    private Player_ArcherBehavior archerScript;
    private PlayerBehavior playerScript;

    public int currentHealthAmount = 0;
    public int currentHeavyAmount = 0;
    public int currentMageAmount = 0;
    public bool useHeavy = false;
    public bool useMageFire = false;
    public bool useMageIce = false;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerBehavior>();
    }

    // Update is called once per frame
    void Update()
    {   
        try
        {
            mageScript = GameObject.FindWithTag("Player").transform.Find("Player-Mage").GetComponent<Player_MageBehavior>();//had to do it this way for some reason findobjectwithtag returns null
            heavyScript = GameObject.FindWithTag("Player").transform.Find("Player-Heavy").GetComponent<Player_HeavyBehavior>();
            katanaScript = GameObject.FindWithTag("Player").transform.Find("Player-Katana").GetComponent<Player_KatanaBehavior>();
            archerScript = GameObject.FindWithTag("Player").transform.Find("Player-Archer").GetComponent<Player_ArcherBehavior>();
        }
        catch(Exception e)
        {
            ;
        }

        if(healingFX != null)
        {
            healingFX.transform.position = playerScript.gameObject.transform.position;
        }
        if(heavyFX != null)
        {
            heavyFX.transform.position = playerScript.gameObject.transform.position;
        }
        if(magicFX != null)
        {
            magicFX.transform.position = playerScript.gameObject.transform.position;
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(currentHealthAmount > 0)
            {
                healingFX = Instantiate(healingFXPrefab, playerScript.gameObject.transform.position, Quaternion.identity);
                currentHealthAmount--;
                HealthRefillAmount.text = currentHealthAmount + "";

                ForceReviveEverything();
                
                katanaScript.gameObject.GetComponent<PlayerVitalityHandler>().Heals(amountHeals);
                archerScript.gameObject.GetComponent<PlayerVitalityHandler>().Heals(amountHeals);
                heavyScript.gameObject.GetComponent<PlayerVitalityHandler>().Heals(amountHeals);
                mageScript.gameObject.GetComponent<PlayerVitalityHandler>().Heals(amountHeals);

            }
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if(currentHeavyAmount > 0)
            {
                heavyFX = Instantiate(heavyFXPrefab, playerScript.gameObject.transform.position, Quaternion.identity);
                useHeavy = true;
                heavyScript.elapsedTime = Time.time;
                currentHeavyAmount--;
                HeavyRefillAmount.text = currentHeavyAmount + "";
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            if(currentMageAmount > 0)
            {
                magicFX = Instantiate(magicFXPrefab, playerScript.gameObject.transform.position, Quaternion.identity);

                useMageFire = true;
                //useMageIce = true;
                
                mageScript.elapsedTimeFireBall = Time.time;
                //mageScript.elapsedTimeIceBall = Time.time;

                
                currentMageAmount--;
                MageRefillAmount.text = currentMageAmount + "";
            }
        }
    }

    private void ForceReviveEverything()
    {
        katanaScript.enabled = true;
        archerScript.enabled = true;
        heavyScript.enabled = true;
        mageScript.enabled = true;

        playerScript.isArcherAlive = true;
        playerScript.isKatanaAlive = true;
        playerScript.isMageAlive = true;
        playerScript.isHeavyAlive = true;

        heavyScript.heavyCoolDownBar.SetActive(true);
        mageScript.mageCoolDownBarFire.SetActive(true);
        mageScript.mageCoolDownbarIce.SetActive(true);
        
        GameObject text = playerScript.gameObject.GetComponent<DamagePopUpSpawnScript>().SpawnDamagedText();
        DamagePopUpTextScript textScript = text.GetComponent<DamagePopUpTextScript>();
        textScript.SetText("Restored");
        textScript.colorStart = Color.green;
        textScript.colorEnd = Color.green;
        textScript.colorEnd.a = 0;
        textScript.fadeDuration = 0.9f;

        // for(int i = 0; i < playerScript.AliveList.Count; i++)
        // {
        //     playerScript.AliveList[i] = true;
        // }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp-Mage"))
        {
            Destroy(other.gameObject);
            currentMageAmount+=1;
            MageRefillAmount.text = currentMageAmount + "";
            
        }   
        else if (other.gameObject.CompareTag("PickUp-Heavy"))
        {
            Destroy(other.gameObject);
            currentHeavyAmount+=1;
            HeavyRefillAmount.text = currentHeavyAmount + "";
            
        }
        else if (other.gameObject.CompareTag("PickUp-Health"))
        {
            Destroy(other.gameObject);
            currentHealthAmount+=1;
            HealthRefillAmount.text = currentHealthAmount + "";
        }
    }
}
