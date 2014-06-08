using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class SaveLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyUp(KeyCode.S))
        {
            GetComponent<GameSaver>().SaveGame();
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            GetComponent<GameSaver>().LoadGame();
        }
	}
}
