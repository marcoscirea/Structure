using UnityEngine;
using System.Collections;

public class Depth : MonoBehaviour {

	Vector3 initScale;
	float initY;
	public float rate = 1;

	// Use this for initialization
	void Start () {
		initScale = transform.localScale;
		initY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		float diff = initY - transform.position.y;
		transform.localScale = initScale + new Vector3(diff,diff,diff) * rate;
	}
}
