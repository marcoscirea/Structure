using UnityEngine;
using System.Collections;

[AddComponentMenu ("Inventory/Inventory Display")]
[RequireComponent(typeof(Inventory))]

public class InventoryDisplay : MonoBehaviour
{
//Displaying the Inventory.

//Variables for dragging:
		public Item itemBeingDragged; //This refers to the 'Item' script when dragging.
		private Vector2 draggedItemPosition; //Where on the screen we are dragging our Item.
		private Vector2 draggedItemSize;//The size of the item icon we are dragging.

//Variables for the window:
		Vector2 windowSize = new Vector2 (375, 162.5f); //The size of the Inventory window.
		bool useCustomPosition = false; //Do we want to use the customPosition variable to define where on the screen the Inventory window will appear?
		Vector2 customPosition = new Vector2 (70, 400); // The custom position of the Inventory window.
		Vector2 itemIconSize = new Vector2 (60.0f, 60.0f); //The size of the item icons.

//Variables for updating the inventory
		float updateListDelay = 9999;//This can be used to update the Inventory with a certain delay rather than updating it every time the OnGUI is called.
//This is only useful if you are expanding on the Inventory System cause by default Inventory has a system for only updating when needed (when an item is added or removed).
		private float lastUpdate = 0.0f; //Last time we updated the display.
		private Transform[] UpdatedList; //The updated inventory array.

//More variables for the window:
		static bool displayInventory = false; //If inv is opened.
		private Rect windowRect = new Rect (200, 200, 108, 130); //Keeping track of the Inventory window.
		public GUISkin invSkin; //This is where you can add a custom GUI skin or use the one included (InventorySkin) under the Resources folder.
		Vector2 Offset = new Vector2 (7, 12); //This will leave so many pixels between the edge of the window (x = horizontal and y = vertical).
		bool canBeDragged = true; //Can the Inventory window be dragged?

		public KeyCode onOffButton = KeyCode.I; //The button that turns the Inventory window on and off.

//Keeping track of components.
		private Inventory associatedInventory;
		private bool cSheetFound = false;
		//private Character cSheet;

        

//Store components and adjust the window position.
		void Awake ()
		{
				if (useCustomPosition == false) {
						windowRect = new Rect (Screen.width - windowSize.x - 70, Screen.height - windowSize.y - 70, windowSize.x, windowSize.y);
				} else {
						windowRect = new Rect (customPosition.x, customPosition.y, windowSize.x, windowSize.y);
				}
				associatedInventory = GetComponent<Inventory> ();//keepin track of the inventory script
				if (GetComponent<Character> () != null) {
						cSheetFound = true;
						//cSheet = GetComponent<Character> ();
				} else {
						//Debug.LogError ("No Character script was found on this object. Attaching one allows for functionality such as equipping items.");
						cSheetFound = false;
				}
		}

//Update the inv list
		public void UpdateInventoryList ()
		{
				UpdatedList = associatedInventory.Contents;
				//Debug.Log("Inventory Updated");
		}

		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.Escape)) { //Pressed escape
						ClearDraggedItem (); //Get rid of the dragged item.
				}
				if (Input.GetMouseButtonDown (1)) { //Pressed right mouse
						ClearDraggedItem (); //Get rid of the dragged item.
				}
	
				//Turn the Inventory on and off and handle audio + pausing the game.
				if (Input.GetKeyDown (onOffButton)) {
						if (displayInventory) {
								Close ();
						} else {
								Open ();
						}
				}
	
				//Making the dragged icon update its position
				if (itemBeingDragged != null) {
						//Give it a 15 pixel space from the mouse pointer to allow the Player to click stuff and not hit the button we are dragging.
						draggedItemPosition.y = Screen.height - Input.mousePosition.y + 15;
						draggedItemPosition.x = Input.mousePosition.x + 15;
				}
	
				//Updating the list by delay
				if (Time.time > lastUpdate) {
						lastUpdate = Time.time + updateListDelay;
						UpdateInventoryList ();
				}
		}

		public void Close ()
		{
				displayInventory = false;
		
				gameObject.SendMessage ("ChangedState", false, SendMessageOptions.DontRequireReceiver);
				gameObject.SendMessage ("PauseGame", false, SendMessageOptions.DontRequireReceiver); //StopPauseGame/EnableMouse/ShowMouse

		GameObject.FindGameObjectWithTag ("Player").GetComponent<PointClick> ().canMove = true;
	}

		public void Open ()
		{
				displayInventory = true;
		
				gameObject.SendMessage ("ChangedState", true, SendMessageOptions.DontRequireReceiver);
				gameObject.SendMessage ("PauseGame", true, SendMessageOptions.DontRequireReceiver); //PauseGame/DisableMouse/HideMouse

		GameObject.FindGameObjectWithTag ("Player").GetComponent<PointClick> ().canMove = false;
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PointClick>().selectedItem != null)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PointClick>().selectedItem.GetComponent<Item>().Select(false);
        }
	}

