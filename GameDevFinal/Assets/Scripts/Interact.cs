using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour {

    public string text;
    private bool talking;
    private Vector3 mouseWorldPos;


    public void OnTriggerEnter2D(Collider2D collider2D) {
        print("Entered..");
        if (collider2D.gameObject.CompareTag("Player") & !GameManager.Instance.GetPlayerBusy() & !talking) {
            talking = true;
            GameManager.Instance.DialogShow(text);
            StartCoroutine(Dialog());
            if(gameObject.GetComponent<NPCWander>()!= null){
                gameObject.GetComponent<NPCWander>().FaceFront();   
            }
        }
    }

    void OnMouseUp()
    {
        if (!GameManager.Instance.GetPlayerBusy() & !talking) {
            GameManager.Instance.DialogShow(text);
            talking = true;
            StartCoroutine(Dialog());
            if(gameObject.GetComponent<NPCWander>() != null){
                gameObject.GetComponent<NPCWander>().FaceFront();
            }            
        }
    }


    public void OnTriggerExit2D(Collider2D collider2D) {
        if (collider2D.gameObject.CompareTag("Player")) {
            GameManager.Instance.DialogHide();
            StopCoroutine(Dialog());
        }
    }

    private IEnumerator Dialog(){
        while(!Input.GetKeyDown(KeyCode.E)){
            yield return null;        
        } 
        GameManager.Instance.DialogHide();
        talking = false;
        print("end");
        yield return null;
    }

    void Start() {
        talking = false;
    }

    void Update() {
        // if (Input.GetMouseButtonDown(0)) {
        //     // print("oy");
        //     mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     mouseWorldPos.z = 0f;
        //     var cols = Physics2D.OverlapCircleAll(mouseWorldPos, 0.25f);
        //     foreach (Collider2D col in cols) {
        //         if (col.gameObject == gameObject) { Dialog(); }
        //     }
        // }

    }


}
