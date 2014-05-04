using UnityEngine;
using System.Collections;

public class NewInventory : MonoBehaviour {
	
	Vector2 hidden = new Vector2 (0.5f,0.95f);
	Vector2 shown = new Vector2 (0.5f, 0.9f);
	RaycastHit hit;
	public float speed = 0.01f;
	static ArrayList items = new ArrayList();
	bool noUpdate = false;
	//arraylist for already picked up items to avoid duplicate items (see Pickable)
	static ArrayList pickedUp = new ArrayList();

	bool mouseOn = false;
	
	// Use this for initialization
	void Start () {
		//transform.position = new Vector3(transform.position.x, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height - (Screen.height / 10), Camera.main.nearClipPlane)).y);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!mouseOn) {
			Debug.Log(transform.position + " " + hidden);
			if (Vector2.Distance(transform.position, hidden) > 0.01f){
				Debug.Log(Vector2.Distance(transform.position, hidden));
				transform.position += Vector3.up * Time.deltaTime * speed;
				foreach (GameObject item in items)
					item.transform.position += Vector3.up * Time.deltaTime * speed;
			}
		}
		
		//if (!noUpdate)
		//	updateItems ();
	}

	void OnMouseOver(){
		mouseOn = true;
		if (Vector2.Distance(transform.position, shown) > 0.1f){
			transform.localPosition += Vector3.down * Time.deltaTime * speed;
			foreach (GameObject item in items)
				item.transform.position += Vector3.down * Time.deltaTime * speed;
		}
	}

	void OnMouseExit(){
		mouseOn = false;
		}

	void showUnshow(){
		
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		if (Physics.Raycast(ray, out hit)) {
			
			if (hit.collider.tag=="Inventory" || items.Contains(hit.collider.gameObject)){
				if (Vector3.Distance(transform.localPosition, shown) > 0.1f){
					transform.localPosition += Vector3.down * Time.deltaTime * speed;
					foreach (GameObject item in items)
						item.transform.position += Vector3.down * Time.deltaTime * speed;
				}
				//transform.localPosition = shown;
			}
			else{
				if (Vector3.Distance(transform.localPosition, hidden) > 0.1f){
					transform.localPosition += Vector3.up * Time.deltaTime * speed;
					foreach (GameObject item in items)
						item.transform.position += Vector3.up * Time.deltaTime * speed;
				}
				//transform.localPosition = hidden;
			}
		}
	}
	
	public void addItem(GameObject item){
		items.Add(item);
		//item.transform.position= new Vector3( transform.position.x, transform.position.y, -0.5f);
		item.transform.localScale = item.GetComponent<Pickable>().invScale;
		//updateItems ();
		
		Object.DontDestroyOnLoad (item);
		pickedUp.Add (item.name);
	}
	
	public void removeItem(GameObject item){
		items.Remove (item);
		Destroy (item);
		//updateItems ();
	}
	
	public void updateItems(){
		float l = items.Count;
		if (l > 0) {
			for (int i = 0; i<l; i++) {
				GameObject tmp = items[i] as GameObject;
				//Debug.Log ((l-1)/2 + ";" + i);
				tmp.transform.position = transform.position + Vector3.left * ((l-1)/2) + Vector3.right * ( i  ) + Vector3.back * 0.5f;
			}
			transform.localScale = new Vector3( l + 0.5f, transform.localScale.y, transform.localScale.z);
		}
	}
	
	public void stopUpdate(bool yes){
		noUpdate = yes;
	}
	
	public bool hasBeenPickedUp(string name){
		return pickedUp.Contains (name);
	}
}
