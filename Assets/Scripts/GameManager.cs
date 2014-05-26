using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    static GameManager instance;

    public GameObject fadeAndLoadCamera;
    public GameObject exitPrompt;
    public GameObject loadingScreen;
	public GameObject pathfinding;

    
    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;    
        DontDestroyOnLoad(this); 
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
