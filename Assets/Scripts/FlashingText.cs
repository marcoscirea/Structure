using UnityEngine;
using System.Collections;

public class FlashingText : MonoBehaviour {

    public Color from = Color.clear;
    public Color to = Color.white;

    float startTime = 0f;
    bool change = true;
    TextMesh text;

	// Use this for initialization
	void Start () {
        text = gameObject.GetComponent<TextMesh>();
        text.color = from;
	}
	
	// Update is called once per frame
	void Update () {
        if (change)
        {
            text.color = Color.Lerp(from, to, Time.time - startTime);
            if (text.color.Equals(to)){
                change=!change;
                startTime = Time.time;
            }
        }
        else 
        {
            text.color = Color.Lerp(to, from, Time.time - startTime);
            if (text.color.Equals(from)){
                change=!change;
                startTime = Time.time;
            }
        }
	}
}
