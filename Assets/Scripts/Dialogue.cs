﻿using UnityEngine;
using System.Collections;

public class Dialogue : Interaction {

	//public int dialogueNum = 3;

	protected override void doStart ()
	{

	}
	public override void Update ()
	{

	}
	public override void action(){
		//dm.startDialogue(dialogueNum);
		Debug.Log ("Dialogue");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PointClick> ().activate ();
	}

	public override void secondary(){
		//description code if necessary
	}

	
	public void startDialogue(){
		
		
	}
   
}
