using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance {get; private set;}

    public EvidenceWatcher prequakeWatcher;
    public EvidenceWatcher postquakeWatcher;
    public static Dictionary<string, bool> evidence = new Dictionary<string, bool>();

    public static Dictionary<string, int> used_evidence = new Dictionary<string, int>();

    public static List<int> score_keeper = new List<int>();

    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;

    public GameObject gameDialogBox;
    public TextMeshProUGUI gameDialogText;

    public GameObject Title;
    public GameObject InvesArea;
    public GameObject PostQuakeInves;
    public GameObject Credits;
    public GameObject UI;
    public GameObject testNotebook;

    public GameObject journalPopup;
    public GameObject Timer;

    public GameObject Timerbox;
    public GameObject postTestNotebook;
    public GameObject BossUI;
    public GameObject PostUI;

    public GameObject TravelDisplay;

    public TextMeshProUGUI   firstArticle;
    public TextMeshProUGUI  secondArticle;
    public TextMeshProUGUI  firstFeedback;
    public TextMeshProUGUI secondFeedback;

    public TextMeshProUGUI dateText;

    public TextMeshProUGUI dueDateText;

    public TextMeshProUGUI daysLeft;
    public TextMeshProUGUI postDateText;
    public TextMeshProUGUI postDaysLeft;
    public static List<TestEvidence> TestEvidenceList = new List<TestEvidence>();
    private static int days = 20;
    private static int month = 9;

    private static int year = 1811;  

    private static int pDays = 20;
    private static int pMonth = 5;
    private static int finalDay = 30;
    private static int pFinalDay = 30;

    private static bool playerBusy = false;

    private static int daysTravel;

    public GameObject failureScreen;
    public GameObject postFailureScreen;

    private bool groundshake = false;

    public string currentScene    = "TitleScreen";
    public string currentLocation = "TitleScreen";
    public static bool post = false;

    public static bool timerGoing = false;
    public static string article  = "";
    private bool beenanywhere = false;
    private int haveEvidence = 0;
    //0 if no evidence, 1 if evidence has been added, 2 if the journal has appeared.
    public bool seenintro;
    public static string feedback = "";


    // Start is called before the first frame update
    void Start()
    {
        // testPieceOne.setEvidenceName("Houses are on fire");
        // testPieceOne.setEvidence("There are houses burning everywhere! Soon there may be nothing left!");
        // testPieceOne.setSummary("There are houses burning");
        // testPieceOne.setCollected(false);

        // testPieceOne.setEvidenceName("Churches are on fire");
        // testPieceOne.setEvidence("There are churches burning everywhere! Soon there may be nothing left!");
        // testPieceOne.setSummary("There are churches burning");
        // testPieceOne.setCollected(false);
    }

    // Update is called once per frame
    void Update()
    {
        firstArticle.text    = "\n \n Article: " + article;
        secondArticle.text   = "\n \n Article: " + article;
        firstFeedback.text   = "\n \n Feedback: " + feedback + "\n \n Score: " + ArticleManager.getScore().ToString();
        secondFeedback.text  = "\n \n Feedback: " + feedback + "\n \n Score: " + ArticleManager.getScore().ToString();
        if (finalDay - days < 16) {
            daysLeft.text = "Days Left: " + (finalDay - days).ToString();
        } else {
            daysLeft.text = "Days Left: 0";
        }
        dateText.text = "Date: " + month + "/" + days.ToString() + "/" + year;
        dueDateText.text = "Due: " + month + "/" + finalDay + "/" + year;    
        if (currentScene == "Boss") {
            if(Input.GetKey(KeyCode.T) && Input.GetKey(KeyCode.H) && Input.GetKey(KeyCode.C)) {
                BossUI.SetActive(false);
                DialogHide();
                GameDialogHide();
            }

        }

        /**if (groundshake) {
            if (pDays > pFinalDay) {
                PostDeadlineMissed();
                pDays = pDays % 30;
                pMonth = pMonth + 1;
            }
        }**/
      //  if (currentScene == "InvestigativeArea") {
      //      if(Input.GetKeyDown(KeyCode.Q)) {
      //          ChangeScene(currentLocation); This lets you go back for free
      //      }
      //  }
    }

    public void StartTime(){
        if(Timer.GetComponent<Timer>().Started == false){
            Timer.GetComponent<Timer>().StartTimer();
            timerGoing = true;
        }
    }
    public bool GetPlayerBusy(){
        return playerBusy;
    }

    public void SetPlayerBusy(bool p){
        playerBusy = p;
    }

    public void AddScoreKeeper(int n) {
        score_keeper.Add(n);
    }

    public void MakeFeedback() {
        feedback = ArticleManager.getFeedback();
    }

    public void GmCollectEvidence(TestEvidence testevi){
        if(!testevi.test_collected){
            testevi.setCollected(true);
            TestEvidenceList.Add(testevi);
            /*if(!haveEvidence){
                haveEvidence = true;
                Debug.Log("shouldopen");
                testNotebook.GetComponent<testJournal>().openingJournal();
            }*/
//            print(testevi.dialogue);         // literal dialogue
//            print(testevi.test_evidence);         // sentence
//            print(testevi.test_evidence_summary); // phrase
        }
        
    }

    public string GetCurrentScene() {
        return currentScene;
    }

    public string GetCurrentLocation() {
        return currentLocation;
    }

    public void DeadlineMissed() {
        failureScreen.SetActive(true);
    }

    public void PostDeadlineMissed() {
        postFailureScreen.SetActive(true);
    }

    public Boolean IsPost(){
        return post;
    }

    public void pre() {
        post = false;
        days = 20;
        month = 9;
        year = 1811;
        finalDay = 30;
    }

    public void SetPost(){
        post = true;
        days = pDays;
        month = pMonth;
        year = 1812;
        finalDay = pFinalDay;
    }

    public void ResetScene() {
        //GABBY UPDATE
        TestEvidenceList.Clear();
        print(TestEvidenceList.Count);
        //
        groundshake = false;
        RemoveAllEvidence();
        RemoveAllUsedEvidence();
        prequakeWatcher.ResetEvidence();
        postquakeWatcher.ResetEvidence();
        testNotebook.GetComponent<testJournal>().ResetJournal();
        print("cleared journal");
        postTestNotebook.GetComponent<testJournal>().ResetJournal();
        print("cleared journal");
        ArticleManager.resetArticleAndScore();
        article = "";
        
        days = 20;
        month = 9;
        pDays = 20;
        pMonth = 5;
    }
    public void StartDialogue(string[] text){
        dialogBox.SetActive(true);
        dialogBox.GetComponent<Dialogue>().StartDialogue(text);
    }
    public void DialogShow(string text) {
        dialogBox.SetActive(true);
        StopAllCoroutines();
        playerBusy = true;
        StartCoroutine(TypeText(dialogText, text));
    }

    public void DialogHide(){
        dialogBox.SetActive(false);
        playerBusy = false;
        if(haveEvidence == 1){
            testNotebook.GetComponent<testJournal>().openingJournal();
            testNotebook.GetComponent<testJournal>().flipToPage(1);

            haveEvidence = 2;
        }
    }

    public void FirstTown() {
        if (!beenanywhere) {
            testNotebook.GetComponent<testJournal>().openingJournal();
        } 
    }

    public void GameDialogShow(string text) {
        gameDialogBox.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(TypeText(gameDialogText, text));
        playerBusy = true;
    }
    public void GameDialogHide(){
        gameDialogBox.SetActive(false);
        playerBusy = false;
    }

    public void SkipTypeText(TextMeshProUGUI textBox, string text){
        StopAllCoroutines();
        textBox.text = text;
    }
    IEnumerator TypeText(TextMeshProUGUI textBox, string text) {
        textBox.text = "";
        foreach(char c in text.ToCharArray()) {
            textBox.text += c;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void AddEvidence(string evi) {
        if (!evidence.ContainsKey(evi)) {
            evidence.Add(evi, true);
            journalPopup.GetComponent<JournalPopup>().Notify();
        } 
        if(haveEvidence == 0){
                haveEvidence = 1;
        }
    }

    public static bool CheckEvidence(string evi){
        // print(evidence.ContainsKey(evi));
        return evidence.ContainsKey(evi);
    }

    public static void RemoveAllEvidence() {
        evidence.Clear();
        print("Evidence Removed");
        print(evidence.Count);
        
    }

    public static void AddUsedEvidence(string evi, int score) {
        if (!used_evidence.ContainsKey(evi)) {
            used_evidence.Add(evi, score);
            article = ArticleManager.updateArticle(evi,false);
        } else {
        }
    }

    public static void DeleteUsedEvidence(string evi) {
        if (used_evidence.ContainsKey(evi)) {
            used_evidence.Remove(evi);
            article = ArticleManager.updateArticle(evi,true);
        } else {
            print("That evidence is not being used");
        }
    }

    public static void RemoveAllUsedEvidence() {
        used_evidence.Clear();
    }

    public void AddDays(int n) {
        days = n + days;
        TravelDisplay.GetComponent<TravelDisplay>().daysTravel = n/2;
    }

    public void PAddDays(int n) {
        pDays = n + pDays;
        TravelDisplay.GetComponent<TravelDisplay>().daysTravel = n/2;
    }

    void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void BackToOffice(){
        SetPlayerBusy(false);
        if(!post){
            ChangeScene("InvestigativeArea");
        }
        else{
            ChangeScene("PostQuake");
        }
        TravelDisplay.GetComponent<TravelDisplay>().ShowTraveling("desk");
    }

    IEnumerator LoadYourAsyncScene(string scene) {
        if (scene != "InvestigativeArea") {currentLocation = scene;}
        currentScene = scene;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while(!asyncLoad.isDone) {
            yield return null;
        }
        if(scene == "Boss"){
            BossUI.transform.Find("SkipButton").gameObject.SetActive(true);
        }
        // DialogHide();
        // if(scene != "Menu") {
        //     mainScreen.SetActive(false);
        // }
    }

    public void TimeManager(string lastScene, string nextScene) { 
        if (lastScene == nextScene) {print("No Travel Cost");}
        else if (nextScene == "St.LouisPreQuake"  ) {AddDays(1);}
        else if (nextScene == "RiverPreQuake"     ) {AddDays(2);}
        else if (nextScene == "NewMadridPreQuake" ) {AddDays(5);}
        else if (nextScene == "St.LouisPostQuakes") {AddDays(1);}
        else if (nextScene == "RiverPostQuake"    ) {AddDays(2);}
        else if (nextScene == "NewMadridPostQuake") {AddDays(5);}
    }


    public void ChangeScene(string scene){
        TimeManager(currentLocation,scene);
        StartCoroutine(LoadYourAsyncScene(scene));
        // RemoveAllUsedEvidence();
        if (scene == "TitleScreen") {            
            dialogBox.SetActive(false);
            UI.SetActive(false);
            PostUI.SetActive(false);
            Title.SetActive(true);
            InvesArea.SetActive(false);
            PostQuakeInves.SetActive(false);
            Credits.SetActive(false);
            BossUI.SetActive(false);
            testNotebook.SetActive(false);
            SetPlayerBusy(false);
        } else if (scene == "InvestigativeArea") {
            timerGoing = false;
            groundshake = false;
            dialogBox.SetActive(false);
            UI.SetActive(false);
            testNotebook.SetActive(false);
            PostUI.SetActive(false);
            Title.SetActive(false);
            InvesArea.SetActive(true);
            PostQuakeInves.SetActive(false);
            Credits.SetActive(false);
            BossUI.SetActive(false);
            Timerbox.SetActive(false);
            SetPlayerBusy(false);
        } else if (scene == "NewMadridPreQuake" || scene == "St.LouisPreQuake" || scene == "St.LouisPostQuake" || scene == "RiverPreQuake" || 
                scene == "RiverPostQuake" || scene == "NewMadridPostQuake") {
            beenanywhere = true;
            dialogBox.SetActive(false);
            Timerbox.SetActive(true);
            if (post) {
                PostUI.SetActive(true);
                UI.SetActive(false);
            } else {
                UI.SetActive(true);
                PostUI.SetActive(false);
            }
            Title.SetActive(false);
            InvesArea.SetActive(false);
            PostQuakeInves.SetActive(false);
            Credits.SetActive(false);
            BossUI.SetActive(false);
            StartTime();
        } else if (scene == "Credits") {
            dialogBox.SetActive(false);
            UI.SetActive(false);
            testNotebook.SetActive(false);
            PostUI.SetActive(false);
            Title.SetActive(false);
            InvesArea.SetActive(false);
            PostQuakeInves.SetActive(false);
            Credits.SetActive(true);
            BossUI.SetActive(false);
        } else if (scene == "Boss") {
            //dialogBox.SetActive(true);
            UI.SetActive(false);
            ArticleManager.resetArticleAndScore();
            testNotebook.SetActive(false);
            PostUI.SetActive(false);
            Title.SetActive(false);
            InvesArea.SetActive(false);
            PostQuakeInves.SetActive(false);
            Credits.SetActive(false);
            BossUI.SetActive(true);
            SetPlayerBusy(false);
            BossUI.transform.Find("SkipButton").gameObject.SetActive(false);
        } else if (scene == "Cutscene") {
            ArticleManager.resetArticleAndScore();
            ResetScene();
            SetPost();
            currentLocation = "InvestigativeArea";
            dialogBox.SetActive(false);
            UI.SetActive(false);
            testNotebook.SetActive(false);
            PostUI.SetActive(false);
            Title.SetActive(false);
            InvesArea.SetActive(false);
            PostQuakeInves.SetActive(false);
            Credits.SetActive(false);
            BossUI.SetActive(false);
        }
        else if (scene == "PostQuake") {
            //GameManager.TestEvidenceList.Clear();
            print("New scene..");
            groundshake = true;
            dialogBox.SetActive(false);
            UI.SetActive(false);
            testNotebook.SetActive(false);
            PostUI.SetActive(false);
            Title.SetActive(false);
            InvesArea.SetActive(true);
            //PostQuakeInves.SetActive(true);
            Credits.SetActive(false);
            BossUI.SetActive(false);
            Timerbox.SetActive(false);
            SetPlayerBusy(false);
        } else {
            dialogBox.SetActive(false);
            UI.SetActive(false);
            testNotebook.SetActive(false);
            PostUI.SetActive(false);
            Title.SetActive(true);
            InvesArea.SetActive(false);
            PostQuakeInves.SetActive(false);
            Credits.SetActive(false);
            BossUI.SetActive(false);
            Timerbox.SetActive(false);
        }
    }

}