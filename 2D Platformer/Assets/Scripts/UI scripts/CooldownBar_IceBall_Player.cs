using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CooldownBar_IceBall_Player : MonoBehaviour
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
        SetMaxCooldown(playerMageScript.attackIntervalIceBallSec);
        curr = playerMageScript.attackIntervalIceBallSec;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerBehaviorScript.currentCharacter == 4 && Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(Time.time > playerMageScript.elapsedTimeIceBall)
            {
                curr = 0f;
                StartCoroutine("CountToFull");  
            }
        }     
        if(inventoryScript.useMageIce == true)
        {
            curr = playerMageScript.attackIntervalIceBallSec; 
            SetCoolDown(playerMageScript.attackIntervalIceBallSec);
            inventoryScript.useMageIce = false;
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
        for (float i = 0; i < playerMageScript.attackIntervalIceBallSec*10; i++)
        {
            if(curr == playerMageScript.attackIntervalIceBallSec)
            {
                break;
            }
            SetCoolDown(curr+=.1f);
            yield return new WaitForSeconds(.1f);
        }
        
    }
}
