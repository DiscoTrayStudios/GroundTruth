using System.Threading.Tasks;
using System.Security.AccessControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArticleManager : MonoBehaviour
{
    public static Dictionary<string, string> evidence_sentences = new Dictionary<string, string>();
    public static List<TestEvidence> tespreordered  = new List<TestEvidence>{};
    public static List<TestEvidence> tespostordered = new List<TestEvidence>{};
    public static List<string> dialoguespreordered  = new List<string>{};
    public static List<string> dialoguespostordered = new List<string>{};
    public static HashSet<string> sentences = new HashSet<string>(); 
    private static HashSet<string> truesums = new HashSet<string>{"Prophetstown", "Tenskwatawa predicted earthquakes", "River flowing backwards.",
                                                                  "Ground continues to shake", "Potential war in 1812", "Louisiana Purchase", 
                                                                  "Joining the United States", "Steamboat on the Mississippi", "Houses got burned" };
    private static HashSet<string> falsesums = new HashSet<string>{"Monster Earthquakes", "Creature in the woods", 
                                                                   "River People", "Man claims he saves family", "Traveling Up River", 
                                                                   "Volcanic Eruption", "Flying", "I'm gonna get you!"};
    
    public static string article = "";
    public static int score = 0;

    private static int scorenum;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

public static string getFeedback()
{
    System.Random rnd = new System.Random();
    string feedback = "";    
    int lenArticle = sentences.Count;
    int s1 = 0;
    int s2 = 0;
    int score = 0;
 
    if (lenArticle > 1)
    {
        s1 = rnd.Next(0, lenArticle);
        s2 = (s1 + 1 + rnd.Next(0, lenArticle - 1)) % lenArticle;
    }

    switch (lenArticle)
    {
        case 0: feedback += "Where's the article?! Were you goofing off again? You're lucky standards are low around here. "; break;
        case 1: feedback += "Your article was a little too short. I, and the public, expect more from you. "; break;
        case 2: feedback += "Your article was a little too short. I, and the public, expect more from you. "; break;
        case 3: feedback += "Your article was a little too short. I, and the public, expect more from you. "; break;
        case 4: feedback += "This article was just the right size. "; break;
        case 5: feedback += "This article was just the right size. "; break;
        case 6: feedback += "Your article was a little overlong. You should be a bit more discerning. "; break;
        case 7: feedback += "Your article was a little overlong. You should be a bit more discerning. "; break;
        case 8: feedback += "Your article was a little overlong. You should be a bit more discerning. "; break;
        case 9: feedback += "Good grief! Is there anything you WON'T try to publish? "; break;
    }
    if (GameManager.post && lenArticle > 1) {
        switch(tespostordered[s1].test_evidence_summary)
        {
            case "I'm gonna get you!":                feedback += "We, at the editorial board, hope that no one will 'get' us, and are concerned about these threats of being got. "; break;
            case "Monster Earthquakes":               feedback += "A monster caused the earthquakes? That's very untrue. Publishing that would be disrespectful. "; break;
            case "Tenskwatawa predicted earthquakes": feedback += "Regardless of the undemocratic and tumultuous leadership in Prophetstown, we were surprised to hear that Tenskatawa correctly predicted the earthquake, and have noticed an increase in support for his cause because of it. "; break;
            case "Houses got burned":                 feedback += "Our readers were shocked and saddened by the news of newly homeless and rampant fires. Good work getting the scoop. "; break;
            case "Man claims he saves family":        feedback += "On the man who saved the family - was anyone else able to corroborate this? The family, for example. "; break;
            case "Ground continues to shake":         feedback += "Our readers were unaware of the lingering effects of earthquake aftershocks - thank you for letting us know! "; break;
            case "Volcanic Eruption":                 feedback += "Our fact-checkers couldn't find any record of a volcano erupting in the area. "; break;
            case "River flowing backwards.":          feedback += "Amazingly, our fact-checkers have found many accounts of the Mississippi River running backwards during the quakes - the power of nature never ceases to amaze me. "; break;
        }
        switch(tespostordered[s2].test_evidence_summary)
        {
            case "I'm gonna get you!":                feedback += "We, at the editorial board, do not believe that someone will 'get' us, and worry this is mere braggadocio."; break;
            case "Monster Earthquakes":               feedback += "A monster caused the earthquakes? That's very untrue. Publishing that would be disrespectful. "; break;
            case "Tenskwatawa predicted earthquakes": feedback += "Regardless of the undemocratic and tumultuous leadership in Prophetstown, we were surprised to hear that Tenskatawa correctly predicted the earthquake, and have noticed an increase in support for his cause because of it. "; break;
            case "Houses got burned":                 feedback += "Our readers were shocked and saddened by the news of newly homeless and rampant fires. Good work getting the scoop. "; break;
            case "Man claims he saves family":        feedback += "On the man who saved the family - was anyone else able to corroborate this? The family, for example. "; break;
            case "Ground continues to shake":         feedback += "Our readers were unaware of the lingering effects of earthquake aftershocks - thank you for letting us know! "; break;
            case "Volcanic Eruption":                 feedback += "Our fact-checkers couldn't find any record of a volcano erupting in the area. "; break;
            case "River flowing backwards.":          feedback += "Amazingly, our fact-checkers have found many accounts of the Mississippi River running backwards during the quakes - the power of nature never ceases to amaze me. "; break;
        }
    }

    else if (lenArticle > 1) {
        switch(tespreordered[s1].test_evidence_summary)
        {
            case "Traveling Up River":           feedback += "Oh, someone travelled up the river on a whim? Fat chance! Liars like to stand by the river and tell tall tales, that was day one at my journalism school. "; break;
            case "Flying":                       feedback += "Get your head out of the clouds! We don't even have telegrams yet, there are certainly no planes. "; break;
            case "River People":                 feedback += "I saw a river person once. No, no I didn't. That's not true. "; break;
            case "Prophetstown":                 feedback += "The thoughts on Prophetstown are even more relevant in the wake of the recent Battle of Tippecanoe, fought between Tecumseh's followers and the Indiana army. "; break;
            case "Creature in the woods":        feedback += "A creature in the woods? Is it spiky, or squishy? Just kidding, we do not care. Please report truth. "; break;
            case "Potential war in 1812":        feedback += "The war is in response to continued British aggression at our ports - Likely not a major concern for Missourians. "; break;
            case "Louisiana Purchase":           feedback += "I remember the Louisiana Purchase, but only thought of it politically. Now that you've written it out, I grant it was immoral for Napoleon to waltz in, as if he owned the place. "; break;
            case "Steamboat on the Mississippi": feedback += "A boat powered by steam? This is a great invention! "; break;
            case "Joining the United States":    feedback += "In the city, it can be difficult to gauge how people impacted feel about the policies we pass. Missouri's reluctance to join the union is a great example. "; break;
        }
        switch(tespreordered[s2].test_evidence_summary)
        {
            case "Traveling Up River":           feedback += "Oh, someone travelled up the river on a whim? Fat chance! Liars like to stand by the river and tell tall tales, that was day one at my journalism school. "; break;
            case "Flying":                       feedback += "Get your head out of the clouds! We don't even have telegrams yet, there are certainly no planes. "; break;
            case "River People":                 feedback += "I saw a river person once. No, no I didn't. That's not true. "; break;
            case "Prophetstown":                 feedback += "The thoughts on Prophetstown are even more relevant in the wake of the recent Battle of Tippecanoe, fought between Tecumseh's followers and the Indiana army. "; break;
            case "Creature in the woods":        feedback += "A creature in the woods? Is it spiky, or squishy? Regardless, we do not care. Please report truth. "; break;
            case "Potential war in 1812":        feedback += "The war is in response to continued British aggression at our ports - Likely not a major concern for Missourians. "; break;
            case "Louisiana Purchase":           feedback += "I remember the Louisiana Purchase, but only thought of it politically. Now that you've written it out, I grant it was immoral for Napoleon to waltz in, as if he owned the place. "; break;
            case "Steamboat on the Mississippi": feedback += "A boat powered by steam? This is a great invention! "; break;
            case "Joining the United States":    feedback += "In the city, it can be difficult to gauge how people impacted feel about the policies we pass. Missouri's reluctance to join the union is a great example. "; break;
        }
    }

    switch(score > 75, score > 0, score > -50, lenArticle != 0)
    {
        case (false, false, false, true): feedback += "Your article was widely disliked; our readers have begun protesting outside your office. Better luck on the next one! -Your Editor"; scorenum = 1; break;
        case (false, false,  true, true): feedback += "Your article was mediocre. I don't have much else to say about it. -Your Editor"; scorenum = 2; break;
        case (false,  true,  true, true): feedback += "Your article was accurate and interesting. You might be on track for a promotion. -Your Editor"; scorenum = 3; break;
        case ( true,  true,  true, true): feedback += "I don't have any complaints, this is perfect. You're going to go far, kid. -Your Editor"; scorenum = 4; break;
    }

    return feedback;
}

    public static int getScoreNum(){
        return scorenum;
    }
    
    public static string getArticle() { return article; }

    public static void updateOrderedEvidenceSet(TestEvidence te, bool whichDialogue) {
        print(te.test_evidence_name);
        print(te.test_evidence);
        print(te.test_evidence_summary);
        print(te.dialogue);
        print(te.dialogue1);
        print(whichDialogue);
        if (GameManager.post) { 
            tespostordered.Add(te); 
            if (whichDialogue) {
                dialoguespostordered.Add(te.dialogue1); 
            } else {
                dialoguespostordered.Add(te.dialogue); 
            }
        }
        else                  {  
            tespreordered.Add(te); 
            if (whichDialogue)  {
                dialoguespreordered.Add(te.dialogue1);  
            }
            else { 
                dialoguespreordered.Add(te.dialogue); 
            }
        }
    } 

    public static int getEvidenceIndex(TestEvidence te) {
        List<TestEvidence> telist = new List<TestEvidence>();
        if (GameManager.post) { telist = tespostordered; } else { telist = tespreordered; };
        int index = 0;
        foreach (TestEvidence tf in telist) {
                if (tf.test_evidence_summary == te.test_evidence_summary) {
                    return index;
                }
                index++;
        } return index;
    }

    public static string getDialogues(int index) {
        if (GameManager.post) { return dialoguespostordered[index]; }
        else                  {  return dialoguespreordered[index]; }
    }
    public static void bothDialogues(int index, TestEvidence te) {
        if (GameManager.post) { dialoguespostordered[index] = te.dialogue + "\n" + te.dialogue1; }
        else                  {  dialoguespreordered[index] =  te.dialogue  + "\n" + te.dialogue1; }
    }



    public static int getScore() { return score; }

    public static void resetArticleAndScore() { article = ""; score = 0; sentences = new HashSet<string>(); }

    public static string updateArticle(string evix, bool remove) {
        print(evix);
        int num = evix[^1] - '0'; //This line changes something like  'Evi8' :: str  to  8 :: int
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
