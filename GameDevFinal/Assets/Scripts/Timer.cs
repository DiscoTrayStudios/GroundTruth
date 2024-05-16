using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{   
    public TextMeshProUGUI TimeText;

    public GameObject TimeBox;
    public int TimeLeft;

    private int hours;

    private int prevHours;
    private int seconds;
    public int TimeLimit = 90;
    public bool Started;

    private bool firstCheck = true;

    public AudioSource chime;

    // Start is called before the first frame update
    void Start()
    {
       Started = false;
       TimeText = gameObject.GetComponent<TextMeshProUGUI>();
       TimeLeft = TimeLimit; 
       firstCheck = true;
       prevHours = hours;
    }
    public void StartTimer(){
        if(Started == false){
            TimeLeft = TimeLimit;
            firstCheck = true;
            UpdateTime();
            firstCheck = true;
            Debug.Log("start");
            StartCoroutine(RunTimer());
            Started = true;  
        }
    }

    public void StopTimer(){
        Started = false;
        StopCoroutine(RunTimer());
        TimeLeft = TimeLimit;
        firstCheck = true;
        TimeBox.transform.localScale = Vector3.one;
    }
    public void UpdateTime(){
        /*minutes = TimeLeft/60;
        seconds = TimeLeft - (minutes*60);
        TimeText.text = minutes + ":";
        if(seconds < 10){
            TimeText.text += "0";
        }
        TimeText.text += seconds;*/
        hours = TimeLeft/10 + 1;
        if (!firstCheck && (hours != prevHours || hours == 1)) {
            chime.Play();
            StopCoroutine("EmbiggenBox");
            StartCoroutine("EmbiggenBox");
        }
        prevHours = hours;
        firstCheck = false;
        TimeText.text = hours + " ";
        if(hours == 1){
            TimeText.text += "hour left";
        }
        else{
            TimeText.text += "hours left";
        }
    }
    public void Awake(){
        if(Started == false){
            TimeLeft = TimeLimit;
           // StartTimer();
        }
        chime = GetComponent<AudioSource>();
    }
    IEnumerator RunTimer(){
        while(TimeLeft>0){
            if(!GameManager.Instance.GetPlayerBusy()){
                TimeLeft -= 2;
                gameObject.transform.parent.GetComponent<Image>().color = Color.white;
                UpdateTime();    
            }
            else{
                gameObject.transform.parent.GetComponent<Image>().color = new Color32(255, 199, 199, 255);
            }
            
            yield return new WaitForSeconds(0.8f); 
            //Debug.Log("less");   
        }
        UpdateTime();
        yield return new WaitForSeconds(0.4f); 
        GameManager.Instance.BackToOffice();
        Started = false;
        TimeLeft = TimeLimit;
    }

    IEnumerator EmbiggenBox() {

        float scale = 1.5f;
        while (scale > 1) {
            TimeBox.transform.localScale = scale * Vector3.one;
            scale *= 0.99f;
            yield return new WaitForSeconds(0.02f);
        }
        TimeBox.transform.localScale = Vector3.one;

    }

   
    // Update is called once per frame
    void Update()
    {
        /*
        if(gameObject.activeInHierarchy){
            if(GameManager.Instance.GetPlayerBusy()){
            gameObject.transform.parent.GetComponent<Image>().color = new Color32(255, 199, 199, 255);
            }
            else{
            gameObject.transform.parent.GetComponent<Image>().color = Color.white;
            }
        }*/
        
    }
}
