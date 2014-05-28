using UnityEngine;
using System.Collections;

public class ParallaxDisable : MonoBehaviour {

    void Awake(){
        if (GameObject.FindGameObjectsWithTag("ParallaxFront").Length==0)
        {
            Debug.Log("No ParallaxFront objects, disabling relative camera");
            transform.FindChild("ParallaxFrontCamera").gameObject.SetActive(false);
        }
        if (GameObject.FindGameObjectsWithTag("ParallaxBack").Length==0)
        {
            Debug.Log("No ParallaxFront objects, disabling relative camera");
            transform.FindChild("ParallaxBackCamera").gameObject.SetActive(false);
        }
    }
}
