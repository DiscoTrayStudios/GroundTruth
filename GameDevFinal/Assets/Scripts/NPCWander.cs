using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// brought to you in part by https://discussions.unity.com/t/freeze-rigidbody-position-in-script/110627/2

public class NPCWander : MonoBehaviour {
    public Waypoint[] waypoints;
    private Stack<Vector3> path;
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


    void Start() {
        path = new Stack<Vector3>();    
        rb = GetComponent<Rigidbody2D>();
        playerNear = true;
        //moveToWaypointCoroutine = StartCoroutine(MoveToWaypoint());    
    }

    void Update() {
        if (gameObject.activeSelf) {
            Collider2D[] near = Physics2D.OverlapCircleAll(rb.gameObject.transform.position, 2.5f);
            int l = near.Length;
            foreach (Collider2D n in near) {
                if (n.CompareTag("Player")) { playerNear = true;  }
                else                        { l--; }
            }
            if (l == 0) { playerNear = false; }
            if (moveToWaypointCoroutine == null && !playerNear && gameObject.activeInHierarchy) {
                moveToWaypointCoroutine = StartCoroutine(MoveToWaypoint());
            }
        }
    }

    private IEnumerator MoveToWaypoint(){
        while (!playerNear) {
            Waypoint currentWaypoint = waypoints[currentWaypointIndex];
            Vector3 targetPosition = currentWaypoint.transform.position;
            path = gameObject.GetComponent<AStar>().Path(targetPosition);
            Vector3 direction = (targetPosition - transform.position).normalized;
            // Move towards the waypoint
            while (path.Count > 0) {
                transform.position = Vector3.MoveTowards(transform.position, path.Peek(), moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, path.Peek()) < 0.1f) {
                    path.Pop();
                }
                if (colliding || (playerNear && GameManager.Instance.GetPlayerBusy())) {
                    currentWaypoint = waypoints[UnityEngine.Random.Range(0,waypoints.Length)];
                    targetPosition = currentWaypoint.transform.position;
                    path = gameObject.GetComponent<AStar>().Path(targetPosition);
                    direction = (targetPosition - transform.position).normalized;
                    colliding = false;
                }

                rb.velocity = direction * moveSpeed;
                //if(transform.position.x - targetPosition.x > transform.position.x - targetPosition.x)
                if (!front.Equals(null) & !back.Equals(null) & !playerNear) {
                    if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){                    
                        if(direction.x > 0) {
                            gameObject.GetComponent<SpriteRenderer>().sprite = right;
                        }
                        else { gameObject.GetComponent<SpriteRenderer>().sprite = left; }  
                    }
                    else if(Mathf.Abs(direction.y) > Mathf.Abs(direction.x)){
                        if (direction.y > 0) {
                            gameObject.GetComponent<SpriteRenderer>().sprite = back;
                        }
                        else {
                            gameObject.GetComponent<SpriteRenderer>().sprite = front;
                        }
                    }
                }
                colliding = false;
                yield return null;
            }
            if (!front.Equals(null)) {
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

    public void resetPath() { path = new Stack<Vector3>(); }

    public void FaceFront(){
        if(!front.Equals(null)){
            if (moveToWaypointCoroutine != null) {
                StopCoroutine(moveToWaypointCoroutine);
            }
            gameObject.GetComponent<SpriteRenderer>().sprite = front; 
        }
    }

    public void DontFaceFront(){
        if(!front.Equals(null)){
            if (moveToWaypointCoroutine == null && gameObject.activeInHierarchy) {
                moveToWaypointCoroutine = StartCoroutine(MoveToWaypoint());
            }
        }
    }

 


    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") || other.CompareTag("NPC")){
            if (moveToWaypointCoroutine != null){
                StopCoroutine(moveToWaypointCoroutine);
                moveToWaypointCoroutine = null;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (moveToWaypointCoroutine == null && gameObject.activeInHierarchy){
                moveToWaypointCoroutine = StartCoroutine(MoveToWaypoint());
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
       //   if (moveToWaypointCoroutine != null){
       //       StopCoroutine(moveToWaypointCoroutine);               
       //   }
       //   currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
       //   colPos = new Vector3(collision.GetContact(0).point.x, collision.GetContact(0).point.y, 0);
        colliding = true;
       // Vector3 colPos = new Vector3(collision.GetContact(0).point.x, collision.GetContact(0).point.y, 0);
       // Vector3 desired = transform.position - colPos;
       // while (Vector3.Distance(transform.position, desired.normalized * 5.0f) > 0.1f) {
       //     transform.position = Vector3.MoveTowards(transform.position, transform.position - (desired.normalized * 5.0f), moveSpeed * Time.deltaTime);
       //     rb.velocity = desired.normalized * moveSpeed;            
       // }        
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (moveToWaypointCoroutine == null && collision.gameObject.activeSelf && gameObject.activeInHierarchy){
                moveToWaypointCoroutine = StartCoroutine(MoveToWaypoint());
                colliding = false;
        }
    }
}