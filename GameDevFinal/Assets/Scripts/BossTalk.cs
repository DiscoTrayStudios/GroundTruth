using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTalk : MonoBehaviour
{
public string[] text;

    private bool canShowDialog = true;

    public string evidence_name;

    private int currentTextIndex = 0;

    public AudioSource scribble;

    void Awake(){
        GameManager.Instance.GameDialogShow("Welcome to Ground Truth! (Press E to Continue)");
        GameManager.Instance.SetPlayerBusy(false);
    }

    void Update() {

        if (canShowDialog && Input.GetKeyDown(KeyCode.E))
        {
                GameManager.Instance.DialogHide();
                GameManager.Instance.GameDialogHide();
                
                if (currentTextIndex < text.Length)
                {
                    if(currentTextIndex == 0 || currentTextIndex == 14){
                        GameManager.Instance.GameDialogShow(text[currentTextIndex]);
                    }
                    else{
                        GameManager.Instance.DialogShow(text[currentTextIndex]);    
                    }
                    currentTextIndex++;
                }
                else
                {
                    //collect();
                    
                    GameManager.Instance.DialogShow(text[1]);
                    currentTextIndex = 2;
                }
        }
    }

  

}