using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LeaveConfirmation : MonoBehaviour
{

    // Start is called before the first frame update
    public void OpenConfirmBox(){
        if(!GameManager.Instance.GetPlayerBusy()){
            gameObject.SetActive(true);  
            GameManager.Instance.SetPlayerBusy(true); 
        }
        
    }
    public void LeaveConfirmed(){
        GameManager.Instance.SetPlayerBusy(false);
        gameObject.SetActive(false);
        GameManager.Instance.ChangeScene("InvestigativeArea");
    }



    

    // Update is called once per frame
    void Update()
    {
        
    }
}
