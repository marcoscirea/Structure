using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {


string LevelName = "Level02";
string PlayerName = "Player";

[AddComponentMenu ("Inventory/Other/ChangeScene")]

void Awake (){
	DontDestroyOnLoad (GameObject.Find(PlayerName));
}

void Update (){
	if (Input.GetKeyDown(KeyCode.T))
	{
		Application.LoadLevel("Level02");
	}
}
}