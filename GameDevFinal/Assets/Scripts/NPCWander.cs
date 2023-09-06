using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWander : MonoBehaviour {
    public Waypoint[] waypoints;
    public float moveSpeed = 2f;

    private int currentWaypointIndex = 0;
    private Coroutine moveToWaypointCoroutine;
    private Rigidbody2D rb;

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
    void Update() {
        
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
    if (collision.gameObject.CompareTag("Player") && moveToWaypointCoroutine != null) {
        StopCoroutine(moveToWaypointCoroutine);
        moveToWaypointCoroutine = null;
    }
}

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")){
            if (moveToWaypointCoroutine == null) {
                moveToWaypointCoroutine = StartCoroutine(MoveToWaypoint());
            }
        }
    }

}