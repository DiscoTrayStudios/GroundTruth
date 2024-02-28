using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossTalk : MonoBehaviour
{
public string[] text;

    public string[] postText;

    private string[] usedPostText;

    // private bool canShowDialog = true;

    public string evidence_name;

    private int currentTextIndex = 0;

    private int prevScore = 0;

    private GameObject button;

    public GameObject nextButton;
    public GameObject backButton;
    private bool startedDialogue;
    public AudioSource speaking;

    void Awake(){
        currentTextIndex = 0;
        if(!GameManager.Instance.IsPost()){
            GameManager.Instance.GameDialogShow("Welcome to Ground Truth! (Press E to Continue)");    
        }
        else{
            prevScore = ArticleManager.getScoreNum();
            print(prevScore);
            speaking.Play();
            GameManager.Instance.DialogShow(postText[0]);
            usedPostText = new string[postText.Length/4 + 1];   
            usedPostText[0] = postText[0];
            int i = 1;
            for(int j = prevScore; j < postText.Length;j+= 4){
                usedPostText[i] = postText[j];
                print(usedPostText[i]);
                i++;
            }
            GameManager.Instance.StartDialogue(usedPostText);
        }
        //GameManager.Instance.SetPlayerBusy(false);
    
    }

    
    void Update() {
        if(Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)){
            if(!GameManager.Instance.IsPost() & !startedDialogue){
                if(currentTextIndex == 0){
                    GameManager.Instance.GameDialogShow("To advance through dialogue in game, press" + "\"" + "E" + "\"" + "or click, like you just did.");
                    currentTextIndex++;
                }
                else{
                    startedDialogue = true;
                    speaking.Play();
                    GameManager.Instance.StartDialogue(text);   
                    GameManager.Instance.GameDialogHide();
                }
                
                
            }
        }
        /**if (canShowDialog && Input.GetKeyDown(KeyCode.E))
        {
                GameManager.Instance.DialogHide();
                GameManager.Instance.GameDialogHide();
                if(!GameManager.Instance.IsPost()){
                    if (currentTextIndex < text.Length)
                    {
                        if(currentTextIndex == 0 || currentTextIndex == 14){
                            speaking.Stop();
                            GameManager.Instance.GameDialogShow(text[currentTextIndex]);
                        }  
                        
                        else{
                            if(currentTextIndex == 1){
                            speaking.Play();
                            }
                            GameManager.Instance.DialogShow(text[currentTextIndex]);    
                        }
                        if(currentTextIndex == 14){
                            button.GetComponent<Button>().enabled = true;
                            button.GetComponent<Image>().enabled = true;
                            button.transform.Find("Text").gameObject.SetActive(true);
                        }
                    
                    }
                    else
                    {
                    GameManager.Instance.DialogShow(text[1]);
                    speaking.Play();
                    currentTextIndex = 1;
                    }
                    currentTextIndex ++;
                }
                else{
                    if (currentTextIndex == 0){
                        currentTextIndex = prevScore;
                        GameManager.Instance.DialogShow(postText[currentTextIndex]);
                    }
                    else if(currentTextIndex <= postText.Length - 5){
                        currentTextIndex += 4;
                        GameManager.Instance.DialogShow(postText[currentTextIndex]);
                    }
                    else if(currentTextIndex == postText.Length - 1){
                        speaking.Play();
                        currentTextIndex = 0;
                        GameManager.Instance.DialogShow(postText[currentTextIndex]);
                        
                    }
                    else{
                        speaking.Stop();
                        GameManager.Instance.DialogHide();
                        currentTextIndex = postText.Length - 1;
                        GameManager.Instance.GameDialogShow(postText[currentTextIndex]);
                    }
                    if(currentTextIndex == postText.Length - 1){
                        button.GetComponent<Button>().enabled = true;
                        button.GetComponent<Image>().enabled = true;
                        button.transform.Find("Text").gameObject.SetActive(true);
                        GameManager.Instance.DialogHide();
                        GameManager.Instance.GameDialogShow(postText[currentTextIndex]);
                    }
                    print(currentTextIndex);
                }
                
                
        }**/
    }

  

}