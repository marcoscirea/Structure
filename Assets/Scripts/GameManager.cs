using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    static GameManager instance;

    public GameObject fadeAndLoadCamera;
    public GameObject exitPrompt;
    public GameObject loadingScreen;
	public GameObject pathfinding;

    //public bool debug = true;

    
    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;    
        DontDestroyOnLoad(this); 

        /*if (!debug)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("CameraNode")){
                g.transform.GetChild(0).gameObject.SetActive(false);
            }

            foreach(GameObject g in GameObject.FindGameObjectsWithTag("Waypoint")){
                g.transform.GetChild(0).gameObject.SetActive(false);
            }

            foreach(GameObject g in GameObject.FindGameObjectsWithTag("Walkable")){
                g.renderer.enabled=false;
            }
        }*/
    }

    void Start(){
        if (GameObject.FindGameObjectWithTag("ExitPrompt") == null)
            Instantiate(exitPrompt);
        if (GameObject.FindGameObjectWithTag("Loading") == null)
            Instantiate(loadingScreen);
        if (GameObject.FindGameObjectWithTag("FadeAndLoad") == null)
            Instantiate(fadeAndLoadCamera);
		if (GameObject.FindGameObjectWithTag("Path") == null)
			Instantiate(pathfinding);
    }

	void OnLevelWasLoaded(int n){
        if (GameObject.FindGameObjectWithTag("ExitPrompt") == null)
            Instantiate(exitPrompt);
        if (GameObject.FindGameObjectWithTag("Loading") == null)
            Instantiate(loadingScreen);
        if (GameObject.FindGameObjectWithTag("FadeAndLoad") == null)
            Instantiate(fadeAndLoadCamera);
		if (GameObject.FindGameObjectWithTag("Path") == null)
			Instantiate(pathfinding);
    }

}
