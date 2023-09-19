using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour {

    public string text;


    public void OnTriggerEnter2D(Collider2D collider2D) {
        print("Entered..");
        if (collider2D.gameObject.CompareTag("Player") & !GameManager.Instance.GetPlayerBusy()) {
            GameManager.Instance.DialogShow(text);
            StartCoroutine(Dialog());
        }
    }

    public void OnTriggerExit2D(Collider2D collider2D) {
        if (collider2D.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.DialogHide();
            StopAllCoroutines();
        }
    }

    private IEnumerator Dialog(){
       while(!Input.GetKeyDown(KeyCode.E)){
        yield return null;
        
       } 
       GameManager.Instance.DialogHide();
       print("end");
        StopAllCoroutines();
    }

    void Start() {

    }

    void Update() {
      
    }


}
