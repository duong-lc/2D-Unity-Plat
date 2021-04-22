using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class CooldownBar_GoblinAtk : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private float curr = 0;

    public GameObject parentObject;
    private NPC_Enemy_GoblinBehavior goblinScript;
    // Start is called before the first frame update
    void Start()
    {
        goblinScript = parentObject.GetComponent<NPC_Enemy_GoblinBehavior>();
        SetMaxCooldown(goblinScript.attackInterval2Sec);
        curr = goblinScript.attackInterval2Sec;
    }

    // Update is called once per frame
    void Update()
    {   

        
            //fill.color = Color.gray;

            //curr = 0f;
            //StartCoroutine(CountToFull());
        
        
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
        for (float i = 0; i < goblinScript.attackInterval2Sec*10; i++)
        {
            SetCoolDown(curr+=.1f);
            yield return new WaitForSeconds(.1f);
        }
    }

    public void Flip(){
        //facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
