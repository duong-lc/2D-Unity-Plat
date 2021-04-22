using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private float curr = 0;
    private Player_HeavyBehavior playerHeavyScript;
    private PlayerBehavior playerBehaviorScript;
    private PlayerInventorySystem inventoryScript;

    // Start is called before the first frame update
    void Start()
    {
        playerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        playerHeavyScript = GameObject.Find("Player").transform.Find("Player-Heavy").GetComponent<Player_HeavyBehavior>();
        inventoryScript = GameObject.Find("Player").GetComponent<PlayerInventorySystem>();
        SetMaxCooldown(playerHeavyScript.attackIntervalSec);
        curr = playerHeavyScript.attackIntervalSec;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerBehaviorScript.currentCharacter == 3 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(Time.time > playerHeavyScript.elapsedTime)
            {
                curr = 0f;
                StartCoroutine("CountToFull");  
            }
        }                           
        if(inventoryScript.useHeavy ==true)
        {
            curr = playerHeavyScript.attackIntervalSec; 
            SetCoolDown(playerHeavyScript.attackIntervalSec);
            inventoryScript.useHeavy = false;
        }  
    }

    public void SetMaxCooldown(float maxTime)
    {
        slider.maxValue = maxTime;
        slider.value = maxTime;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetCoolDown(float currTime)
    {
        slider.value = currTime;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    IEnumerator CountToFull()
    {
        for (float i = 0; i < playerHeavyScript.attackIntervalSec*10; i++)
        {
            if(curr == playerHeavyScript.attackIntervalSec)
            {
                break;
            }
            SetCoolDown(curr+=.1f);
            yield return new WaitForSeconds(.1f);
        }
    }
}
