using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance {get; private set;}

    public static Dictionary<string, bool> evidence = new Dictionary<string, bool>();

    public static Dictionary<string, int> used_evidence = new Dictionary<string, int>();

    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;

    public GameObject Title;
    public GameObject InvesArea;
    public GameObject PostQuakeInves;
    public GameObject Credits;
    public GameObject UI;
    public GameObject BossUI;
    public GameObject PostUI;

    public TextMeshProUGUI totalText;
    private int total;
    public TextMeshProUGUI secondTotal;

    public TextMeshProUGUI dateText;
    public TextMeshProUGUI daysLeft;
    public TextMeshProUGUI postDateText;
    public TextMeshProUGUI postDaysLeft;
    private static int days = 20;
    private static int month = 9;
    private static int pDays = 20;
    private static int pMonth = 5;
    private static int finalDay = 30;
    private static int pFinalDay = 30;

    public GameObject failureScreen;
    public GameObject postFailureScreen;

    private bool groundshake = false;


    // Start is called before the first frame update
    void Start()
    {

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
        daysLeft.text = "Days Left: " + (finalDay - days).ToString();
        dateText.text = "Date: " + month + "/" + days.ToString() + "/1811";
        postDaysLeft.text = "Days Left: " + (pFinalDay - pDays).ToString();
        postDateText.text = "Date: " + pMonth + "/" + pDays.ToString() + "/1812";
        if (days > finalDay) {
            DeadlineMissed();
            days = 1;
            month = month + 1;
        }
        if (groundshake) {
            if (pDays > pFinalDay) {
                PostDeadlineMissed();
                pDays = 1;
                pMonth = pMonth + 1;
            }
        }
    }

    public void DeadlineMissed() {
        failureScreen.SetActive(true);
    }

    public void PostDeadlineMissed() {
        postFailureScreen.SetActive(true);
    }

    public void ResetScene() {
        RemoveAllEvidence();
        RemoveAllUsedEvidence();
        days = 1;
        month = 9;
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

    void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    IEnumerator LoadYourAsyncScene(string scene) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while(!asyncLoad.isDone) {
            yield return null;
        }
        // DialogHide();
        // if(scene != "Menu") {
        //     mainScreen.SetActive(false);
        // }
    }

    public void ChangeScene(string scene){
        print(scene);
        // RemoveAllUsedEvidence();
        StartCoroutine(LoadYourAsyncScene(scene));
        if (scene == "TitleScreen") {
            dialogBox.SetActive(false);
            UI.SetActive(false);
            PostUI.SetActive(false);
            Title.SetActive(true);
            InvesArea.SetActive(false);
            PostQuakeInves.SetActive(false);
            Credits.SetActive(false);
            BossUI.SetActive(false);
        } else if (scene == "InvestigativeArea") {
            groundshake = false;
            dialogBox.SetActive(false);
            UI.SetActive(false);
            PostUI.SetActive(false);
            Title.SetActive(false);
            InvesArea.SetActive(true);
            PostQuakeInves.SetActive(false);
            Credits.SetActive(false);
            BossUI.SetActive(false);
        } else if (scene == "GN_Test" || scene == "St.LouisPreQuake" || scene == "St.LouisPostQuakes" || scene == "RiverPreQuake" || 
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
            PostUI.SetActive(false);
            Title.SetActive(false);
            InvesArea.SetActive(false);
            PostQuakeInves.SetActive(false);
            Credits.SetActive(true);
            BossUI.SetActive(false);
        } else if (scene == "Boss") {
            dialogBox.SetActive(true);
            UI.SetActive(false);
            PostUI.SetActive(false);
            Title.SetActive(false);
            InvesArea.SetActive(false);
            PostQuakeInves.SetActive(false);
            Credits.SetActive(false);
            BossUI.SetActive(true);
        } else if (scene == "Cutscene") {
            dialogBox.SetActive(false);
            UI.SetActive(false);
            PostUI.SetActive(false);
            Title.SetActive(true);
            InvesArea.SetActive(false);
            PostQuakeInves.SetActive(false);
            Credits.SetActive(false);
            BossUI.SetActive(false);
        }
        else if (scene == "PostQuake") {
            groundshake = true;
            dialogBox.SetActive(false);
            UI.SetActive(false);
            PostUI.SetActive(false);
            Title.SetActive(false);
            InvesArea.SetActive(false);
            PostQuakeInves.SetActive(true);
            Credits.SetActive(false);
            BossUI.SetActive(false);
        } else {
            dialogBox.SetActive(false);
            UI.SetActive(false);
            PostUI.SetActive(false);
            Title.SetActive(true);
            InvesArea.SetActive(false);
            PostQuakeInves.SetActive(false);
            Credits.SetActive(false);
            BossUI.SetActive(false);
        }
    }

}