using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class CooldownBar_NPC : MonoBehaviour
{
    [Serializable]
    public enum OwningNPC {
        goblin, flyingEye, boss
    }
   // Start is called before the first frame update
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private float curr = 0;

    //public GameObject parentObject;
    [SerializeField] private NPC_Enemy_FlyingEyeBehavior EyeScript;
    [SerializeField] private BossScript bossScript;
    [SerializeField] private NPC_Enemy_GoblinBehavior goblinScript;
    [SerializeField] private OwningNPC parentMob;
    
    // Start is called before the first frame update
    void Start()
    {
        //EyeScript = parentObject.GetComponent<NPC_Enemy_FlyingEyeBehavior>();

        switch(parentMob){
            case OwningNPC.flyingEye:
                SetMaxCooldown(EyeScript.attackInterval2Sec);
                curr = EyeScript.attackInterval2Sec;
                break;
            case OwningNPC.boss:
                SetMaxCooldown(bossScript.avoidCooldown);
                curr = bossScript.avoidCooldown;
                break;
            case OwningNPC.goblin:
                SetMaxCooldown(goblinScript.attackInterval2Sec);
                curr = goblinScript.attackInterval2Sec;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        
        
    }

    public void StartCoolDown(){
        curr = 0f;
        StartCoroutine(CountToFull());
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


        switch(parentMob){
            case OwningNPC.flyingEye:
                for (float i = 0; i < EyeScript.attackInterval2Sec*10; i++)
                {
                    SetCoolDown(curr+=.1f);
                    yield return new WaitForSeconds(.1f);
                }
                break;
            case OwningNPC.boss:
                for (float i = 0; i < bossScript.avoidCooldown*10; i++)
                {
                    SetCoolDown(curr+=.1f);
                    yield return new WaitForSeconds(.1f);
                }
                break;
            case OwningNPC.goblin:
                for (float i = 0; i < goblinScript.attackInterval2Sec*10; i++)
                {
                    SetCoolDown(curr+=.1f);
                    yield return new WaitForSeconds(.1f);
                }
                break;
        }


        
    }

    public void Flip(){
        //facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}


