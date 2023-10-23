using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI; 
using UnityEngine;
public class TestEvidencePiece : MonoBehaviour
{

    public GameObject eviPaper;
    public TextMeshProUGUI eviSummary;
    public GameObject eviFullPaper;
    public TextMeshProUGUI eviFullText;
    public TextMeshProUGUI dialogue;
    public TextMeshProUGUI dialogue1;
    private bool ddol;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!ddol) {
            DontDestroyOnLoad(eviPaper);
            ddol = true;
        }
    }
}
