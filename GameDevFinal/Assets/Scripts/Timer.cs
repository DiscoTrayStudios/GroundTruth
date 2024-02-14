using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Timer : MonoBehaviour
{   
    public TextMeshProUGUI TimeText;
    public int TimeLeft;

    private int hours;
    private int seconds;
    public int TimeLimit = 90;
    public bool Started;
    // Start is called before the first frame update
    void Start()
    {
       Started = false;
       TimeText = gameObject.GetComponent<TextMeshProUGUI>();
       TimeLeft = TimeLimit; 
    }
    public void StartTimer(){
        if(Started == false){
            UpdateTime();
            Debug.Log("start");
            StartCoroutine(RunTimer());
            Started = true;  
        }
        
    }
    public void UpdateTime(){
        /*minutes = TimeLeft/60;
        seconds = TimeLeft - (minutes*60);
        TimeText.text = minutes + ":";
        if(seconds < 10){
            TimeText.text += "0";
        }
        TimeText.text += seconds;*/
        hours = TimeLeft/10;
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
    }
    IEnumerator RunTimer(){
        while(TimeLeft>0){
            if(!GameManager.Instance.GetPlayerBusy()){
                TimeLeft--;
                UpdateTime();    
            }
            
            yield return new WaitForSeconds(0.9f); 
            Debug.Log("less");   
        }
        GameManager.Instance.BackToOffice();
        Started = false;
        TimeLeft = TimeLimit;
    }

   
    // Update is called once per frame
    void Update()
    {
        
    }
}
