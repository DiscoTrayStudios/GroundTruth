using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class WalkAwayFromBoss : MonoBehaviour
{
    public AudioSource speaking;
    public AudioSource walking;
    public AudioSource door;
    private float moveSpeed = 1f;
    private Rigidbody2D rb;
    private Animator animator;
    private bool leaving = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //WalkAway();
    }

    public void WalkAway(){
        speaking.Stop();
        walking.Play();
        leaving = true;
        StartCoroutine(MoveToWaypoint());
    }

    private IEnumerator MoveToWaypoint(){
        animator.SetTrigger("Walking");
        Vector3 targetPosition = new Vector3(0f, -6.6f, 0f);
        Vector3 direction = (targetPosition - transform.position).normalized;
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            rb.velocity = direction * moveSpeed;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        walking.Stop();
        door.Play();
        while(door.isPlaying){
            yield return null;
        }
        if(!GameManager.Instance.IsPost()){
            GameManager.Instance.ChangeScene("InvestigativeArea");    
        }
        else{
            GameManager.Instance.ChangeScene("PostQuake");    
        }
        
        StopCoroutine(MoveToWaypoint());   
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.BossUI.activeSelf == false){
            if(!leaving){
                WalkAway();   
            }
            
        }
    }
}
