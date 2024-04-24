using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonText : MonoBehaviour
{
    public TextMeshProUGUI boolean;
    public Slider slider;
    private bool clicked = false;
    public Image handle;
    public Image background;
    private RectTransform rt;
    public string evidence;
    public Image evi;
    private Color trueColor = new Color(0f, 0.85f, 0f, 1);
    private Color falseColor = new Color(0.85f, 0f, 0f, 1f);
    // Start is called before the first frame update
    void Start()
    {
        rt = boolean.GetComponent<RectTransform>();
        boolean.text = "false";
        evi = gameObject.transform.parent.gameObject.GetComponent<Image>();
    }

    public void toggle() {
        clicked = !clicked;
        string s = "";
        if (clicked) { 
            s = "true";
            boolean.text = s;
            rt.anchoredPosition = new Vector2(15, rt.anchoredPosition.y);
            slider.value = 0;
            background.color = trueColor;
            handle.color = trueColor;
            GameManager.AddUsedEvidence(evidence, 0);
            evi.color = Color.white;
        } else { 
            s = "false"; 
            boolean.text = s;
            rt.anchoredPosition = new Vector2(-15, rt.anchoredPosition.y);
            slider.value = 1;
            background.color = falseColor;
            handle.color = falseColor;
            GameManager.DeleteUsedEvidence(evidence);
            evi.color = new Color (0.7f, 0.7f, 0.7f, 0.7f);
        }
        boolean.text = s;
    }

    public void Reset(){
        if(clicked){
            toggle();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
