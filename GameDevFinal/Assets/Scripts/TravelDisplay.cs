using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class TravelDisplay : MonoBehaviour
{
    public int daysTravel;

    public Image longhand;
    public Image shorthand;

    public TextMeshProUGUI TravelText;
    
    public void ShowTraveling(){
        gameObject.SetActive(true);
        StartCoroutine(ShortHandMove(daysTravel, 1));
    }
    public void ShowTraveling(String place){
        ChangeTravelText(place);
        gameObject.SetActive(true);
        StartCoroutine(ShortHandMove(daysTravel, 1));
    }

    public void EndTravel(){
        gameObject.SetActive(false);
    }

    //IEnumerator ShowTravel(int daysback){
    //int d = 0;
      //  while(d < daysback){
        //StartCoroutine(ShortHandMove());
        //yield return
        //}
    //}

    public void ChangeTravelText(String place){
        
        if (place != "investigative area"){
            TravelText.SetText("Traveling to " + place + "...");
        } else {
            TravelText.SetText("Traveling back to desk...");
        }
        
    }

    IEnumerator LongHandMove(){
        float longrx = longhand.rectTransform.eulerAngles.x;
        float longry = longhand.rectTransform.eulerAngles.y;
        float longrz = 360f;
        while(longrz > 0f){
            longrz = longrz - 24;            
            longhand.rectTransform.eulerAngles = new UnityEngine.Vector3 (longrx, longry, longrz);
            yield return null;
        }
         longhand.rectTransform.eulerAngles = new UnityEngine.Vector3 (longrx, longry, 360f);
        
        yield break;
    }
    IEnumerator ShortHandMove(int daysback, int d){
        float shortrx = shorthand.rectTransform.eulerAngles.x;
        float shortry = shorthand.rectTransform.eulerAngles.y;
        float shortrz = 360f;
        float rot = 30f;
        while(shortrz > 0){
                       
           
            if(shortrz % rot == 0){
                StartCoroutine(LongHandMove());
            }
            shortrz = shortrz - 2f; 
            shorthand.rectTransform.eulerAngles = new UnityEngine.Vector3 (shortrx, shortry, shortrz);
           
            yield return null;
        }
        if(d < daysback){
            
            StartCoroutine(ShortHandMove(daysback, d + 1));
        }
        else{
            yield return new WaitForSecondsRealtime(0.5f);
            EndTravel();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }
    void awake(){
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
