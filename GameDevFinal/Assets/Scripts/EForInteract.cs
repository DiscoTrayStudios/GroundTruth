using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EForInteract : MonoBehaviour {

    public string[] text;

    private bool canShowDialog;

    private bool dialogShown;

    public string evidence_name;
    public TestEvidence testEvi;

    public GameObject exPoint;

    private int currentTextIndex = 0;

    void Awake(){
        if(testEvi){
            exPoint = gameObject.transform.Find("ExPoint").gameObject;    
        }
        if(GameManager.CheckEvidence(evidence_name)){
            print("checking");
            exPoint.SetActive(false);
            print(exPoint.name);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider2D) {
        if (collider2D.gameObject.CompareTag("Player")) {
            canShowDialog = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider2D) {
        if (collider2D.gameObject.CompareTag("Player"))
        {
            canShowDialog = false;
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // If dialog not currently shown and it can be, starts dialog.
            if (!GameManager.Instance.GetPlayerBusy() & !dialogShown & canShowDialog)
            {
                GameManager.Instance.DialogShow(text[currentTextIndex]);
                dialogShown = true;
            }
            
            else if (dialogShown)
            {

                if (currentTextIndex < text.Length - 1)
                {
                    currentTextIndex++;
                    GameManager.Instance.DialogShow(text[currentTextIndex]);
                }
                else
                {
                    collect();
                    dialogShown = false;
                    GameManager.Instance.DialogHide();
                    currentTextIndex = 0;
                }
            }
        }
    }

    private void collect() {
        // set text to something like "can I help you? and can repeat"
        // scribble.Play();
        if(!GameManager.CheckEvidence(evidence_name)){
            GameManager.AddEvidence(evidence_name);
            GameManager.Instance.GmCollectEvidence(testEvi);    
        }
       
        exPoint.SetActive(false);
        print("Evidence collected");
        //Journal.addToJournal(evidence_name);
        //testJournal.Instance.testAddToJournal(testEvi); 
    }

}
