using UnityEngine;
using System.Collections;
using UnityEditor;
//THIS IS THE EDITOR WINDOW. DON'T TOUCH THIS :)
#pragma warning disable
class InventoryEditor: EditorWindow {
	string playerName = "Player";
	bool groupEnabled = false;
	bool myBool = true;
	float myFloat = 1.23f;
	GUISkin invGUI;
	GUISkin cSheetGUI;
	bool includeCSheet = true;
	Transform WeaponHolder;
	private Transform WeaponHolderBack;
	int pauseGameInt = 0;
	string[] pauseGameOptions = {"Pause Game + Disable Mouse", "Pause Game", "Disable Mouse", "Keep playing"};
	
	Transform theCamera;
	
	int taskTBInt = 0;
    string[] taskTBStrings = {"Set up Inventory", "Make Items"};
	
	Vector2 scrollPos;
	
	Transform selectedObject;
	string selected = "";
	
	// Add menu named "My Window" to the Window menu
	[MenuItem ("Window/Inventory %i")]
	
	static void Init (){
		// Get existing open window or if none, make a new one:
		InventoryEditor window= ScriptableObject.CreateInstance<InventoryEditor>();
		window.title = "Inventory";
		window.Show();
	}
	
	void OnGUI (){
		EditorGUILayout.BeginVertical();
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		
		taskTBInt = GUILayout.Toolbar (taskTBInt, taskTBStrings);
		Object cSheetGUI = null;
		Object invGUI = null;
		Object theCamera = null;
		Object WeaponHolder = null;
		if (taskTBInt == 0)
		{
			if (Selection.transforms.Length > 0)
			{
				selectedObject = Selection.activeTransform;
			}
			else
			{
				EditorGUILayout.Space();
				
				GUILayout.Label ("---> Select the Player in the scene to get started! <---", EditorStyles.wordWrappedLabel);
				
				EditorGUILayout.Space();
				
				GUILayout.Label ("Learn how", EditorStyles.boldLabel);
				GUILayout.Label ("Visit:  http://brackeys.com/inventory", EditorStyles.wordWrappedLabel);
				
				EditorGUILayout.Space();
				
				if(GUILayout.Button("Close"))
				{
					this.Close();
				}
				
				GUILayout.Label ("Current version:  1.1f.1", EditorStyles.wordWrappedLabel);
				
				EditorGUILayout.EndScrollView();
				EditorGUILayout.EndVertical();
				return;
			}
		
			if (invGUI == null)
			{
				//"Assets/Inventory/GUI/InventorySkin"
			}
			
			EditorGUILayout.Space();
			
			GUILayout.Label ("This section sets up the Inventory system automatically and as recommended. Just make sure to select the Player, fill in the fields below and hit the 'Set it up!' button!", EditorStyles.wordWrappedLabel);
			
			EditorGUILayout.Space();
			
			includeCSheet = EditorGUILayout.BeginToggleGroup ("Include Character Sheet (Highly recommended)", includeCSheet);
			GUILayout.Label ("This is where you can add a custom skin or use the one included (CSheetSkin).", EditorStyles.wordWrappedLabel);
			
			cSheetGUI = EditorGUILayout.ObjectField("Character Sheet Skin", cSheetGUI,typeof (GUISkin), false);
			if (cSheetGUI == null)
			{
				cSheetGUI = Resources.Load("CSheetSkin") as GUISkin;
			}
			GUILayout.Label ("This is the object which the equipped weapons are going to be parented to.", EditorStyles.wordWrappedLabel);
			WeaponHolder = EditorGUILayout.ObjectField ("Weapon Holder ", WeaponHolder, typeof (Transform), true);
			EditorGUILayout.EndToggleGroup ();
			 
			EditorGUILayout.Space();
			
			GUILayout.Label ("This is where you can add a custom skin for the Inventory or use the one included (InventorySkin).", EditorStyles.wordWrappedLabel);
			invGUI = EditorGUILayout.ObjectField("Inventory Skin", invGUI, typeof (GUISkin), false);
			if (invGUI == null)
			{
				invGUI = Resources.Load("InventorySkin") as GUISkin;
			}
			
			EditorGUILayout.Space();
			
			GUILayout.Label ("Choose what happens when a window is open. Options that disable the mouse will deactive the 'MouseLook' component included in the standard First Person Controller.", EditorStyles.wordWrappedLabel);
			pauseGameInt = EditorGUILayout.Popup(pauseGameInt, pauseGameOptions);
			EditorGUI.BeginDisabledGroup (pauseGameInt == 3 || pauseGameInt == 1);
			theCamera = EditorGUILayout.ObjectField ("Player Camera ", theCamera,typeof (Transform), true);
			EditorGUI.EndDisabledGroup ();
			
			EditorGUILayout.Space();
			
			if (Selection.transforms.Length <= 1)
			{
				selected +=  selectedObject.name + " ";
				EditorGUILayout.LabelField("Selected Object: ", selected);
				selected = "";
			}
			else
			{
				EditorGUILayout.LabelField("More than one object selected, please correct this.");
			}
			
			GUI.color =new Color(0.5f, 1, 0.5f, 1);
			if(GUILayout.Button("Set it up!", GUILayout.Height(30)))
			{
				if (Selection.transforms.Length <= 1)
				{
					InventorySetUp();
				}
				else
				{
					Debug.LogError("Select only one gameobject");
				}
			}
			GUI.color = Color.white;
			
			GUI.color = new Color(1, 0.5f, 0.5f, 1);
			if(GUILayout.Button("Remove Inventory"))
			{
				if (Selection.transforms.Length <= 1)
				{
					InventoryDelete();
				}
				else
				{
					Debug.LogError("Select only one gameobject");
				}
			}
			GUI.color = Color.white;
		}
		else if (taskTBInt == 1)
		{
			EditorGUILayout.Space();
			GUILayout.Label ("This is done manually. Read the 'Guide' to learn how to create Items.", EditorStyles.wordWrappedLabel);
		}
		
		EditorGUILayout.Space();
		
		GUILayout.Label ("Learn how", EditorStyles.boldLabel);
		GUILayout.Label ("Visit:  http://brackeys.com/inventory", EditorStyles.wordWrappedLabel);
		
		EditorGUILayout.Space();
		
		if(GUILayout.Button("Close"))
		{
			this.Close();
		}
		
		GUILayout.Label ("Current version:  1.1f.1", EditorStyles.wordWrappedLabel);
		
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}

