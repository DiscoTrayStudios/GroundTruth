using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvidence : MonoBehaviour
{
    // Start is called before the first frame update

    public string test_evidence_name;
    public string test_evidence;
    public string test_evidence_summary;
    public bool test_collected;
    void Start()
    {
        
    }

    public void setEvidenceName(string name){
        test_evidence_name = name;
    }
    public void setEvidence(string evidence){
        test_evidence = evidence;
    }

    public void setSummary(string summary){
        test_evidence_summary = summary;
    }
    public void setCollected(bool collected){
        test_collected = collected;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
