using System;
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

    private List<Text> textList = new List<Text>();
    // Start is called before the first frame update
    void Start()
    {
        textList = new List<Text>()
        {
            katanaText,
            archerText,
            heavyText,
            mageText
        };
    }

    private void LateUpdate() 
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
        switch (playerScript.currentCharacter)
        {
            case CurrentCharacter.Katana:
                ActivateText(katanaText);
                break;
            case CurrentCharacter.Archer:
                ActivateText(archerText);
                break;
            case CurrentCharacter.Heavy:
                ActivateText(heavyText);
                break;
            case CurrentCharacter.Mage:
                ActivateText(mageText);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void ActivateText(Text activateText)
    {
        foreach (Text text in textList)
        {
            text.color = activateText == text ? Color.green : Color.gray;
        }
    }
}
