using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTalk : MonoBehaviour
{
public string[] text;

    private bool canShowDialog = true;

    private bool dialogShown;

    public string evidence_name;

    private int currentTextIndex = 0;

    public AudioSource scribble;

    void Awake(){
        GameManager.Instance.DialogShow("Welcome to Ground Truth! (Press E to Continue)");
    }

    void Update() {

        if (canShowDialog && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogShown)
            {
                GameManager.Instance.DialogShow(text[currentTextIndex]);
                dialogShown = true;
            }

            else
            {
                GameManager.Instance.DialogHide();
                dialogShown = false;

                if (currentTextIndex < text.Length - 1)
                {
                    currentTextIndex++;
                    GameManager.Instance.DialogShow(text[currentTextIndex]);
                    dialogShown = true;
                }
                else
                {
                    //collect();
                    currentTextIndex = 0;
                }
            }
        }
    }

    private void collect() {
        // set text to something like "can I help you? and can repeat"
        scribble.Play();
        GameManager.AddEvidence(evidence_name);
        print("Evidence collected");
        Journal.addToJournal(evidence_name);
    }

}