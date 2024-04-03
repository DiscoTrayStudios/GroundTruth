using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndButton : MonoBehaviour
{

    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(FunctionOnClick); 
    }

    void Awake(){
        
    }

    public void SetText(){
        if(GameManager.Instance.IsPost()){
            text.SetText("Back to Main Menu");
        }
        else{
            text.SetText("Continue");
        }
    }

    void FunctionOnClick(){
        if(GameManager.Instance.IsPost()){
            GameManager.Instance.pre();
            GameManager.Instance.ChangeScene("TitleScreen");
        }
        else{
            GameManager.Instance.ChangeScene("Cutscene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
