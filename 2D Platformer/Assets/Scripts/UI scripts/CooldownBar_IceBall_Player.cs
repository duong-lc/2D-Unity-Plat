using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CooldownBar_IceBall_Player : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private float _curr = 0;
    private Player_MageBehavior _playerMageScript;
    private PlayerBehavior _playerBehaviorScript;
    private PlayerInventorySystem _inventoryScript;
    // Start is called before the first frame update
    void Start()
    {
        _playerBehaviorScript = GameObject.FindWithTag("Player").GetComponent<PlayerBehavior>();
        _playerMageScript = GameObject.FindWithTag("Player").transform.Find("Player-Mage").GetComponent<Player_MageBehavior>();
        _inventoryScript = GameObject.FindWithTag("Player").GetComponent<PlayerInventorySystem>();
        SetMaxCooldown(_playerMageScript.attackIntervalIceBallSec);
        _curr = _playerMageScript.attackIntervalIceBallSec;
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerBehaviorScript.currentCharacter == CurrentCharacter.Mage && Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(Time.time > _playerMageScript.elapsedTimeIceBall)
            {
                _curr = 0f;
                StartCoroutine("CountToFull");  
            }
        }     
        if(_inventoryScript.useMageIce == true)
        {
            _curr = _playerMageScript.attackIntervalIceBallSec; 
            SetCoolDown(_playerMageScript.attackIntervalIceBallSec);
            _inventoryScript.useMageIce = false;
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
        for (float i = 0; i < _playerMageScript.attackIntervalIceBallSec*10; i++)
        {
            if(_curr == _playerMageScript.attackIntervalIceBallSec)
            {
                break;
            }
            SetCoolDown(_curr+=.1f);
            yield return new WaitForSeconds(.1f);
        }
        
    }
}
