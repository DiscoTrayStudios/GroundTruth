// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class EForInteract : MonoBehaviour {

//     public string text;

//     private bool canCollectClue;

//     public string evidence_name;

//     private bool dialogShown = false;

//     public void OnTriggerEnter2D(Collider2D collider2D) {
//         print("Entered..");
//         print("Press E to Interact.");
//         if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return)) {
//             if (collider2D.gameObject.CompareTag("Player")) {
//                 // GameManager.Instance.DialogShow(text);
//                 canCollectClue = true;
//             }
//         }
//     }

//     public void OnTriggerExit2D(Collider2D collider2D) {
//         if (collider2D.gameObject.CompareTag("Player"))
//         {
//             // GameManager.Instance.DialogHide();
//             canCollectClue = false;
//         }
//     }

//     void Start() {

//     }

//     void Update() {
//         if (canCollectClue && Input.GetKeyDown(KeyCode.E))
//         {
//             if (!dialogShown)
//             {
//                 GameManager.Instance.DialogShow(text);
//                 dialogShown = true;
//             }
//             else
//             {
//                 collect();
//                 GameManager.Instance.DialogHide();
//                 dialogShown = false;
//             }
//         }
//     }

    // private void collect() {
    //     // set text to something like "can I help you? and can repeat"
    //     // add evidence to collection
    //     GameManager.AddEvidence(evidence_name);
    //     print("Evidence collected");
    // }


// }

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

    // public AudioSource scribble;
    

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
            GameManager.Instance.DialogHide();
            canShowDialog = false;
            currentTextIndex = 0;
        }
    }

    void Update() {
        if (canShowDialog && Input.GetKeyDown(KeyCode.E))
        {
            
            if (!dialogShown)
            {
                GameManager.Instance.DialogShow(text[currentTextIndex]);
                dialogShown = true;
            }
            // else
            // {
            //     GameManager.Instance.DialogHide();
            //     dialogShown = false;
            //     collect();
            // }
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
                    collect();
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
