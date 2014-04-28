using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

	public GameObject[] neighbor_nodes;
	public Vector3 pos;
    public float dist;
    public Node previous;

	// Use this for initialization
	void Start () {
		pos = transform.position;

        //check for missing arcs
        missingArcs();
	}
	
	// Update is called once per frame
	void Update () {
	    foreach (GameObject g in neighbor_nodes)
        {
            Debug.DrawLine(transform.position, g.transform.position, Color.red);
        }
	}

    void missingArcs(){
        foreach (GameObject g in neighbor_nodes)
        {
            Node n = g.GetComponent<Node>();
            GameObject[] tmp = new GameObject[n.neighbor_nodes.Length+1];
            bool found = false;

            for (int i = 0; i < n.neighbor_nodes.Length; i++){
                if (n.neighbor_nodes[i] == gameObject)
                    found = true;
                tmp[i] = n.neighbor_nodes[i];
            }

            //not found link, adding one
            if (!found){
                tmp[tmp.Length-1] = gameObject;
                n.neighbor_nodes = tmp;
            }
        }
    }
}
