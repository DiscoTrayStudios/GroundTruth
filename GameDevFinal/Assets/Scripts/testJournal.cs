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

    // Start is called before the first frame update
    void Start()
    {
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

        print(pageIndex);
        Pages.Add(PageOne);
        Pages.Add(PageTwo);
        Pages.Add(PageThree);
        Pages.Add(PageFour);
        print(pageIndex);
        
    }
    public void openingJournal()
    {
        bookOpenSound.Play();
        userInterface.SetActive(false);
        openedNotebook.SetActive(true);
        testAddToJournal(GameManager.TestEvidenceList);
        //PageOne.SetActive(true);
    }

    public void closingJournal()
    {
        bookCloseSound.Play();
        userInterface.SetActive(true);
        openedNotebook.SetActive(false);
    }

    public void testAddToJournal(List<TestEvidence> evidenceList){
        boxIndex = 0;
        foreach (var item in evidenceList)
        {
            // if(!item.test_collected){
            journalBoxes[boxIndex].text = item.test_evidence;
            if(boxIndex +1 < journalBoxes.Count){
                boxIndex = boxIndex + 1;
            }
            // item.test_collected = true;
            } 
               
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
        
    }
}
