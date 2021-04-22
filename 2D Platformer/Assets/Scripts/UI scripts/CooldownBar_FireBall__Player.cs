using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CooldownBar_FireBall__Player : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private float curr = 0;
    private Player_MageBehavior playerMageScript;
    private PlayerBehavior playerBehaviorScript;
    private PlayerInventorySystem inventoryScript;
    // Start is called before the first frame update
    void Start()
    {
        playerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        playerMageScript = GameObject.Find("Player").transform.Find("Player-Mage").GetComponent<Player_MageBehavior>();
        inventoryScript = GameObject.Find("Player").GetComponent<PlayerInventorySystem>();
        SetMaxCooldown(playerMageScript.attackIntervalFireBallSec);
        curr = playerMageScript.attackIntervalFireBallSec;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerBehaviorScript.currentCharacter == 4 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(Time.time > playerMageScript.elapsedTimeFireBall)
            {
                curr = 0f;
                StartCoroutine("CountToFull");  
            }
        }
        if(inventoryScript.useMageFire == true)
        {
            curr = playerMageScript.attackIntervalFireBallSec; 
            SetCoolDown(playerMageScript.attackIntervalFireBallSec);
            inventoryScript.useMageFire = false;
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


    IEnumerator CountToFull(){
        for (float i = 0; i < playerMageScript.attackIntervalFireBallSec*10; i++)
        {
            if(curr == playerMageScript.attackIntervalFireBallSec)
            {
                break;
            }
            SetCoolDown(curr+=.1f);
            yield return new WaitForSeconds(.1f);
        }
        
    }
}
