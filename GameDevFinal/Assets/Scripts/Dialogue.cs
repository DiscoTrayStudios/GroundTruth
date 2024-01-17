using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Dialogue : MonoBehaviour {
    public string[] text;
   
    public TextMeshProUGUI dialogueText;

    public GameObject backButton;
    public GameObject nextButton;
    public TextMeshProUGUI nextButtonText;
    private bool DialogueHappening = false;

    

    public int textIndex;

    public void StartDialogue(string[] newtext){
        GameManager.Instance.SetPlayerBusy(true);
        textIndex = 0;
        text = newtext;
        GameManager.Instance.DialogShow(text[textIndex]);
        DialogueHappening = true;
        StartCoroutine(InDialogue());

    }

    public void NextText(){
        if(textIndex == text.Length - 1){
            DialogueHappening = false;
            nextButtonText.text = "Next";
            GameManager.Instance.DialogHide();
        }
        else{
            if(textIndex == text.Length - 2){
                nextButtonText.text = "Done";
            }
            textIndex++;
            GameManager.Instance.DialogShow(text[textIndex]);  
        }
        
    }

    public void BackText(){
        if(textIndex > 0){
            textIndex--;
            GameManager.Instance.DialogShow(text[textIndex]);
            nextButtonText.text = "Next";  
        }
        
    }

    IEnumerator InDialogue(){
        while(DialogueHappening){
            if(Input.GetKeyDown(KeyCode.E)){
                if(dialogueText.text.Equals(text[textIndex])){
                    NextText();
                }
                else{
                    GameManager.Instance.SkipTypeText(dialogueText, text[textIndex]);
                }
            }
            yield return null;
        }
        
    }
}