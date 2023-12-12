using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TravelButtonScript : MonoBehaviour
{

    public string travelto;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(FunctionOnClick); 
    }

    void FunctionOnClick(){
        string scenename = travelto;
        if(GameManager.Instance.IsPost()){
            scenename += "PostQuake";
            //GameManager.Instance.ChangeScene(travelto + "PostQuake");
        }
        else{
            scenename += "PreQuake";
        }
        GameManager.Instance.ChangeScene(scenename);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
