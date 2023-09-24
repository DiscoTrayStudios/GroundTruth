using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArticleManager : MonoBehaviour
{
    public static Dictionary<string, string> evidence_sentences = new Dictionary<string, string>();
    public static List<TestEvidence> tespreordered  = new List<TestEvidence>{};
    public static List<TestEvidence> tespostordered = new List<TestEvidence>{};
    public static HashSet<string> sentences = new HashSet<string>(); 
    
    public static string article = "";
    // Start is called before the first frame update
    void Start()
    {
      }

    // Update is called once per frame
    void Update()
    {
    }
    
    public static string getArticle() { return article; }

    public static void updateOrderedEvidenceSet(TestEvidence te) {
        if (GameManager.post) { tespostordered.Add(te); }
        else                  {  tespreordered.Add(te); }
    } 

    public static string updateArticle(string evix, bool remove) {
        int num = evix[^1] - '0';
        string sentence = ""; 
        if (GameManager.post) { sentence = tespostordered[num-1].test_evidence; }
        else                   { sentence =  tespreordered[num-1].test_evidence; }
        if (remove) { 
            sentences.Remove(sentence);
            article = "";
            foreach (string s in sentences) {
                article += s;
            } return article;
        }
        else {
            sentences.Add(sentence); 
            article += sentence;
            return article;
        
        }
    }
}
