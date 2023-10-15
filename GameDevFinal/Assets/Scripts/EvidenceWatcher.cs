using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceWatcher : MonoBehaviour
{

    public static EvidenceWatcher Instance {get; private set;}
    public GameObject[] evidence;
    private List<TestEvidencePiece> Pieces = new List<TestEvidencePiece>();

    public TestEvidencePiece evidencePieceOne;
    public TestEvidencePiece evidencePieceTwo;
    public TestEvidencePiece evidencePieceThree;
    public TestEvidencePiece evidencePieceFour;
    public TestEvidencePiece evidencePieceFive;
    public TestEvidencePiece evidencePieceSix;
    public TestEvidencePiece evidencePieceSeven;
    public TestEvidencePiece evidencePieceEight;

    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        Pieces.Add(evidencePieceOne);
        Pieces.Add(evidencePieceTwo);
        Pieces.Add(evidencePieceThree);
        Pieces.Add(evidencePieceFour);
        Pieces.Add(evidencePieceFive);
        Pieces.Add(evidencePieceSix);
        Pieces.Add(evidencePieceSeven);
        Pieces.Add(evidencePieceEight);

        TestShowEvidence();
    }

    // Update is called once per frame
    void Update()
    {
        //ShowEvidence();
        TestShowEvidence();
    }

    public void ShowEvidence() {
        
        foreach (var evi in evidence) {
            if (GameManager.evidence.ContainsKey(evi.name)) { // Learned from https://code-maze.com/csharp-detect-dictionary-key-exists/
                evi.SetActive(true);
            } else {
                evi.SetActive(false);
            }
        }
    }
    public void ResetEvidence(){
        foreach (var evi in evidence) {
            evi.SetActive(false);
            evi.GetComponentInChildren<Highlight>().SetUnclicked();
        }
    }
    public void TestShowEvidence(){
        //print("testShowEvidence" + GameManager.TestEvidenceList);
        index = 0;
        foreach (var item in GameManager.TestEvidenceList)
        {   
            Pieces[index].eviSummary.text = item.test_evidence_summary;
            Pieces[index].eviFullText.text = item.dialogue;
            Pieces[index].eviPaper.SetActive(true);
            index +=1;
        }
        
    }
}