//Drawing the Inventory window
		void OnGUI ()
		{
				GUI.skin = invSkin; //Use the invSkin
				if (itemBeingDragged != null) { //If we are dragging an Item, draw the button on top:
						GUI.depth = 3;
						GUI.Button (new Rect (draggedItemPosition.x, draggedItemPosition.y, draggedItemSize.x, draggedItemSize.y), itemBeingDragged.itemIcon);
						GUI.depth = 0;
				}
	
				//If the inventory is opened up we create the Inventory window:
				if (displayInventory) {
						windowRect = GUI.Window (0, windowRect, DisplayInventoryWindow, "Inventory");
				}
		}

//Setting up the Inventory window
		void DisplayInventoryWindow (int windowID)
		{

				if (canBeDragged == true) {
						GUI.DragWindow (new Rect (0, 0, 10000, 30));  //the window to be able to be dragged
				}
	
				float currentX = 0 + Offset.x; //Where to put the first items.
				float currentY = 18 + Offset.y; //Im setting the start y position to 18 to give room for the title bar on the window.
	
				foreach (Transform i in UpdatedList) { //Start a loop for whats in our list.
						Item item = i.GetComponent<Item> ();
						if (cSheetFound) { //CSheet was found (recommended)
								if (GUI.Button (new Rect (currentX, currentY, itemIconSize.x, itemIconSize.y), item.itemIcon)) {
										bool dragitem = true; //Incase we stop dragging an item we dont want to redrag a new one.
										if (itemBeingDragged == item) { //We clicked the item, then clicked it again
												if (cSheetFound) {
														GetComponent<Character> ().UseItem (item, 0, true); //We use the item.
												}
												ClearDraggedItem (); //Stop dragging
												dragitem = false; //Dont redrag
										}
										if (Event.current.button == 0) { //Check to see if it was a left click
												if (dragitem) {
														if (item.isEquipment == true) { //If it's equipment
																itemBeingDragged = item; //Set the item being dragged.
																draggedItemSize = itemIconSize; //We set the dragged icon size to our item button size.
																//We set the position:
																draggedItemPosition.y = Screen.height - Input.mousePosition.y - 15;
																draggedItemPosition.x = Input.mousePosition.x + 15;
														} else {
																i.GetComponent<ItemEffect> ().UseEffect (); //It's not equipment so we just use the effect.
														}
												}
										} else if (Event.current.button == 1) { //If it was a right click we want to drop the item.
												associatedInventory.DropItem (item);
										}
								}
						} else { //No CSheet was found (not recommended)
								if (GUI.Button (new Rect (currentX, currentY, itemIconSize.x, itemIconSize.y), item.itemIcon)) {
										if (Event.current.button == 0 && item.isEquipment != true) { //Check to see if it was a left click.
												i.GetComponent<ItemEffect> ().UseEffect (); //Use the effect of the item.
										} else if (Event.current.button == 1) { //If it was a right click we want to drop the item.
												associatedInventory.DropItem (item);
										}
								}
						}
		
						if (item.stackable) { //If the item can be stacked:
								GUI.Label (new Rect (currentX, currentY, itemIconSize.x, itemIconSize.y), "" + item.stack, "Stacks"); //Showing the number (if stacked).
						}
		
						currentX += itemIconSize.x;
						if (currentX + itemIconSize.x + Offset.x > windowSize.x) { //Make new row
								currentX = Offset.x; //Move it back to its startpoint wich is 0 + offsetX.
								currentY += itemIconSize.y; //Move it down a row.
								if (currentY + itemIconSize.y + Offset.y > windowSize.y) { //If there are no more room for rows we exit the loop.
										return;
								}
						}
				}
		}

//If we are dragging an item, we will clear it.
		public void ClearDraggedItem ()
		{
				itemBeingDragged = null;
		}
}