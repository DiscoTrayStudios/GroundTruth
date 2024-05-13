using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalPopup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Notify(){
        //StartCoroutine("JournalNotify");
    }
    IEnumerator JournalNotify(){
        while(GameManager.Instance.GetPlayerBusy()){
           yield return null;
        }
        float time = 0;
        float duration = 2f;
        Color startValue = new Color (1,1,1,1);
        Color endValue = new Color(1, 1, 1, 0);
        Image image = gameObject.GetComponent<Image>();
        while (time < duration)
        {
            image.color = Color.Lerp(startValue, endValue, time / duration);
            time+= Time.deltaTime;
            yield return null;    
        } 
        
        StopCoroutine(JournalNotify());
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
