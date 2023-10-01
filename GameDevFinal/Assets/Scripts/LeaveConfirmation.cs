using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LeaveConfirmation : MonoBehaviour
{
    private bool isPost;
    // Start is called before the first frame update
    public void OpenConfirmBox(bool post){
        if(!GameManager.Instance.GetPlayerBusy()){
            isPost = post;
            gameObject.SetActive(true);  
            GameManager.Instance.SetPlayerBusy(true); 
        }
        
    }
    public void LeaveConfirmed(){
        GameManager.Instance.SetPlayerBusy(false);
        gameObject.SetActive(false);
        if(!isPost){
            GameManager.Instance.ChangeScene("InvestigativeArea");
        }
        else{
            GameManager.Instance.ChangeScene("PostQuake");
        }
    }



    

    // Update is called once per frame
    void Update()
    {
        
    }
}
