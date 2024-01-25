using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;


// brought to you in part by https://discussions.unity.com/t/freeze-rigidbody-position-in-script/110627/2

public class NPCWander : MonoBehaviour {
    public Waypoint[] waypoints;
    public float moveSpeed = 2f;
    public GameObject player;
    private int currentWaypointIndex = 0;
    private Coroutine moveToWaypointCoroutine;
    private Rigidbody2D rb;
    private bool colliding = false;
    private Vector3 colPos;
    private bool playerNear = false;
    public Sprite front;
    public Sprite back;
    public Sprite left;
    public Sprite right;
    private bool[,] grid;
    public Tilemap tilemap;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        moveToWaypointCoroutine = StartCoroutine(MoveToWaypoint());   
    }
    public void OnTriggerExit2D(Collider2D collider2D) {
        if (collider2D.gameObject.CompareTag("Player")) {
            playerNear = false;
        }
    }

    void Update() {
        Collider2D[] near = Physics2D.OverlapCircleAll(rb.gameObject.transform.position, 1.5f);
        int l = near.Length;
        foreach (Collider2D n in near) {
            if (n.CompareTag("Player")) { playerNear = true;  }
            else                        { l--; }
        }
        if (l == 0) { playerNear = false; }

        // body.AddForce(transform.up * speed);
		// transform.Rotate(Vector3.back * (Random.value * 2 * angle - angle));
		// if (Mathf.Abs(transform.position.y) >= 25) {
		// 	transform.position = new Vector2(Random.Range(-40f, 40f), -25);
		// }
        if (moveToWaypointCoroutine == null){
            moveToWaypointCoroutine = StartCoroutine(MoveToWaypoint());
        }        
    }

    private IEnumerator MoveToWaypoint(){
        while (!playerNear){
            Waypoint currentWaypoint = waypoints[currentWaypointIndex];
            Vector3 targetPosition = currentWaypoint.transform.position;
            Vector3 direction = (targetPosition - transform.position).normalized;
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f) {
                if (colliding) {
                    direction = (transform.position - colPos).normalized;
                    targetPosition = transform.position + direction.normalized;
                }
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                rb.velocity = direction * moveSpeed;
                //if(transform.position.x - targetPosition.x > transform.position.x - targetPosition.x)
                if(!front.Equals(null) & !back.Equals(null)){
                    if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {    
                        if(direction.x > 0) { gameObject.GetComponent<SpriteRenderer>().sprite = right; }
                        else                { gameObject.GetComponent<SpriteRenderer>().sprite = left;  }  
                    }
                    else if(Mathf.Abs(direction.y) > Mathf.Abs(direction.x)) {
                        if(direction.y >0) { gameObject.GetComponent<SpriteRenderer>().sprite = back;  }
                        else               { gameObject.GetComponent<SpriteRenderer>().sprite = front; }
                    }
                }
                colliding = false;
                yield return null;
            }
            if(!front.Equals(null)){
                gameObject.GetComponent<SpriteRenderer>().sprite = front; 
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
    public void FaceFront(){
        if(!front.Equals(null)){
            StopCoroutine(MoveToWaypoint());
            gameObject.GetComponent<SpriteRenderer>().sprite = front; 
        }
    }
}