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
    private Dictionary<Vector3, Vector3> parentmap;
    private Stack<Vector3> path;
    private int t;
    private int b;
    private int l;
    private int r;
    private Vector3 mouseWorldPos;
    private bool newDestination;
    private bool done;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        moveToWaypointCoroutine = StartCoroutine(MoveToWaypoint());   
        path = new Stack<Vector3>();    
        done = true;
        TilemapCollider2D tilemapCollider = tilemap.GetComponent<TilemapCollider2D>();  
        Vector2 tilemapCenter = tilemapCollider.bounds.center;
        Vector2 tilemapExtents = tilemapCollider.bounds.extents;    

        l = (int)Mathf.Round(tilemapCenter.x - tilemapExtents.x);
        t = (int)Mathf.Round(tilemapCenter.y + tilemapExtents.y);
        r = (int)Mathf.Round(tilemapCenter.x + tilemapExtents.x);
        b = (int)Mathf.Round(tilemapCenter.y - tilemapExtents.y);   

        grid = new bool[4 * (t - b + 1), 4 * (r - l + 1)];
        for (int x = 0; x < 4 * (r - l) + 1; x++) {
            for (int y = 0; y < 4 * (t - b) + 1; y++) {
                Vector3 cellPosition = new Vector3(((float) 1 + x + 4 * l) / 4f,((float) 1 + y + 4 * b) / 4f);
                grid[y, x] = !IsCellOccupied(cellPosition);
            //    print($"cell positioncellPosition: {cellPosition}, cell center: {cellCenter}");
            //    if (grid[y,x]) { print("hit! " + (x + l) + ", " + (y + b)); }
            }
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
        if (moveToWaypointCoroutine == null && !playerNear){
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
                    if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
                    
                        if(direction.x > 0){
                            gameObject.GetComponent<SpriteRenderer>().sprite = right;
                        }
                        else{
                            gameObject.GetComponent<SpriteRenderer>().sprite = left;
                        }  
                    }
                    else if(Mathf.Abs(direction.y) > Mathf.Abs(direction.x)){
                        if(direction.y >0){
                            gameObject.GetComponent<SpriteRenderer>().sprite = back;
                        }
                        else{
                            gameObject.GetComponent<SpriteRenderer>().sprite = front;
                        }
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
            StopAllCoroutines();
            gameObject.GetComponent<SpriteRenderer>().sprite = front; 
        }
    }

    private bool IsCellOccupied(Vector3 cellCenter)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(cellCenter);
        (int, int)[] playersize = {(0,0), (0,-1), (1,0), (1,-1),(0,-2),(-1,-2) };
        foreach ((int,int) ps in playersize) {
            if (tilemap.HasTile(new Vector3Int(cellPosition.x + ps.Item1, cellPosition.y + ps.Item2, 0))) {
                return true;
            }
            Collider2D[] near = Physics2D.OverlapCircleAll(cellCenter, 0.125f);
            foreach (Collider2D n in near) {
                if (n.CompareTag("NPC")) { 
                    return true;  
                }
            }
        } return false;
    }

        public Stack<Vector3> AStar(Vector3 end) {

        parentmap = new Dictionary<Vector3, Vector3>();
        
        (int, int) startc = ((int) Mathf.Round(4*transform.position.x), (int) Mathf.Round(4*transform.position.y));
        (int, int)   endc = ((int) Mathf.Round(4*               end.x), (int) Mathf.Round(4*               end.y));

        Node s       = new Node(startc, DistanceEstimate(startc, endc));
        Node e       = new Node(endc,   DistanceEstimate(startc, endc));
        Node current = s;

        //open contains all nodes, sorted by heuristic value
        SortedSet<Node>       open = new SortedSet<Node>(new NodeComparer());
        open.Add(s);

        //closed contains all previously explored positions
        HashSet<(int, int)> closed = new   HashSet<(int, int)>();
        int path_length = 0;
        while (open.Count > 0) {
            current = open.Min;
            open.Remove(current);
            path_length++;
            foreach ((int, int) loc in neighbors(current)) {
                if (loc.Item2 - (4*b) < 4 * (t - b + 1) && loc.Item1 - (4*l) < 4 * (r - l + 1)) {
                    if (!(closed.Contains(loc)) && grid[loc.Item2 - (4*b), loc.Item1 - (4*l)]) {
                        // value = dist from start + dist from end
                        float g = path_length;
                        float h = DistanceEstimate(endc,   loc);
                        Node newNode = new Node(loc, h); 
                        open.Add(newNode);
                        parentmap[vec(newNode.position)] = vec(current.position);
                    }
                }
            }
            closed.Add(current.position);
        }
        Stack<Vector3> path = new Stack<Vector3>();
        Vector3 x = vec(endc);
        path.Push(vec(endc));
        while (parentmap.ContainsKey(x)) {
            x = parentmap[x];
            path.Push(x);
        }
        return path;
    }

public (int, int)[] neighbors(Node n)
{
    int x = n.position.Item1;
    int y = n.position.Item2;
    (int, int)[] ns = { (x + 1, y), (x - 1, y), (x, y + 1), (x, y - 1) };
    List<(int, int)> validNeighbors = new List<(int, int)>();
    foreach (var neighbor in ns) {
        if (IsValidNeighbor(neighbor)) {
            validNeighbors.Add(neighbor); } }
    return validNeighbors.ToArray();
}

    private bool IsValidNeighbor((int, int) n)
    {
        int nx = n.Item1;
        int ny = n.Item2;
        return (nx >= 4 * l && nx < 4 * (r + 1) && ny >= 4 * b && ny < 4 * (t + 1));
    }

    public float DistanceEstimate((int, int) v1, (int, int) v2) {
        float dx = Mathf.Abs(v1.Item1 - v2.Item1);
        float dy = Mathf.Abs(v1.Item2 - v2.Item2);
        return Mathf.Sqrt(dx * dx + dy * dy);
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
        if (moveToWaypointCoroutine == null){
                moveToWaypointCoroutine = StartCoroutine(MoveToWaypoint());
        }
    }

    Vector3 vec((int, int) coord) {
        return new Vector3(((float) coord.Item1)/4f,((float) coord.Item2)/4f, 0f);
    }
}