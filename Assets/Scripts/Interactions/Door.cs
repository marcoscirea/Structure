using UnityEngine;
using System.Collections;

public class Door : Interaction {

	public string to;
    public Vector3 exitDoorWalkpoint;

	protected override void doStart ()
	{
		
	}
	// Update is called once per frame
	public override void Update () {
	
	}

	public override void action ()
	{
		//Application.LoadLevel (to);
        PointClick.exitThroughDoor(exitDoorWalkpoint);
		LoadingScreen.Load(to);
	}

	public override void secondary ()
	{
		throw new System.NotImplementedException ();
	}
}
