using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour
{

    public static Journal Instance {get; private set;}

    public GameObject userInterface;
    public GameObject openedNotebook;
    public GameObject informationOne;
    public GameObject informationTwo;
    private static bool infoOne;
    private static bool infoTwo;

    public AudioSource bookOpenSound;
    public AudioSource bookCloseSound;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void openingJournal()
    {
        if(!GameManager.Instance.GetPlayerBusy()){
            GameManager.Instance.SetPlayerBusy(true);
            bookOpenSound.Play();
            userInterface.SetActive(false);
            openedNotebook.SetActive(true);
            if(infoOne == true){
            informationOne.SetActive(true);
            }
            if(infoTwo == true){
            informationTwo.SetActive(true);
            } 
        }

        
    }

    public void closingJournal()
    {
        bookCloseSound.Play();
        userInterface.SetActive(true);
        openedNotebook.SetActive(false);
        GameManager.Instance.SetPlayerBusy(false);
    }

    public static void addToJournal(string evid){
        print("Made it to addToJournal");
        print(evid);
        if(evid == "informationOne"){
            print("Passed the check");
            infoOne = true;
        }
        if(evid == "informationTwo"){
            print("Passed the check");
            infoTwo = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
