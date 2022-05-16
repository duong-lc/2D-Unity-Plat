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
        }
    }

    private async void PrintTextSequence(){
        int waitTime = 0;

        try{
            if(count < dialogueArray.Length){

                switch (dialogueArray[count].character){
                    case characterToTalk.Player:
                        waitTime = macroFunction(playerDialogueTextTMP, dialogueArray[count].dialogueText);
                        break;
                    case characterToTalk.Golem:
                        waitTime = macroFunction(golemDialogueTextTMP, dialogueArray[count].dialogueText);
                        break;
                    case characterToTalk.Boss:
                        waitTime = macroFunction(bossDialogueTextTMP, dialogueArray[count].dialogueText);
                        break;
                }

                await Task.Delay(waitTime);


                switch (dialogueArray[count].character){
                    case characterToTalk.Player:
                        macroFunction(playerDialogueTextTMP, "");
                        break;
                    case characterToTalk.Golem:
                        macroFunction(golemDialogueTextTMP, "");    
                        break;
                    case characterToTalk.Boss:
                        macroFunction(bossDialogueTextTMP, "");
                        break;
                }
            

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

    private int macroFunction(TMP_Text characterTMP, string text){
        return TextEntered(text, characterTMP, dialogueArray[count].fullStringTimer, dialogueArray[count].endStringTimer);
    }

    private int TextEntered(string text, TMP_Text tmpComponent, float timer, float timerWaitAfterFin){
        if(this.gameObject.GetComponent<TypeWriterEffect>()){
            this.gameObject.GetComponent<TypeWriterEffect>().BeginEffect(text, tmpComponent, timer);
            return (int)((timer+timerWaitAfterFin)*1000);
        }
        return 0;
    }
}
    