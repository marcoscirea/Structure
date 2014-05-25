using UnityEngine;
using System.Collections;

public class Item : Interaction {
public Texture2D itemIcon; //The Icon.
bool canGet = true; //If we can pick up the Item.
public string itemType; //This will let us equip the item to specific slots. Ex: Head, Shoulder, or whatever we set up. If the item is equipment (or weapon) this needs to match a slot to work properly.
public bool stackable = false; //Is it stackable? If yes then items with the same itemType will be stacked.
float maxStack = 20; //How many Items each stack can have before creating a new one. Remember that the Items that should be stacked should have the same itemType.
public float stack = 1; //This is how many stack counts this Item will take up.
public bool isEquipment = true; //Can the Item be equipped? This includes weapons.
public bool isAlsoWeapon = false; //Is the Item also a Weapon? This only works with isEquipment set to true.

//This is the object we will instantiate in the Players hand.
//We use this so we can have two versions of the weapon. One for picking up and one for using.
public Transform equippedWeaponVersion;

//These will store information about usefull components.
public EquipmentEffect equipmentEffect;
static Inventory playersinv;

private bool FPPickUpFound = false;

//customs
	public bool selected = false;

[AddComponentMenu ("Inventory/Items/Item")]

//Here we find the components we need.
void Awake (){
	playersinv = FindObjectOfType(typeof(Inventory)) as Inventory; //finding the players inv.
	if (playersinv == null)
	{
		canGet = false;
		Debug.LogWarning("No 'Inventory' found in game. The Item " + transform.name + " has been disabled for pickup (canGet = false).");
	}
	else
	{
		gameObject.SendMessage("RetrievePlayer", playersinv, SendMessageOptions.DontRequireReceiver);
	}
	
	if (isEquipment == false && GetComponent<ItemEffect>() == null)
	{
		Debug.LogError(gameObject.name + " is not equipment so please assign an ItemEffect script to it");
	}
	
	if (GetComponent<EquipmentEffect>())
	{
		equipmentEffect = GetComponent<EquipmentEffect>();
	}
	
	if (GetComponent<FirstPersonPickUp>() != null)
	{
		FPPickUpFound = true;
	}
	else if (transform.GetComponentInChildren<FirstPersonPickUp>() != null)
	{
		FPPickUpFound = true;
	}
}

//When you click an item
public override void action(){
	//If the 'FirstPersonPickUp' script is not attached we want to pick up the item.
	if (FPPickUpFound == false)
	{
		PickUpItem();
			GameObject.FindGameObjectWithTag ("Player").GetComponent<PointClick> ().activate ();
	}
}

//Picking up the Item.
public void PickUpItem (){
	bool getit = true;
	if(canGet){//if its getable or hasnt been gotten.
	Item locatedit = null;
	playersinv.gameObject.SendMessage ("PlayPickUpSound", SendMessageOptions.DontRequireReceiver); //Play sound
	
		if(stackable){
			
			foreach(Transform t in playersinv.Contents){
				if(t.name==this.transform.name){//if the item we wanna stack this on has the same name
					Item i=t.GetComponent<Item>();
					if(i.stack<i.maxStack){
						locatedit=i;
					}
				}
			}
			if(locatedit!=null){//if we have a stack to stack it to!
				getit=false;
				locatedit.stack+=1;
				Destroy(this.gameObject);
			}
			else{
				getit=true;
			}
		}
		//If we can get it and the inventory isn't full.
		if (getit && playersinv.Contents.Length < playersinv.MaxContent)
		{
			playersinv.AddItem(this.transform);
			MoveMeToThePlayer(playersinv.itemHolderObject);//moves the object, to the player
		}
		else if (playersinv.Contents.Length >= playersinv.MaxContent)
		{
			Debug.Log("Inventory is full");
		}
	}
}

//Moves the item to the Players 'itemHolderObject' and disables it. In most cases this will just be the Inventory object.
void MoveMeToThePlayer ( Transform itemHolderObject  ){
	canGet = false;
	gameObject.SetActive(false);
	transform.parent = itemHolderObject;
	transform.localPosition = Vector3.zero;
}

//Drops the Item from the Inventory.
public void DropMeFromThePlayer ( bool makeDuplicate  ){
		GameObject clone;
	 if (makeDuplicate == false) //We use this if the object is not stacked and so we can just drop it.
	{
		canGet = true;
		gameObject.SetActive(true);
		transform.parent = null;
		DelayPhysics();
	}
	else //If the object is stacked we need to make a clone of it and drop the clone instead.
	{
		canGet = true;
		clone = Instantiate(gameObject, transform.position, transform.rotation) as GameObject;
		canGet = false;
		clone.SetActive(true);
		clone.transform.parent = null;
		clone.name = gameObject.name;
	}
}

IEnumerator DelayPhysics (){
	if (playersinv.transform.parent.collider != null && collider != null)
	{
		Physics.IgnoreCollision(playersinv.transform.parent.collider, collider, true);
		yield return new WaitForSeconds (1);
		Physics.IgnoreCollision(playersinv.transform.parent.collider, collider, false);
	}
}

//Drawing an 'I' icon on top of the Item in the scene to keep organised.
void OnDrawGizmos (){
	Gizmos.DrawIcon (new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), "ItemGizmo.png", true);
}

	protected override void doStart()
	{
	}
	
	public override void Update()
	{
		if (selected) {
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.position = new Vector3(transform.position.x, transform.position.y, 0);
				}
	}

	public override void secondary()
	{
		Select (false);
	}

	public void Select(bool b){
		if (b) {
			selected = true;
			gameObject.collider.enabled = false;
			GameObject.FindGameObjectWithTag ("Player").GetComponent<PointClick> ().usingItem (gameObject);
			gameObject.SetActive (true);
				} else {
			selected = false;
			gameObject.collider.enabled = true;
			GameObject.FindGameObjectWithTag ("Player").GetComponent<PointClick> ().usingItem (null);
			gameObject.SetActive (false);	
				}
	}

	public void useWith (GameObject gameObject)
	{
		throw new System.NotImplementedException ();
	}
}