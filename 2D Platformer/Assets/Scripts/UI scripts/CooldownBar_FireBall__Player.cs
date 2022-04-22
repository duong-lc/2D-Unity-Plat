using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CooldownBar_FireBall__Player : MonoBehaviour
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
        _playerBehaviorScript = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        _playerMageScript = GameObject.Find("Player").transform.Find("Player-Mage").GetComponent<Player_MageBehavior>();
        _inventoryScript = GameObject.Find("Player").GetComponent<PlayerInventorySystem>();
        SetMaxCooldown(_playerMageScript.attackIntervalFireBallSec);
        _curr = _playerMageScript.attackIntervalFireBallSec;
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerBehaviorScript.currentCharacter == CurrentCharacter.Mage && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(Time.time > _playerMageScript.elapsedTimeFireBall)
            {
                _curr = 0f;
                StartCoroutine("CountToFull");  
            }
        }
        if(_inventoryScript.useMageFire == true)
        {
            _curr = _playerMageScript.attackIntervalFireBallSec; 
            SetCoolDown(_playerMageScript.attackIntervalFireBallSec);
            _inventoryScript.useMageFire = false;
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
        for (float i = 0; i < _playerMageScript.attackIntervalFireBallSec*10; i++)
        {
            if(_curr == _playerMageScript.attackIntervalFireBallSec)
            {
                break;
            }
            SetCoolDown(_curr+=.1f);
            yield return new WaitForSeconds(.1f);
        }
        
    }
}
