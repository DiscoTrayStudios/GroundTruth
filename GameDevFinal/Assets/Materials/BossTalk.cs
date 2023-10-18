using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTalk : MonoBehaviour
{
public string[] text;

    public string[] postText;

    private bool canShowDialog = true;

    public string evidence_name;

    private int currentTextIndex = 0;

    private int prevScore = 0;

    public AudioSource scribble;

    void Awake(){
        if(!GameManager.Instance.IsPost()){
            GameManager.Instance.GameDialogShow("Welcome to Ground Truth! (Press E to Continue)");    
        }
        else{
            GameManager.Instance.DialogShow(postText[currentTextIndex]);
            currentTextIndex++;
        }
        GameManager.Instance.SetPlayerBusy(false);
    }

    void Update() {

        if (canShowDialog && Input.GetKeyDown(KeyCode.E))
        {
                GameManager.Instance.DialogHide();
                GameManager.Instance.GameDialogHide();
                if(!GameManager.Instance.IsPost()){
                    if (currentTextIndex < text.Length)
                    {
                    if(currentTextIndex == 0 || currentTextIndex == 14){
                        GameManager.Instance.GameDialogShow(text[currentTextIndex]);
                    }
                    else{
                        GameManager.Instance.DialogShow(text[currentTextIndex]);    
                    }
                    
                    }
                    else
                    {
                    GameManager.Instance.DialogShow(text[1]);
                    currentTextIndex = 1;
                    }
                }
                else{
                    if (currentTextIndex < postText.Length-1){
                        GameManager.Instance.DialogShow(postText[currentTextIndex]);
                    }
                    else if(currentTextIndex == postText.Length - 1){
                        GameManager.Instance.GameDialogShow(postText[currentTextIndex]);
                    }
                    else
                    {
                    GameManager.Instance.DialogShow(postText[0]);
                    currentTextIndex = 0;
                    }
                }
                currentTextIndex++;
                
        }
    }

  

}