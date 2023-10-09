using System;
using System.Security.Cryptography;
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
    private static HashSet<string> truesums = new HashSet<string>{"Building Arch", "Prophetstown", "Tenskwatawa predicted earthquakes", 
                                                                  "Ground continues to shake", "Potential war in 1812", "Louisiana Purchase", 
                                                                  "Joining the United States", "Steamboat on the Mississippi", "Houses got burned" };
  private static HashSet<string> falsesums = new HashSet<string>{"Monster Earthquakes", "Prophetstown", "Creature in the woods", 
                                                                  "River People", "Man claims he saves family", "Traveling Up River", 
                                                                  "Volcanic Eruption", "Flying" };
    
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


  //  public static string getFeedback {
  //      Random rnd = new Random();
  //      string closing;
  // 
  //      string lenFeedback = switch(sentences.Count)
  //      {
  //          case 0:      "Where's the article?! Were you goofing off again? You're lucky standards are low around here.";
  //          case 1 || 2: "Your article was a little too short. I, and the public, expect more from you.";
  //          case 4 || 5: "This article was just the right size.";
  //          case 6 || 7: "Your article was a little overlong. You should be a bit more discerning.";
  //          case 8:      "Good grief! Is there anything you WON'T try to publish?";
  //      }
  //      if (GameManager.post) {
//
  //      }
  //      switch(rnd.Next(1,4))
  //      {
  //          case "Building Arch": "We. -Your Editor";
  //          case "Prophetstown": "Your article was widely disliked; our readers have begun protesting outside your office. Better luck on the next one! -Your Editor";
  //          case "Tenskwatawa predicted earthquakes": "Your article was mediocre. I don't have much else to say about it. -Your Editor";
  //          case "Houses got burned": "Your article was accurate and interesting. You might be on track for a promotion. -Your Editor";
  //          case "Ground continues to shake": "I don't have any complaints, this is perfect. You're going to go far, kid. -Your Editor";
  //      }
  //      switch(rnd.Next(1,4))
  //      {
  //          case "Joining the United States": "We. -Your Editor";
  //          case "Steamboat on the Mississippi": "Your article was widely disliked; our readers have begun protesting outside your office. Better luck on the next one! -Your Editor";
  //          case "Potential war in 1812": "Your article was mediocre. I don't have much else to say about it. -Your Editor";
  //          case "Louisiana Purchase": "Your article was accurate and interesting. You might be on track for a promotion. -Your Editor";
  //          case "": "I don't have any complaints, this is perfect. You're going to go far, kid. -Your Editor";
  //      }
  //      switch(rnd.Next(1,4))
  //      {
  //          case "River People": "We. -Your Editor";
  //          case "Flying": "Your article was mediocre. I don't have much else to say about it. -Your Editor";
  //          case "Traveling Up River": "Your article was accurate and interesting. You might be on track for a promotion. -Your Editor";
  //          case "Creature in the woods": "I don't have any complaints, this is perfect. You're going to go far, kid. -Your Editor";
  //      }
  //      switch(rnd.Next(1,4))
  //      {
  //          case "Prophetstown": "Your article was widely disliked; our readers have begun protesting outside your office. Better luck on the next one! -Your Editor";
  //          case "Man claims he saves family": "Your article was widely disliked; our readers have begun protesting outside your office. Better luck on the next one! -Your Editor";
  //          case "Volcanic Eruption": "Your article was mediocre. I don't have much else to say about it. -Your Editor";
  //          case "Monster Earthquakes": "Your article was accurate and interesting. You might be on track for a promotion. -Your Editor";
  //          case "": "I don't have any complaints, this is perfect. You're going to go far, kid. -Your Editor";
  //      }
  //      switch(rnd.Next(1,5))
  //      {
  //          case 1: "Your article was widely disliked; our readers have begun protesting outside your office. Better luck on the next one! -Your Editor";
  //          case 2: "Your article was mediocre. I don't have much else to say about it. -Your Editor";
  //          case 3: "Your article was accurate and interesting. You might be on track for a promotion. -Your Editor";
  //          case 4: "I don't have any complaints, this is perfect. You're going to go far, kid. -Your Editor";
  //      } return "";
  //  }
    
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
        print(evix);
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
