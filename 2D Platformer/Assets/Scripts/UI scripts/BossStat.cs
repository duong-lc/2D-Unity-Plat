using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStat : MonoBehaviour
{
    [SerializeField] private GameObject boss, healthText, rageMeterText;
    private BossScript bossScript;
    //private Text healthText;

    void Awake(){
        bossScript = boss.GetComponent<BossScript>();

        healthText.GetComponent<Text>().color = Color.green;
        UpdateHealth();
        rageMeterText.GetComponent<Text>().color = Color.yellow;
        UpdateRageCounter();
    }
    public void UpdateHealth(){
        healthText.GetComponent<Text>().text = "Boss HP: " + boss.GetComponent<NPCVitalityHandler>().currentHealth;
    }

    public void UpdateRageCounter(){
        rageMeterText.GetComponent<Text>().text = "Rage: " + boss.GetComponent<BossScript>().rageCounter + " / "  + bossScript.maxRageCounter;
    }
    
}
