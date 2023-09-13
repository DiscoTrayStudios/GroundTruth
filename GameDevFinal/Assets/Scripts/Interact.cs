using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour {

    public string text;
    private bool interacting = false;

    public void OnTriggerEnter2D(Collider2D collider2D) {
        print("Entered..");
        if (collider2D.gameObject.CompareTag("Player") & !GameManager.Instance.GetPlayerBusy()) {
            GameManager.Instance.DialogShow(text);
            interacting = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider2D) {
        if (collider2D.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.DialogHide();
            
        }
    }

    void Start() {

    }

    void Update() {
        if(interacting & Input.GetKeyDown(KeyCode.E)){
            GameManager.Instance.DialogHide();
            interacting = false;
        }
    }


}
