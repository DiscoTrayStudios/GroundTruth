using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonText : MonoBehaviour
{
    public TextMeshProUGUI boolean;
    private bool clicked = false;
    // Start is called before the first frame update
    void Start()
    {
        boolean.text = "false";
    }

    public void toggle() {
        clicked = !clicked;
        string s = "";
        if (clicked) { s = "true"; } else { s = "false"; }
        boolean.text = s;
    }

    public void Reset(){
        clicked = false;
        boolean.text = "false";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
