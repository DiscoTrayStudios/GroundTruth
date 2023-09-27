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
    private static HashSet<string> truesums = new HashSet<string>{"Building Arch", "Tenskwatawa predicted earthquakes", "Houses got burned", 
                                                                  "Ground continues to shake", "Potential war in 1812", "Louisiana Purchase", 
                                                                  "Joining the United States", "Steamboat on the Mississippi", };
    
    public static string article = "";
    public static int score = 0;
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
        print(te.test_evidence_name);
        print(te.test_evidence);
        print(te.test_evidence_summary);
        if (GameManager.post) { tespostordered.Add(te); }
        else                  {  tespreordered.Add(te); }
    } 

    public static int getScore() { return score; }

    public static void resetArticleAndScore() { article = ""; score = 0; sentences = new HashSet<string>(); }

    public static string updateArticle(string evix, bool remove) {
        int num = evix[^1] - '0';
        string sentence = "";
        string sum =      "";
        if (GameManager.post) {  
            TestEvidence evi = tespostordered[num-1]; 
            sentence = evi.test_evidence; 
            sum = evi.test_evidence_summary;
        }
        else                   { 
            TestEvidence evi =  tespreordered[num-1]; 
            sentence = evi.test_evidence; 
            sum = evi.test_evidence_summary;
        }

        if (remove) { 
            if (truesums.Contains(sum)) { score -= 25; }
            else                        { score += 25; }
            sentences.Remove(sentence);
            article = "";
            foreach (string s in sentences) {
                article += s;
            } return article;
        }
        else {
            if (truesums.Contains(sum)) { score += 25; }
            else                 { score -= 25; }            
            sentences.Add(sentence); 
            article += sentence;
            return article;
        
        }
    }
}
