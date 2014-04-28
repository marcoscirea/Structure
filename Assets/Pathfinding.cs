using UnityEngine;
using System.Collections;

public class Pathfinding : MonoBehaviour
{
    float INF = 999999f;
    public GameObject[] nodesObj;
    public ArrayList path;
    ArrayList nodes;
    public GameObject s;
    public GameObject go;

    // Use this for initialization
    void Start()
    {
        nodesObj = GameObject.FindGameObjectsWithTag("Waypoint");
        nodes = populateList();

        /*Node tmp = Dijkstra(s.GetComponent<Node>(), go.GetComponent<Node>());
        Debug.Log(tmp.ToString());
        Path(GameObject.FindGameObjectWithTag("Player").transform.position, new Vector3(8f, -1f, -1));*/
    }
    
    // Update is called once per frame
    void Update()
    {
    
    }

    public ArrayList Path(Vector3 playerPosition, Vector3 destination){
        path = new ArrayList();
        //find the starting node and goal node
        Node start = null;
        float start_w = INF;
        Node goal = null;
        float goal_w = INF;
        foreach (GameObject g in nodesObj)
        {
            if (Vector3.Distance(g.transform.position, playerPosition) + Vector3.Distance(g.transform.position, destination) < start_w){
                start = g.GetComponent<Node>();
                start_w = Vector3.Distance(g.transform.position, playerPosition) + Vector3.Distance(g.transform.position, destination);
            }
            if (Vector3.Distance(g.transform.position, destination) < goal_w){
                goal = g.GetComponent<Node>();
                goal_w = Vector3.Distance(g.transform.position, destination);
            }
        }
        Debug.Log("start " + start.name + " goal " + goal.name);

        if (start == goal)
            return path;
        //Calculate best path
        if (Dijkstra(start, goal) != null)
        {
            //add the path to the ArrayList from the last to the first
            path = new ArrayList();
            path.Add(goal.transform.position);
            Node tmp = goal;
            while (tmp.previous != null){
                path.Add(tmp.previous.transform.position);
                tmp = tmp.previous;
            }
            //invert to be from start to finish
            path.Reverse();
            /*foreach( Node n in path){
                Debug.Log(n.name);
            }*/

            return path;
        }
        else
            return path; //case when start and goal are the same node
    }

    ArrayList populateList()
    {
        ArrayList l = new ArrayList();
        foreach (GameObject g in nodesObj)
        {
            l.Add(g.GetComponent<Node>());
        }
        return l;
    }

    Node smallest(ArrayList Q)
    {
        Node tmp = null;
        float min = INF;
        foreach (Node n in Q)
        {
            if (n.dist < min)
            {
                min = n.dist;
                tmp = n;
            }
        }
        return tmp;
    }

    float dist_between(Node u, Node v)
    {
        return Vector3.Distance(u.gameObject.transform.position, v.gameObject.transform.position);
    }

    Node Dijkstra(Node source, Node goal)
    {
        foreach (Node v in nodes)
        {                                // Initializations
            v.dist = INF;                                  // Unknown distance function from 
            v.previous = null;                             // Previous node in optimal path
        }                                                    // from source

        source.dist = 0;                                        // Distance from source to source
        ArrayList Q = populateList();                       // All nodes in the graph are
        // unoptimized – thus are in Q
        while (Q.Count!=0)
        {                                      // The main loop
            Node u = smallest(Q);    // Source node in first case
            if (u == goal)
                return u.previous;

            Q.Remove(u);
            if (u.dist == INF)
            {
                break;                                            // all remaining vertices are
            }                                               // inaccessible from source
                 
            foreach (GameObject vObj in u.neighbor_nodes)
            {  
                Node v = vObj.GetComponent<Node>();
                if (Q.Contains(v))
                {     // where v has not yet been 
                    // removed from Q.
                    float alt = u.dist + dist_between(u, v);
                    if (alt < v.dist)
                    {                                  // Relax (u,v,a)
                        v.dist = alt;
                        v.previous = u;
                        //decrease-key v in Q;                           // Reorder v in the Queue (that is, heapify-down) 
                    }
                }
            }
        }
        return null;
        //return dist[], previous[];
    }

}

