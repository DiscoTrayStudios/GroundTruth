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
    public TextMeshProUGUI journalBoxNineArticle;
    private List<TextMeshProUGUI> journalBoxes = new List<TextMeshProUGUI>();

    private int pageIndex;
    public GameObject PageOne;
    public GameObject PageTwo;
    public GameObject PageThree;
    public GameObject PageFour;
    private List<GameObject> Pages = new List<GameObject>();
    public GameObject userInterface;
    public GameObject openedNotebook;
    public AudioSource bookOpenSound;
    public AudioSource bookCloseSound;
    private bool firstOpen = false; 



    // Start is called before the first frame update
    void Start(){
        MakePages();
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
            testAddToJournal(GameManager.TestEvidenceList);
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

    public void testAddToJournal(List<TestEvidence> evidenceList){
        boxIndex = 0;
        print("Boxes" + journalBoxes.Count);
        foreach (var item in evidenceList)
        {
            print(boxIndex);
            print(item.test_evidence);
            // if(!item.test_collected){
            journalBoxes[boxIndex].text = item.test_evidence;
            if(boxIndex +1 < journalBoxes.Count){
                boxIndex = boxIndex + 1;
            } journalBoxNineArticle.text = ArticleManager.getArticle();
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
    }
}
