using UnityEngine;
using System.Collections;

public class WeaponSwitch : MonoBehaviour {
	#pragma warning disable 

//This is an example of how WeaponSwitching could be handled together with the Inventory System in a First Person game.
//This method is fairly taxing but works great for quickly switching between weapons and having the weapons displayed in the top left corner.
//To learn how to use/write this kind of script, please visit http://youtube.com/brackeys/ since we create a similar (not as advanced) script in our Survival Game Series.
//Attach the script to the object which the weapons are parented to when equipped.

private int currentWeapon = 0; //The Weapon currently selected as an int.
int maxWeapons = 2; //The maximum number of weapons the Player can carry.

Transform Fists; //The default 'Fists' object to use when nothing is equipped. The system will make sure that there is always a 'Fists' object unless all weaponSlots are filled.
bool fistsOnObject = true; //Are there a 'Fists' object?

GUISkin theSkin; //This is where you can assign a custom GUI skin or use the one included (OtherSkin) under the Resources folder.

bool switchBetweenAnimations = false; //Set this to true if your Character/Arms has animations for when holding a weapon for using fists only. This technique can also be applied if you want different weapons to have different animations.
Animator theAnimator; //This is the Animator that we will use for the above.

[AddComponentMenu ("Inventory/Other/Weapon Switch")]

//Load the default skin if nothing has been put in.
void Awake (){
	if (theSkin == null)
	{
		theSkin = Resources.Load("OtherSkin") as GUISkin;
	}
}

void Update (){
	//Handle the Fists
	if (transform.childCount-1 < maxWeapons && fistsOnObject == false)
	{
		Transform Clone= Instantiate(Fists, transform.position, transform.rotation) as Transform;
		Clone.transform.parent = transform;
		Clone.gameObject.name = "Fists";
		fistsOnObject = true;
	}
	if (transform.childCount-1 > maxWeapons)
	{
		Destroy(transform.FindChild("Fists").gameObject);
		fistsOnObject = false;
	}
	
	//Change weapons using the Scrollwheel.
	if(Input.GetAxis("Mouse ScrollWheel") > 0)
	{
		if(currentWeapon + 1 <= maxWeapons)
		{
			currentWeapon++;
		}
		else
		{
			currentWeapon = 0;
		}
	}
	else if (Input.GetAxis("Mouse ScrollWheel") < 0)
	{
		if(currentWeapon - 1 >= 0)
		{
			currentWeapon--;
		}
		else
		{
			currentWeapon = maxWeapons;
		}
	}
	
	//Make the weapons "loop" when exceeding the maxWeapons value.
	if(currentWeapon > maxWeapons)
	{
		currentWeapon = 0;
	}
	if(currentWeapon <= -1)
	{
		currentWeapon = maxWeapons;
	}
	
	//Select a weapon using the number keys.
	if(Input.GetKeyDown(KeyCode.Alpha1))
	{
		currentWeapon = 0;
	}
	if(Input.GetKeyDown(KeyCode.Alpha2) && maxWeapons >= 1)
	{
		currentWeapon = 1;
	}
	if(Input.GetKeyDown(KeyCode.Alpha3) && maxWeapons >= 2)
	{
		currentWeapon = 2;
	}
	
	//Make sure that the currentWeapon doesn't exceed the number of weapons.
	while (currentWeapon > transform.childCount-1)
	{
		currentWeapon -= 1;
	}
	
	//Call the SelectWeapon function.
	SelectWeapon(currentWeapon);

}

//Selects the weapon based on the currentWeapon variable.
void SelectWeapon ( int index  ){
	for (int i = 0; i < transform.childCount; i++) //Loop through the weapons.
	{
		//Activate the selected weapon
		if (i == index)
		{
			if (switchBetweenAnimations == true) //If the 'switchBetweenAnimations' variable is true we change the animation to fit the Weapon. In this case if we are using one or not.
			{
				if (transform.GetChild(i).name == "Fists")
				{
					theAnimator.SetBool("WeaponIsOn", false);
				}
				else
				{
					theAnimator.SetBool("WeaponIsOn", true);
				}
			}
			//Activate the match
			transform.GetChild(i).gameObject.SetActive(true);
		}
		else
		{
			//Deactivate all others
			transform.GetChild(i).gameObject.SetActive(false);
		}
	}
}

//Show the selected weapon in the top right corner and the others less visible.
void OnGUI (){
	if (theSkin != null)
	{
		GUI.skin = theSkin;
	}

	GUILayout.BeginArea ( new Rect(10,10,400,50));
	GUILayout.BeginHorizontal ();
	GUI.color = new Color(1, 1, 1, 0.7f);
	GUILayout.Box("Weapons:");
	for (int i = 0; i < transform.childCount; i++)
	{
		Transform theChild = transform.GetChild(i);
		if (currentWeapon == i)
		{
			GUI.color = new Color(1, 1, 1, 0.7f);
		}
		else
		{
			GUI.color = new Color(1,1,1,0.4f);
		}
		GUILayout.Box("" + theChild.name);
	}
	GUILayout.EndHorizontal();
	GUILayout.EndArea();
}
	#pragma warning enable 
}