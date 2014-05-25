using UnityEngine;
using System.Collections;
[AddComponentMenu ("Inventory/Character Sheet")]
[RequireComponent(typeof (Inventory))]
public class Character : MonoBehaviour {
//The Character window (CSheet).

public Transform WeaponSlot; //This is where the Weapons are going to go (be parented too). In my case it's the "Melee" gameobject.

private Item[] ArmorSlot; //This is the built in Array that stores the Items equipped. You can change this to static if you want to access it from another script.
public string[] ArmorSlotName; //This determines how many slots the character has (Head, Legs, Weapon and so on) and the text on each slot.
public Rect[] buttonPositions; //This list will contain where all buttons, equipped or not will be and SHOULD HAVE THE SAME NUMBER OF cells as the ArmorSlot array.

Vector2 windowSize = new Vector2(375,300); //The size of the character window.
bool useCustomPosition = false; //Do we want to use the customPosition variable to define where on the screen the Character window will appear.
Vector2 customPosition = new Vector2 (70, 70); //The custom position of the Character window.
public GUISkin cSheetSkin; //This is where you can add a custom GUI skin or use the one included (CSheetSkin) under the Resources folder.
bool canBeDragged = true; //Can the Character window be dragged?

KeyCode onOffButton = KeyCode.I; //The key to toggle the Character window on and of.

bool DebugMode = false; //If this is enabled, debug.logs will print out information when something happens (equipping items etc.).

static bool csheet = false; //Helps with turning the CharacterSheet on and off.

private Rect windowRect= new Rect(100,100,200,300); //Keeping track of our character window.

//These are keeping track of components such as equipmentEffects and Audio.
private Inventory playersinv; //Refers to the Inventory script.
private bool equipmentEffectIs = false;
private InvAudio invAudio;
private bool invDispKeyIsSame = false;



//Assign the differnet components to variables and other "behind the scenes" stuff.
void Awake (){
	playersinv = GetComponent<Inventory>();

	if (useCustomPosition == false)
	{
		windowRect = new Rect(Screen.width-windowSize.x-70,Screen.height-windowSize.y-(162.5f+70*2),windowSize.x,windowSize.y);
	}
	else
	{
		windowRect = new Rect(customPosition.x,customPosition.y,windowSize.x,windowSize.y);
	}
	invAudio = GetComponent<InvAudio>();
	if (GetComponent<InventoryDisplay>().onOffButton == onOffButton)
	{
		invDispKeyIsSame = true;
	}
}

//Take care of the array lengths.
void Start (){
	ArmorSlot = new Item [ArmorSlotName.Length];
	if (buttonPositions.Length != ArmorSlotName.Length)
	{
		Debug.LogError("The variables on the Character script attached to " + transform.name + " are not set up correctly. There needs to be an equal amount of slots on 'ArmorSlotName' and 'buttonPositions'.");
	}
}

//Checking if we already have somthing equipped
bool CheckSlot ( int tocheck  ){
	bool toreturn = false;
	if(ArmorSlot[tocheck]!=null){
		toreturn=true;
	}
	return toreturn;
}

//Using the item. If we assign a slot, we already know where to equip it.
public void UseItem ( Item i ,  int slot ,  bool autoequip  ){
	 if(i.isEquipment){
		//This is in case we dbl click the item, it will auto equip it. REMEMBER TO MAKE THE ITEM TYPE AND THE SLOT YOU WANT IT TO BE EQUIPPED TO HAVE THE SAME NAME.
		if(autoequip)
		{
			float index = 0; //Keeping track of where we are in the list.
			float equipto = 0; //Keeping track of where we want to be.
			foreach(var a in ArmorSlotName) //Loop through all the named slots on the armorslots list
			{
				if(a==i.itemType) //if the name is the same as the armor type.
				{
					equipto=index; //We aim for that slot.
				}
				index++; //We move on to the next slot.
			}
			EquipItem(i,(int)equipto);
		}
		else //If we dont auto equip it then it means we must of tried to equip it to a slot so we make sure the item can be equipped to that slot.
		{
			if(i.itemType==ArmorSlotName[slot]) //If types match.
			{
				EquipItem(i,slot); //Equip the item to the slot.
			}
		}
	}
	if (DebugMode)
	{
		Debug.Log(i.name + " has been used");
	}
}

//Equip an item to a slot.
void EquipItem ( Item i ,  int slot  ){
	if(i.itemType == ArmorSlotName[slot]) //If the item can be equipped there:
	{
		if(CheckSlot(slot)) //If theres an item equipped to that slot we unequip it first:
		{
			UnequipItem(ArmorSlot[slot]);
			ArmorSlot[slot]=null;
		}
		ArmorSlot[slot]=i; //When we find the slot we set it to the item.
		
		gameObject.SendMessage ("PlayEquipSound", SendMessageOptions.DontRequireReceiver); //Play sound
		
		//We tell the Item to handle EquipmentEffects (if any).
		if (i.equipmentEffect != null)
		{
			equipmentEffectIs = true;
			i.GetComponent<EquipmentEffect>().EquipmentEffectToggle(equipmentEffectIs);
		}
		
		//If the item is also a weapon we call the PlaceWeapon function.
		if (i.isAlsoWeapon == true)
		{
			if (i.equippedWeaponVersion != null)
			{
				PlaceWeapon(i);
			}
			
			else 
			{
				Debug.LogError("Remember to assign the equip weapon variable!");
			}
		}
		if (DebugMode)
		{
			Debug.Log(i.name + " has been equipped");
		}
		
		playersinv.RemoveItem(i.transform); //We remove the item from the inventory
	}
}

//Unequip an item.
void UnequipItem ( Item i  ){
	gameObject.SendMessage ("PlayPickUpSound", SendMessageOptions.DontRequireReceiver); //Play sound
	
	//We tell the Item to disable EquipmentEffects (if any).
	if (i.equipmentEffect != null)
	{
		equipmentEffectIs = false;
		i.GetComponent<EquipmentEffect>().EquipmentEffectToggle(equipmentEffectIs);
	}
	
	//If it's a weapon we call the RemoveWeapon function.
	if (i.itemType == "Weapon")
	{
		RemoveWeapon(i);
	}
	if (DebugMode)
	{
		Debug.Log(i.name + " has been unequipped");
	}
	playersinv.AddItem(i.transform);
}

//Places the weapon in the hand of the Player.
void PlaceWeapon (Item item){
		GameObject Clone= GameObject.Instantiate(item.equippedWeaponVersion, WeaponSlot.position, WeaponSlot.rotation) as GameObject; 
		Clone.name = item.equippedWeaponVersion.name;
		Clone.transform.parent = WeaponSlot;
		if (DebugMode)
		{
			Debug.Log(item.name + " has been placed as weapon");
		}
}

//Removes the weapon from the hand of the Player.
void RemoveWeapon (Item item){	if (item.equippedWeaponVersion != null)
	{
		Destroy(WeaponSlot.FindChild(""+item.equippedWeaponVersion.name).gameObject);
		if (DebugMode)
		{
			Debug.Log(item.name + " has been removed as weapon");
		}
	}
}

void Update (){
	//This will turn the character sheet on and off.
	if (Input.GetKeyDown(onOffButton))
	{
		if (csheet)
		{
			csheet = false;
			if (invDispKeyIsSame != true)
			{
				gameObject.SendMessage ("ChangedState", false, SendMessageOptions.DontRequireReceiver); //Play sound
				gameObject.SendMessage("PauseGame", false, SendMessageOptions.DontRequireReceiver); //StopPauseGame/EnableMouse/ShowMouse
			}
		}
		else
		{
			csheet = true;
			if (invDispKeyIsSame != true)
			{
				gameObject.SendMessage ("ChangedState", true, SendMessageOptions.DontRequireReceiver); //Play sound
				gameObject.SendMessage("PauseGame", true, SendMessageOptions.DontRequireReceiver); //PauseGame/DisableMouse/HideMouse
			}
		}
	}
}

//Draw the Character Window
void OnGUI (){
	GUI.skin = cSheetSkin; //Use the cSheetSkin variable.
	
	if(csheet) //If the csheet is opened up.
	{
		//Make a window that shows what's in the csheet called "Character" and update the position and size variables from the window variables.
		windowRect=GUI.Window (1, windowRect, DisplayCSheetWindow, "Character");
	}
}

//This will display the character sheet and handle the buttons.
void DisplayCSheetWindow ( int windowID  ){
	if (canBeDragged == true)
	{
		GUI.DragWindow ( new Rect(0,0, 10000, 30));  //The window is dragable.
	}
	
	int index = 0;
	foreach(var a in ArmorSlot) //Loop through the ArmorSlot array.
	{
		if(a==null)
		{
			if(GUI.Button(buttonPositions[index], ArmorSlotName[index])) //If we click this button (that has no item equipped):
			{
				InventoryDisplay id=GetComponent<InventoryDisplay>();
				if(id.itemBeingDragged != null) //If we are dragging an item:
				{
					EquipItem(id.itemBeingDragged,index); //Equip the Item.
					id.ClearDraggedItem();//Stop dragging the item.
				}
			}
		}
		else
		{
			if(GUI.Button(buttonPositions[index],ArmorSlot[index].itemIcon)) //If we click this button (that has an item equipped):
			{
				InventoryDisplay id2=GetComponent<InventoryDisplay>();
				if(id2.itemBeingDragged != null) //If we are dragging an item:
				{
					EquipItem(id2.itemBeingDragged,index); //Equip the Item.
					id2.ClearDraggedItem(); //Stop dragging the item.
				}
				else if (playersinv.Contents.Length < playersinv.MaxContent) //If there is room in the inventory:
				{
					UnequipItem(ArmorSlot[index]); //Unequip the Item.
					ArmorSlot[index] = null; //Clear the slot.
					id2.ClearDraggedItem(); //Stop dragging the Item.
				}
				else if (DebugMode)
				{
					Debug.Log("Could not unequip " + ArmorSlot[index].name + " since the inventory is full");
				}
			}
		}
		index++;
	}
}

}