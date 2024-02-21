using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EForInteract : MonoBehaviour {

    public string[] text;

    private bool canShowDialog;

    private bool dialogShown;
    private bool mousePressed;

    public string evidence_name;
    public TestEvidence testEvi;

    private bool canClick = false;
    public GameObject exPoint;

    public bool whichDialogue;
    public bool talking;

    // private int currentTextIndex = 0;

    void Awake(){
        exPoint = gameObject.transform.Find("ExPoint").gameObject;   
        if (evidence_name != "") {
            if(GameManager.CheckEvidence(evidence_name)){
                // print("checking");
                exPoint.SetActive(false);
                print(exPoint.name);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collider2D) {
        if (collider2D.gameObject.CompareTag("Player") & !GameManager.Instance.GetPlayerBusy()) {
            canShowDialog = true;
            gameObject.GetComponent<NPCWander>().resetPath();
            StartCoroutine(WaitForStart());
        }
    }

    public void OnTriggerExit2D(Collider2D collider2D) {
        if (collider2D.gameObject.CompareTag("Player"))
        {
            canShowDialog = false;
            StopAllCoroutines();
        }
    }


    public void OnMouseEnter(){
        canClick = true;
        if(canShowDialog){
            GameManager.Instance.ChatCursor();    
        }
        
    }

    public void OnMouseExit(){
        canClick = false;
        GameManager.Instance.NormalCursor();
    }
    IEnumerator WaitForStart(){
        
        bool started = false;
        while(!started){
            if(Input.GetKeyDown(KeyCode.E)){
                started = true;
            }
            else if(Input.GetMouseButtonDown(0) && canClick){
                started = true;
            }
            yield return null;
        }
        GameManager.Instance.StartDialogue(text);
        dialogShown = true;
        if (gameObject.GetComponent<NPCWander>()!= null) {
            gameObject.GetComponent<NPCWander>().FaceFront();
            Camera.main.GetComponent<FollowCam>().enabled = false;
            Camera.main.GetComponent<ZoomCamera>().ZoomIn(transform.position);             
        }
        talking = true;
        if (evidence_name != "") { collect(); }   
        StartCoroutine(WaitForEnd());
    }
    IEnumerator WaitForEnd(){
        while(dialogShown & GameManager.Instance.GetPlayerBusy()){
            yield return null;
        }
        dialogShown = false;
        Camera.main.GetComponent<ZoomCamera>().UnZoom();
        talking = false;
        mousePressed = false;
        exPoint.SetActive(false);
        StopAllCoroutines();    
    }
    void Update() {
        /**if (Input.GetKeyDown(KeyCode.E))
        {
            // If dialog not currently shown and it can be, starts dialog.
            if (!GameManager.Instance.GetPlayerBusy() & !dialogShown & canShowDialog)
            {
                
            }**/
            
            /**else if (dialogShown)
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
                    Camera.main.GetComponent<ZoomCamera>().UnZoom();
                    
                    GameManager.Instance.DialogHide();
                    
                    currentTextIndex = 0;

                }
            }
            **/
        //}
    }

    private void collect() {
        // set text to something like "can I help you? and can repeat"
        // scribble.Play();
        if(!GameManager.CheckEvidence(evidence_name)){
            GameManager.Instance.AddEvidence(evidence_name);
            GameManager.Instance.GmCollectEvidence(testEvi);    
            ArticleManager.updateOrderedEvidenceSet(testEvi, whichDialogue);
        }
        else {
            string currDialogue = ArticleManager.getDialogues(ArticleManager.getEvidenceIndex(testEvi));
            if ((testEvi.dialogue ==  currDialogue && whichDialogue) || (testEvi.dialogue1 ==  currDialogue && !whichDialogue)) { 
                ArticleManager.bothDialogues(ArticleManager.getEvidenceIndex(testEvi), testEvi); 
            }
        }
        print("Evidence collected");
        //Journal.addToJournal(evidence_name);
        //testJournal.Instance.testAddToJournal(testEvi); 
    }

}
