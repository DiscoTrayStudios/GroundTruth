using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// brought to you in part by https://discussions.unity.com/t/freeze-rigidbody-position-in-script/110627/2

public class NPCWander : MonoBehaviour {
    public Waypoint[] waypoints;
    public float moveSpeed = 2f;

    private int currentWaypointIndex = 0;
    private Coroutine moveToWaypointCoroutine;
    private Rigidbody2D rb;
    private bool someoneIsStill = false;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        moveToWaypointCoroutine = StartCoroutine(MoveToWaypoint());    
    }

    private IEnumerator MoveToWaypoint(){
        while (true){
            Waypoint currentWaypoint = waypoints[currentWaypointIndex];
            Vector3 targetPosition = currentWaypoint.transform.position;
            Vector3 direction = (targetPosition - transform.position).normalized;
            // Move towards the waypoint
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f) {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                rb.velocity = direction * moveSpeed;
                yield return null;
            }

            rb.velocity = Vector2.zero;

            // Wait at the waypoint if specified
            if (currentWaypoint.waitTime > 0) {
                yield return new WaitForSeconds(currentWaypoint.waitTime);
            }

            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 10f);
        if (colls.Length > 1) {
    		foreach (Collider2D col in colls) {
    			if (col.CompareTag("Player")) {
    				Vector2 desired = col.gameObject.transform.position - rb.gameObject.transform.position;

    				float actual = desired.magnitude;
    				if (col.CompareTag("Player")) {
    					actual *= 3;
    				}
    				rb.AddForce(desired.normalized * -1 - rb.velocity);
    			}
    		}
        }
    }


    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")){
            if (moveToWaypointCoroutine != null){
                StopCoroutine(moveToWaypointCoroutine);
                moveToWaypointCoroutine = null;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (moveToWaypointCoroutine == null){
                moveToWaypointCoroutine = StartCoroutine(MoveToWaypoint());
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        print(someoneIsStill);
        if (!someoneIsStill) {
            someoneIsStill = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            if (collision.gameObject.CompareTag("Player") && moveToWaypointCoroutine != null) {
                StopCoroutine(moveToWaypointCoroutine);
                moveToWaypointCoroutine = null;
            }
        }
        else {
            rb.constraints = RigidbodyConstraints2D.None;
            Vector2 desired = rb.gameObject.transform.position - collision.gameObject.transform.position;
            print(collision.gameObject.transform.position);
	       	rb.AddForce(desired.normalized * moveSpeed - rb.velocity);
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        rb.constraints = RigidbodyConstraints2D.None;
        if (collision.gameObject.CompareTag("Player") && moveToWaypointCoroutine == null && rb.gameObject.activeSelf){
            moveToWaypointCoroutine = StartCoroutine(MoveToWaypoint());
            someoneIsStill = false;
        }        
    }
}