	void InventorySetUp (){
	
		InventoryDelete();
		
		selected +=  selectedObject.name + " ";
		
		if (selectedObject.FindChild("Inventory") != null)
		{
			Debug.LogError("An object called 'Inventory' was already found on " + selected + ". If the Inventory has previously been set up on the selected object (" + selected + ") then make sure to delete it by pressing 'Remove Inventory'.");
			return;
		}
		
		GameObject invObject= new GameObject ("Inventory");
		invObject.transform.parent = selectedObject;
		invObject.transform.position = selectedObject.position;
		invObject.transform.rotation = selectedObject.rotation;
		//invObject.transform.position.y += 0.5f;
		invObject.transform.position = new Vector3(invObject.transform.position.x,invObject.transform.position.y+0.5f,invObject.transform.position.z);
		invObject.AddComponent<Inventory>();
		invObject.AddComponent<InventoryDisplay>();
		
		if (includeCSheet)
		{
			invObject.AddComponent<Character>();
			
			if (selectedObject.GetComponent<Character>() != null)
			{
				Debug.LogWarning("The Character Sheet (Character) script was found on the selected object (" + selected + "). Make sure to delete all traces of any previously installed Inventory. There should only be an 'Inventory', 'InventoryDisplay' and 'Character' script on an Inventory gameobject parented to the Player. The Inventory was still set up though.");
			}
		}
		/*
		//Set up the cSheet variables
		Character csheet= invObject.GetComponent<Character>();
		
		if (cSheetGUI != null)
		{
			csheet.cSheetSkin = cSheetGUI;
		}
		else
		{
			Debug.LogWarning("Assign the CSheetSkin");
		}
		
		if (WeaponHolder != null)
		{
			csheet.WeaponSlot = WeaponHolder;
		}
		else
		{
			Debug.LogError("The Weapon Holder variable wasn't assigned. Make sure to assign it manually under the Character script or remove the inventory and add it again with the variable assigned.");
		}

		//Set up ArmorSlotNames
		csheet.ArmorSlotName = new string [6];
		csheet.ArmorSlotName[0] = "Head";
		csheet.ArmorSlotName[1] = "Chest";
		csheet.ArmorSlotName[2] = "Leg";
		csheet.ArmorSlotName[3] = "Weapon";
		csheet.ArmorSlotName[4] = "Weapon";
		csheet.ArmorSlotName[5] = "Weapon";
		
		//Set up buttonPositions
		csheet.buttonPositions = new Rect [6];
		csheet.buttonPositions[0].x = 12;
		csheet.buttonPositions[0].y = 34;
		csheet.buttonPositions[0].width = 80;
		csheet.buttonPositions[0].height = 80;
		csheet.buttonPositions[1].x = 12;
		csheet.buttonPositions[1].y = 120;
		csheet.buttonPositions[1].width = 80;
		csheet.buttonPositions[1].height = 80;
		csheet.buttonPositions[2].x = 12;
		csheet.buttonPositions[2].y = 206;
		csheet.buttonPositions[2].width = 80;
		csheet.buttonPositions[2].height = 80;
		csheet.buttonPositions[3].x = 99;
		csheet.buttonPositions[3].y = 34;
		csheet.buttonPositions[3].width = 80;
		csheet.buttonPositions[3].height = 80;
		csheet.buttonPositions[4].x = 99;
		csheet.buttonPositions[4].y = 120;
		csheet.buttonPositions[4].width = 80;
		csheet.buttonPositions[4].height = 80;
		csheet.buttonPositions[5].x = 99;
		csheet.buttonPositions[5].y = 206;
		csheet.buttonPositions[5].width = 80;
		csheet.buttonPositions[5].height = 80;
		*/
		//Set up the InventoryDisplay variables
		InventoryDisplay invDisp= invObject.GetComponent<InventoryDisplay>();
		invDisp.invSkin = invGUI;
		InvPauseGame pauseGame = new InvPauseGame();
		//Set up the InvPauseGame (if any)
		if (pauseGameInt != 3)
		{
			pauseGame = invObject.AddComponent<InvPauseGame>();
			pauseGame.ThePlayer = selectedObject;
			pauseGame.TheCamera = theCamera;
		}
		switch ((int)pauseGameInt)
		{
			case 1:
				pauseGame.disableMouseLookComponent = false;
				break;
			case 2:
				pauseGame.pauseGame = false;
				break;
		}
		
		//Set up the InvAudio
		AudioSource theSource= invObject.AddComponent<AudioSource>();
		theSource.audio.volume = 0.75f;
		theSource.audio.playOnAwake = false;
		invObject.AddComponent<InvAudio>();
		
		//Handle errors and complications (if any) to avoid compiler errors
		if (selectedObject.GetComponent<Inventory>() != null)
		{
			Debug.LogWarning("The 'Inventory' script was found on the selected object (" + selected + "). Make sure to delete all traces of any previously installed Inventory. There should only be an 'Inventory', 'InventoryDisplay' and 'Character' script on an Inventory gameobject parented to the Player. The Inventory was still set up though.");
		}
		
		if (selectedObject.GetComponent<InventoryDisplay>() != null)
		{
			Debug.LogWarning("The 'InventoryDisplay' script was found on the selected object (" + selected + "). Make sure to delete all traces of any previously installed Inventory. There should only be an 'Inventory', 'InventoryDisplay' and 'Character' script on an Inventory gameobject parented to the Player. The Inventory was still set up though.");
		}
		
		if (includeCSheet)
		{
			Debug.Log("Inventory and Character Sheet has been set up on " + selected + " under a new GameObject called 'Inventory'");
		}
		else
		{
			Debug.Log("Inventory has been set up on " + selected + " under a new GameObject called 'Inventory'");
		}
		
		selected = "";
	}
	
	void InventoryDelete (){
		Undo.RegisterSceneUndo("PlayersInv");
	
		selected +=  selectedObject.name + " ";
		
		if (selectedObject.FindChild("Inventory") != null)
		{
			DestroyImmediate(selectedObject.FindChild("Inventory").gameObject);
		}
		
		if (selectedObject.GetComponent<Character>() != null)
		{
			DestroyImmediate(selectedObject.gameObject.GetComponent<Character>());
		}
		
		if (selectedObject.GetComponent<Inventory>() != null)
		{
			DestroyImmediate(selectedObject.gameObject.GetComponent<Inventory>());
		}
		
		if (selectedObject.GetComponent<InventoryDisplay>() != null)
		{
			DestroyImmediate(selectedObject.gameObject.GetComponent<InventoryDisplay>());
		}
		
		Debug.Log("Inventory has been removed from " + selected + ". This is always done automatically before the Inventory get's set up.");
		selected = "";
	}
	
	void OnInspectorUpdate (){
		Repaint();
	}
	#pragma warning enable 
}