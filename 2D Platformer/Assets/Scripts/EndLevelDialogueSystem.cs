using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System;

public class EndLevelDialogueSystem : MonoBehaviour
{
    public enum characterToTalk{
        Player, Golem, Boss
    }
    [Serializable]
    public class DialogueTypeCustom{
        public characterToTalk character;
        public string dialogueText;
        public float fullStringTimer;
        public float endStringTimer;
    }


    [SerializeField] private BoxCollider2D triggerBoxActivation;
    [SerializeField] private TMP_Text playerDialogueTextTMP, golemDialogueTextTMP, bossDialogueTextTMP;
    [SerializeField] private DialogueTypeCustom[] dialogueArray;
    private bool playOnce = true;
    private bool playerIsInField = false;

    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {   
        playOnce = true;
    }


    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            count = 0;
            playerIsInField = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            playerIsInField = false;
        }
    }
    
    void Update() {
        if(Input.GetKeyDown(KeyCode.E) && playerIsInField && playOnce)
        {
            print($"print text typer");
            PrintTextSequence();
            playOnce = false;
            Destroy(GetComponent<Collider2D>());
        }
    }

    private async void PrintTextSequence(){
        int waitTime = 0;

        try{
            if(count < dialogueArray.Length){
                
                //Pass in the desired dialogue for current text box
                switch (dialogueArray[count].character){
                    case characterToTalk.Player:
                        waitTime = MacroFunction(playerDialogueTextTMP, dialogueArray[count].dialogueText);
                        break;
                    case characterToTalk.Golem:
                        waitTime = MacroFunction(golemDialogueTextTMP, dialogueArray[count].dialogueText);
                        break;
                    case characterToTalk.Boss:
                        waitTime = MacroFunction(bossDialogueTextTMP, dialogueArray[count].dialogueText);
                        break;
                }
                
                //wait for text box to finish prompting
                await Task.Delay(waitTime);

                //Reset the string in the text box
                switch (dialogueArray[count].character){
                    case characterToTalk.Player:
                        MacroFunction(playerDialogueTextTMP, "");
                        break;
                    case characterToTalk.Golem:
                        MacroFunction(golemDialogueTextTMP, "");    
                        break;
                    case characterToTalk.Boss:
                        MacroFunction(bossDialogueTextTMP, "");
                        break;
                }
                
                //Wait for text box to finish loading then load the win screen
                if(count == dialogueArray.Length - 1)
                    print($"this is the end, turn on win screen");
            

                count++;

                PrintTextSequence();
            }
        }
        catch(Exception e){
            Debug.Log("exception caught");
        }
        
        
        
        // if (count >= dialogueArray.Length)
        //     playOnce = false;

    }

    private int MacroFunction(TMP_Text characterTMP, string text){
        return TextEntered(text, characterTMP, dialogueArray[count].fullStringTimer, dialogueArray[count].endStringTimer);
    }

    private int TextEntered(string text, TMP_Text tmpComponent, float timer, float timerWaitAfterFin){
        if(GetComponent<TypeWriterEffect>())
        {
            GetComponent<TypeWriterEffect>().BeginEffect(text, tmpComponent, timer);
            return (int)((timer+timerWaitAfterFin)*1000);
        }
        return 0;
    }
}
    