using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Select_UI : MonoBehaviour
{
    public Text katanaText;
    public Text archerText;
    public Text heavyText;
    public Text mageText;
    public PlayerBehavior playerScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update() 
    {
        if(!playerScript.isKatanaAlive)
            katanaText.color = Color.red;
        if(!playerScript.isArcherAlive)
            archerText.color = Color.red;
        if(!playerScript.isHeavyAlive)
            heavyText.color = Color.red;
        if(!playerScript.isMageAlive)
            mageText.color = Color.red;
    }


    public void SwitchCharacter()
    {
        if(playerScript.currentCharacter == 1)
            ActivateKatanaText();
        if(playerScript.currentCharacter == 2)
            ActivateArcherText();
        if(playerScript.currentCharacter == 3)
            ActivateHeavyText();
        if(playerScript.currentCharacter == 4)
            ActivateMageText();
    }

    private void ActivateKatanaText()
    {
        katanaText.color = Color.green;
        archerText.color = Color.gray;
        heavyText.color = Color.gray;
        mageText.color = Color.gray;
    }

    private void ActivateArcherText()
    {
        katanaText.color = Color.gray;
        archerText.color = Color.green;
        heavyText.color = Color.gray;
        mageText.color = Color.gray;
    }

    private void ActivateHeavyText()
    {
        katanaText.color = Color.gray;
        archerText.color = Color.gray;
        heavyText.color = Color.green;
        mageText.color = Color.gray;
    }

    private void ActivateMageText()
    {
        katanaText.color = Color.gray;
        archerText.color = Color.gray;
        heavyText.color = Color.gray;
        mageText.color = Color.green;
    }


}
