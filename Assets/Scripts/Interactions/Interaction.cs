using UnityEngine;
using System.Collections;

public abstract class Interaction : MonoBehaviour {


	public Vector3 walkpoint;
	//Vector3 initScale;

	// Use this for initialization
	void Start () {
		walkpoint=transform.FindChild("Walk Point").transform.position;
		//initScale = transform.localScale;
		doStart ();
	}

	protected abstract void doStart();

	// Update is called once per frame
	public abstract void Update ();

	public abstract void action ();

	public abstract void secondary ();

	public Vector3 getWalkPoint(){
		return walkpoint;
	}
}
