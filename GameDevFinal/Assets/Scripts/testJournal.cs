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
    }

    void MakePages()
    {
        if(!firstOpen) {
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
            // No more test article
            //Pages.Add(PageFour);
            journalEntries = new Dictionary<string, string>(){
                {"NewMadridPreQuake",  "I went south, to New Madrid. It's a very small town, way out in the boothill. The land there is in constant flux, having been run by the Spanish, French, Native, and Missourian governments in the last twenty years. It's about twenty minutes from the confluence of the Ohio and Mississippi Rivers - maybe I'll visit if I have time. Some locals warn of risk of earthquakes..."},
                {"RiverPreQuake",      "I found a small community who lived at a nearby bank of the Mississippi River. I've heard there's an automated boat, which propels itself with steam power, making its way down the river at present. If I'm lucky, I'll find it, and write about it in the article."},
                {"St.LouisPreQuake",   "I spent some time in my neck of the woods, St. Louis. I've lived here about five years now, which means I never experienced Spanish or French rule. The city is growing rapidly: there are talks of declaring it the capital of the entire Louisiana Territory, which takes up almost half of U.S.-owned land."},
                {"Cutscene",           "A devastating earthquake hit a couple days ago. The roof of my house, in St. Louis, almost caved in from a falling tree. I'm lucky that my family and I survived. My new assignment at the paper is decidedly less open-ended. I need to tell the story of this earthquake, of the lives lost and upended. I've found that its epicenter was New Madrid: going there is a top priority."},
                {"NewMadridPostQuake", "New Madrid was hit hardest by the quake. The people there were wounded, but their stunning resilience filled me with an unexpected hope."},
                {"RiverPostQuake",     "At the River, the quake was so intense that residents had a tough time telling fact from fiction - I don't blame them. Rivers are prone to unexpected behavior, especially during something as intense as this. Some say that rivers can flow backward, during a strong enough earthquake."},
                {"St.LouisPostQuake",  "Despite being far from the epicenter, the earthquake was very destructive in St. Louis. Meriwether Lewis, the explorer, was our governor for years, starting in 1807. "}
            };

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
            testAddToJournal(GameManager.Instance.getSeenScenes(), GameManager.Instance.getTestEvidenceList());
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
        boxIndex = 1;
        if (journalBoxes.Count > 0) {
            journalBoxes[0].text = "A lot of people think the Ozarks are boring, and that the people here have nothing to say. If that were true, I'd be unemployed. Luckily, a lot happens out here. My task, this next week and a half, is to pull together an article about something interesting and particular to our region. This should be fun.";
            print("Boxes" + journalBoxes.Count);
            journalBoxNineArticle.text = "Current Article Draft: \n" + ArticleManager.getArticle();
            foreach (var item in evidenceList) {
                print(boxIndex);
                print(item.test_evidence);
                // if(!item.test_collected){
                // journalBoxes[boxIndex].text = ArticleManager.getDialogues(boxIndex);
                // if (boxIndex +1 < journalBoxes.Count) {
                //     boxIndex = boxIndex + 1;
                // } 

                item.test_collected = true;
            }
            foreach (var scene in seenScenes) {
                journalBoxes[boxIndex].text = journalEntries[scene];
                if (boxIndex + 1 < journalBoxes.Count) {
                    boxIndex++;
                } 
            }
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
        print("pagesIndex = " + pageIndex);
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
