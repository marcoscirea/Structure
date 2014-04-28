using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

    int width = 300;
    int height = 90;
    int center_x;
    int center_y;

    bool playerMoveStatus;
    PointClick playerControl;

    bool show = false;

	// Use this for initialization
	void Start () {

        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PointClick>();

        center_x = (Screen.width / 2);
        center_y = (Screen.height / 2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI(){
        if (show)
        {
            GUI.Box(new Rect(center_x - (width / 2), center_y - (height / 2), width, height), "Exit To Desktop?");
            if (GUI.Button(new Rect(center_x - 90, center_y - 10, 80, 20), "Yeah"))
            {
                Application.Quit();
            }
            if (GUI.Button(new Rect(center_x + 10, center_y - 10, 80, 20), "Not yet"))
            {
                playerControl.canMove = playerMoveStatus;
                Time.timeScale = 1f;
                show = false;
            }
        }
    }

    public void Activate(){
        show = !show;

        if (show)
        {
            playerMoveStatus = playerControl.canMove;
            playerControl.canMove = false;
            Time.timeScale = 0;
        } else
        {
            playerControl.canMove = playerMoveStatus;
            Time.timeScale = 1f;
            show = false;
        }
    }
}
