using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour {

    public string text;
    private bool talking;
    private Vector3 mouseWorldPos;

    public bool GetTalking() { return talking; }

    public void OnTriggerEnter2D(Collider2D collider2D) {
        // print("Entered..");
        if (collider2D.gameObject.CompareTag("Player") & !GameManager.Instance.GetPlayerBusy() & !talking) {
            talking = true;
            GameManager.Instance.DialogShow(text);
            StartCoroutine(Dialog());
            if(gameObject.GetComponent<NPCWander>()!= null){
                gameObject.GetComponent<NPCWander>().FaceFront();   
            }
            Camera.main.GetComponent<ZoomCamera>().ZoomIn(transform.position);

        }
    }




    public void OnTriggerExit2D(Collider2D collider2D) {
        if (collider2D.gameObject.CompareTag("Player")) {
            GameManager.Instance.DialogHide();
            StopCoroutine(Dialog());
        }
    }

    private IEnumerator Dialog(){
        while(!Input.GetKeyDown(KeyCode.E) && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)){
            yield return null;        
        } 
        GameManager.Instance.DialogHide();
        talking = false;
        print("end");
        Camera.main.GetComponent<ZoomCamera>().UnZoom();
        yield return null;
    }

    void Start() {
        talking = false;
    }


}
