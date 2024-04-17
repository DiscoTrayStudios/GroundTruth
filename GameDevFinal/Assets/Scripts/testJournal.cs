using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI; 
using UnityEngine;

public class testJournal : MonoBehaviour
{

    public static testJournal Instance {get; private set;}

    private int boxIndex;
    public TextMeshProUGUI journalBoxOne;
    public TextMeshProUGUI journalBoxTwo;
    public TextMeshProUGUI journalBoxThree;
    public TextMeshProUGUI journalBoxFour;
    public TextMeshProUGUI journalBoxFive;
    public TextMeshProUGUI journalBoxSix;
    public TextMeshProUGUI journalBoxSeven;
    public TextMeshProUGUI journalBoxEight;
    public TextMeshProUGUI journalBoxZero;
    public TextMeshProUGUI journalBoxNineArticle;
    private List<TextMeshProUGUI> journalBoxes = new List<TextMeshProUGUI>();

    private int pageIndex;

    public GameObject Exclaim;
    public GameObject PageOne;
    public GameObject PageTwo;
    public GameObject PageThree;
    public GameObject PageFour;
    public GameObject LeftTab;
    public GameObject RightTab;
    private List<GameObject> Pages = new List<GameObject>();
    public GameObject userInterface;
    public GameObject openedNotebook;
    public AudioSource bookOpenSound;
    public AudioSource bookCloseSound;
    private Dictionary<string, string> journalEntries;
    private bool firstOpen = false; 



    // Start is called before the first frame update
    void Start(){
        MakePages();
        journalEntries = new Dictionary<string, string>(){
            {"NewMadridPreQuake",  "I went to New Madrid. It was a very small town, but it had some charm."},
            {"RiverPreQuake",      "I found a small community who lived at a nearby bank of the Mississippi River."},
            {"StLouisPreQuake",    "I spent some time in my neck of the woods, St. Louis."},
            {"NewMadridPostQuake", "New Madrid was hit hard by the quake. The people there were wounded, but their stunning resilience filled me with an unexpected hope."},
            {"RiverPostQuake",     "At the River, the quake was so intense that residents had a tough time telling fact from fiction - I don't blame them."},
            {"StLouisPostQuake",   "Despite being far from the epicenter, the earthquake was very destructive in St. Louis."}
        };
    }
    void MakePages()
    {
        if(!firstOpen){
        print("Making Journal!");
        boxIndex = 0;
        pageIndex = 0;
        journalBoxes.Add(journalBoxOne);
        journalBoxes.Add(journalBoxTwo);
        journalBoxes.Add(journalBoxThree);
        journalBoxes.Add(journalBoxFour);
        journalBoxes.Add(journalBoxFive);
        journalBoxes.Add(journalBoxSix);
        journalBoxes.Add(journalBoxSeven);
        journalBoxes.Add(journalBoxEight);
        journalBoxes.Add(journalBoxZero);

        Pages.Add(PageOne);
        Pages.Add(PageTwo);
        Pages.Add(PageThree);
        Pages.Add(PageFour);
        firstOpen = true;
        }
    }
    public void openingJournal()
    {
        if(!GameManager.Instance.GetPlayerBusy()){
            GameManager.Instance.SetPlayerBusy(true);
            MakePages();
            userInterface.SetActive(false);
            openedNotebook.SetActive(true);
            bookOpenSound.Play();
            testAddToJournal(GameManager.seenScenes, GameManager.TestEvidenceList);
        //PageOne.SetActive(true);
        }
    }

    public void closingJournal()
    {
        bookCloseSound.Play();
        userInterface.SetActive(true);
        openedNotebook.SetActive(false);
        GameManager.Instance.SetPlayerBusy(false);
    }

    public void testAddToJournal(List<string> seenScenes, List<TestEvidence> evidenceList){
        boxIndex = 0;
        print("Boxes" + journalBoxes.Count);
        foreach (string scene in seenScenes) {
            
        }
        foreach (var item in evidenceList) {
            print(boxIndex);
            print(item.test_evidence);
            // if(!item.test_collected){
            journalBoxes[boxIndex].text = ArticleManager.getDialogues(boxIndex);
            if (boxIndex +1 < journalBoxes.Count) {
                boxIndex = boxIndex + 1;
            } journalBoxNineArticle.text = "Current Article Draft: \n" + ArticleManager.getArticle();
            // item.test_collected = true;
        }
    }

    public void ResetJournal(){
        foreach (var item in journalBoxes)
        {
            item.text = " ";
        }
        boxIndex = 0;
        pageIndex = 0;
    }

    public void Notify(){
        Debug.Log("nnot");
        StartCoroutine("JournalNotify");
        StartCoroutine("JournalNotify");
    }
    IEnumerator JournalNotify(){
        Exclaim.SetActive(true);
        float time = 0;
        float duration = 1.5f;
        Color startValue = new Color (1,1,1,1);
        Color endValue = new Color(1, 1, 1, 0);
        Image image = Exclaim.GetComponent<Image>();

        while (time < duration)
        {
            image.color = Color.Lerp(startValue, endValue, time / duration);
            time+= Time.deltaTime;
            yield return null;
        }
        Exclaim.SetActive(false);
    }

    public void testFlipRightPage(int pageNum){
        if (pageIndex > 0 && pageNum == -1){
            Pages[pageIndex].SetActive(false);
            pageIndex -= 1;
            Pages[pageIndex].SetActive(true);
            
        }
        if (pageIndex +1 < Pages.Count && pageNum == 1){
            Pages[pageIndex].SetActive(false);
            pageIndex += 1;
            Pages[pageIndex].SetActive(true);
            
        }
    }
    public void flipToPage(int pIndex){
        Pages[pageIndex].SetActive(false);
        pageIndex = pIndex;
        Pages[pageIndex].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) {
            if (openedNotebook.activeSelf) {
                bookCloseSound.Play();
                userInterface.SetActive(true);  
                openedNotebook.SetActive(false);
                GameManager.Instance.SetPlayerBusy(false);
            }
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (openedNotebook.activeSelf) {
                testFlipRightPage(-1);
            }
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow)) {
            if (openedNotebook.activeSelf) {
                testFlipRightPage(1);
            }
        }
        if(pageIndex == 0){
            LeftTab.SetActive(false);
        }
        else{
            LeftTab.SetActive(true);
        }
        if(pageIndex == Pages.Count -1){
            RightTab.SetActive(false);
        }
        else{
            RightTab.SetActive(true);
        }
    }
}
