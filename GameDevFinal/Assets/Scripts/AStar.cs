using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class AStar : MonoBehaviour
{
    private bool[,] grid;
    public Tilemap tilemap;
    private Dictionary<Vector3, Vector3> parentmap;
    private Rigidbody2D rb;
    private Stack<Vector3> path;
    private int t;
    private int b;
    private int l;
    private int r;
    private Vector3 mouseWorldPos;
    // private bool newDestination;
    private Animator animator;
    public static bool left;
    public static bool right;

    
    void Start()
    {
        path = new Stack<Vector3>();    
        animator = GetComponent<Animator>();
        // Get the TilemapCollider2D component
        TilemapCollider2D tilemapCollider = tilemap.GetComponent<TilemapCollider2D>();  

        // Calculate grid boundaries based on the TilemapCollider2D
        Vector2 tilemapCenter = tilemapCollider.bounds.center;
        Vector2 tilemapExtents = tilemapCollider.bounds.extents;    

        l = (int)Mathf.Round(tilemapCenter.x - tilemapExtents.x);
        t = (int)Mathf.Round(tilemapCenter.y + tilemapExtents.y);
        r = (int)Mathf.Round(tilemapCenter.x + tilemapExtents.x);
        b = (int)Mathf.Round(tilemapCenter.y - tilemapExtents.y);   

        // Initialize the grid
        grid = new bool[4 * (t - b) + 1, 4 * (r - l) + 1];
        for (int x = 0; x < 4 * (r - l) + 1; x++) {
            for (int y = 0; y < 4 * (t - b) + 1; y++) {
                Vector3 cellPosition = new Vector3(((float) 1 + x + 4 * l) / 4f,((float) 1 + y + 4 * b) / 4f);
                grid[y, x] = !IsCellOccupied(cellPosition);
            //    print($"cell positioncellPosition: {cellPosition}, cell center: {cellCenter}");
            //    if (grid[y,x]) { print("hit! " + (x + l) + ", " + (y + b)); }
            }
        }   

        rb = GetComponent<Rigidbody2D>();
    }

    private bool IsCellOccupied(Vector3 cellCenter)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(cellCenter);
        (int, int)[] playersize = {(0,0), (0,-1), (-1,0), (-1,-1), (-1,1), (0,1), 
        // (1,1), (1,0), (1,-1)
        };
        foreach ((int,int) ps in playersize) {
            if (tilemap.HasTile(new Vector3Int(cellPosition.x + ps.Item1, cellPosition.y + ps.Item2, 0))) {
                return true;
            }
            // Collider2D[] near = Physics2D.OverlapCircleAll(cellCenter, 1f);
            // foreach (Collider2D n in near) {
            //     if (n.CompareTag("NPC")) { 
            //         return true;  
            //     }
            // }
        } return false;
    }


    public Stack<Vector3> Path(Vector3 end) {

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
                if (!(closed.Contains(loc)) && grid[loc.Item2 - (4*b), loc.Item1 - (4*l)]) {
                    // value = dist from start + dist from end
                    float g = path_length;
                    float h = DistanceEstimate(endc,   loc);
                    Node newNode = new Node(loc, h); 
                    open.Add(newNode);
                    parentmap[vec(newNode.position)] = vec(current.position);
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

    public (int, int)[] neighbors(Node n) {
        int x = n.position.Item1;
        int y = n.position.Item2;
        (int, int)[] ns = { (x + 1, y), (x - 1, y), (x, y + 1), (x, y - 1) };
        return ns;
    }

    public float DistanceEstimate((int, int) v1, (int, int) v2) {
        float dx = Mathf.Abs(v1.Item1 - v2.Item1);
        float dy = Mathf.Abs(v1.Item2 - v2.Item2);
        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (horizontal != 0 | vertical != 0 || GameManager.Instance.GetPlayerBusy()) {
            path = new Stack<Vector3>();
        }
        if (Input.GetMouseButtonDown(0)) {
            // print("oy");
            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            // rb.velocity = new Vector2(mouseWorldPos.x - transform.position.x, mouseWorldPos.y - transform.position.y);
            Vector3 v3 = new Vector3(Mathf.Round(mouseWorldPos.x), Mathf.Round(mouseWorldPos.y), 0);
            List<Collider2D> cols = Physics2D.OverlapCircleAll(mouseWorldPos, 0.25f).ToList();
            var envcols = cols.Where(col => !col.CompareTag("NPC")).ToList();
            if (!GameManager.Instance.GetPlayerBusy() && !(envcols.Count > 0)) {
                path = Path(mouseWorldPos);
                wait();
                // string ot = "";
                // foreach (Vector3 p in path) { 
                //     ot +=  $"({p.x}, {p.y}), ";
                // }
                // print(ot);
            }
        }
        if (path.Count > 0 && !GameManager.Instance.GetPlayerBusy()) {
            Vector3 targetDirection = (path.Peek() - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, path.Peek(), 2 * Time.deltaTime);
            rb.velocity = new Vector2(targetDirection.x, targetDirection.y);
            if (path.Count == 0 || Vector3.Distance(transform.position, path.Peek()) < 0.05) {
                path.Pop();                
                // newDestination = true;
            } 
            if      ( Mathf.Abs(targetDirection.x) <  targetDirection.y) { AnimatorUpdate(0); left = false; right = false; }                  
            else if (-Mathf.Abs(targetDirection.x) >  targetDirection.y) { AnimatorUpdate(1); left = false; right = false; }
            else if ( Mathf.Abs(targetDirection.y) <  targetDirection.x) { AnimatorUpdate(2); left = false;  right = true; }
            else if (-Mathf.Abs(targetDirection.y) >  targetDirection.x) { AnimatorUpdate(2); left = true; right = false; }
            else                                                         { AnimatorUpdate(3); left = false; right = false; }

        }
    }   

    public static bool GetLeft()  { return left;  }
    public static bool GetRight() { return right; }

    void AnimatorUpdate(int index) {
        int c = 0;
        string[] animations = {"Up", "Down", "Walking", "Idle"};
        foreach (string s in animations) {
            if (c == index) { animator.SetTrigger(s);   }
            else            { animator.ResetTrigger(s); }
            c++;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "NPC") {
            path = new Stack<Vector3>();
            // newDestination = true;
            rb.velocity = new Vector2(0f, 0f);
        }
    }

    Vector3 vec((int, int) coord) {
        Vector3 v = new Vector3(((float) coord.Item1)/4f,((float) coord.Item2)/4f, 0f);
        return v;
    }
    IEnumerator wait()
    { yield return new WaitForSeconds(1f); }

    public class NodeComparer : IComparer<Node>
    {
        public int Compare(Node m, Node n) {
            return m.value.CompareTo(n.value);
        }
    }

    public class Node
    {
        public (int, int) position;
        public float value;

        public Node((int, int) ps, float v)
        {
            this.position = ps;
            this.value = v;
        }

    }

}
