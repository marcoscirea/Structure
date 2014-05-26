using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

	public GameObject[] neighbor_nodes;
	private GameObject[] original_nodes;
	public Vector3 pos;
    public float dist;
    public Node previous;

	// Use this for initialization
	void Start () {
		pos = transform.position;


        //check for missing arcs
        missingArcs();
		
		//set original neighbors;
		original_nodes = neighbor_nodes;

	}
	
	// Update is called once per frame
	void Update () {
	    foreach (GameObject g in neighbor_nodes)
        {
			if(g!=null)
            	Debug.DrawLine(transform.position, g.transform.position, Color.red);
        }
	}

    public void missingArcs(){
		//cleanup broken arcs
		/*ArrayList temp = new ArrayList ();
		foreach (GameObject g in neighbor_nodes) {
			if (g!=null)
				temp.Add(g);
				}
		neighbor_nodes = new GameObject[temp.Count];
		for (int i = 0; i< neighbor_nodes.Length; i++) {
			neighbor_nodes[i] = (GameObject) temp[i];
				}*/

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

	public void restore(){
		neighbor_nodes = new GameObject[original_nodes.Length];
		neighbor_nodes = (GameObject[])original_nodes.Clone ();
	}

	public void cleanArcs(){
		foreach (GameObject n in neighbor_nodes) {
						ArrayList temp = new ArrayList ();
						Node node = n.GetComponent<Node>();
						foreach (GameObject g in node.neighbor_nodes) {
								if (g != gameObject)
										temp.Add (g);
						}
						GameObject[] new_neighbor_nodes = new GameObject[temp.Count];
						for (int i = 0; i< new_neighbor_nodes.Length; i++) {
								new_neighbor_nodes [i] = (GameObject)temp [i];
						}
			node.neighbor_nodes = new_neighbor_nodes;
				}
	}
}
