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
            GameManager.Instance.DialogShow(postText[currentTextIndex]);
        }
        GameManager.Instance.SetPlayerBusy(false);
        GameManager.Instance.BossUI.transform.Find("SkipButton").gameObject.SetActive(true);
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
                            speaking.Stop();
                            GameManager.Instance.GameDialogShow(text[currentTextIndex]);
                        }  
                        
                        else{
                            if(currentTextIndex == 1){
                            speaking.Play();
                            }
                            GameManager.Instance.DialogShow(text[currentTextIndex]);    
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
                    print(currentTextIndex);
                }
                
                
        }
    }

  

}