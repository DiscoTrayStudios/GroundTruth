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

    public GameObject Title;
    public GameObject InvesArea;
    public GameObject PostQuakeInves;
    public GameObject Credits;
    public GameObject UI;
    public GameObject testNotebook;
    public GameObject BossUI;
    public GameObject PostUI;

    public TextMeshProUGUI totalText;
    private int total;
    public TextMeshProUGUI secondTotal;

    public TextMeshProUGUI dateText;
    public TextMeshProUGUI daysLeft;
    public TextMeshProUGUI postDateText;
    public TextMeshProUGUI postDaysLeft;
    public static List<TestEvidence> TestEvidenceList = new List<TestEvidence>();
    private static int days = 20;
    private static int month = 9;
    private static int pDays = 20;
    private static int pMonth = 5;
    private static int finalDay = 30;
    private static int pFinalDay = 30;

    public GameObject failureScreen;
    public GameObject postFailureScreen;

    private bool groundshake = false;

    public string currentScene    = "TitleScreen";
    public string currentLocation = "TitleScreen";


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
        total = 0;
        foreach (var evi in used_evidence.Values) {
            total += evi;
        }
        totalText.text = "Score: " + total.ToString();
        secondTotal.text = "Score: " + total.ToString();
        if (finalDay - days < 16) {
            daysLeft.text = "Days Left: " + (finalDay - days).ToString();
        } else {
            daysLeft.text = "Days Left: 0";
        }
        dateText.text = "Date: " + month + "/" + days.ToString() + "/1811";
        if (pFinalDay - pDays < 16) {
            postDaysLeft.text = "Days Left: " + (pFinalDay - pDays).ToString();
        } else {
            postDaysLeft.text = "Days Left: 0";
        }
        postDateText.text = "Date: " + pMonth + "/" + pDays.ToString() + "/1812";
        if (days > finalDay) {
            DeadlineMissed();
            days = days % 30;
            month = month + 1;
        }
        if (groundshake) {
            if (pDays > pFinalDay) {
                PostDeadlineMissed();
                pDays = pDays % 30;
                pMonth = pMonth + 1;
            }
        }
        if (currentScene == "InvestigativeArea") {
            if(Input.GetKeyDown(KeyCode.Q)) {
                ChangeScene(currentLocation);
            }
        }
    }

    public void AdddScoreKeeper(int n) {
        score_keeper.Add(n);
    }

    public void GmCollectEvidence(TestEvidence testevi){
        if(!testevi.test_collected){
            testevi.setCollected(true);
            TestEvidenceList.Add(testevi);
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

    public void ResetScene() {
        //GABBY UPDATE
        TestEvidenceList.Clear();
        //
        groundshake = false;
        RemoveAllEvidence();
        RemoveAllUsedEvidence();
        days = 20;
        month = 9;
        pDays = 20;
        pMonth = 5;
    }

    public void DialogShow(string text) {
        dialogBox.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(TypeText(text));
    }

    public void DialogHide(){
        dialogBox.SetActive(false);
    }

    IEnumerator TypeText(string text) {
        dialogText.text = "";
        foreach(char c in text.ToCharArray()) {
            dialogText.text += c;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public static void AddEvidence(string evi) {
        if (!evidence.ContainsKey(evi)) {
            evidence.Add(evi, true);
        } else {
            print("Already Added Evidence");
        }
    }

    public static bool CheckEvidence(string evi){
        print(evidence.ContainsKey(evi));
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
        } else {
            print("Already Added Evidence");
        }
    }

    public static void DeleteUsedEvidence(string evi) {
        if (used_evidence.ContainsKey(evi)) {
            used_evidence.Remove(evi);
        } else {
            print("That evidence is not being used");
        }
    }

    public static void RemoveAllUsedEvidence() {
        used_evidence.Clear();
    }

    public void AddDays(int n) {
        days = n + days;
    }

    public void PAddDays(int n) {
        pDays = n + pDays;
    }

    void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    IEnumerator LoadYourAsyncScene(string scene) {
        if (scene != "InvestigativeArea") {currentLocation = scene;}
        currentScene = scene;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while(!asyncLoad.isDone) {
            yield return null;
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
        else if (nextScene == "St.LouisPostQuakes") {PAddDays(1);}
        else if (nextScene == "RiverPostQuake"    ) {PAddDays(2);}
        else if (nextScene == "NewMadridPostQuake") {PAddDays(5);}
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
        } else if (scene == "InvestigativeArea") {
            
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
        } else if (scene == "NewMadridPreQuake" || scene == "St.LouisPreQuake" || scene == "St.LouisPostQuakes" || scene == "RiverPreQuake" || 
                scene == "RiverPostQuake" || scene == "NewMadridPostQuake") {
            dialogBox.SetActive(false);
            if (groundshake) {
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
            dialogBox.SetActive(true);
            UI.SetActive(false);
            testNotebook.SetActive(false);
            PostUI.SetActive(false);
            Title.SetActive(false);
            InvesArea.SetActive(false);
            PostQuakeInves.SetActive(false);
            Credits.SetActive(false);
            BossUI.SetActive(true);
        } else if (scene == "Cutscene") {
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
            InvesArea.SetActive(false);
            PostQuakeInves.SetActive(true);
            Credits.SetActive(false);
            BossUI.SetActive(false);
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
        }
    }

}