using UnityEngine;
using System.Collections;

public class CameraNode : MonoBehaviour
{

    public float smoothTime = 5f;
    public float cameraSize;

    void Awake()
    {
        cameraSize = gameObject.GetComponent<Camera>().orthographicSize;
        gameObject.GetComponent<Camera>().enabled = false;
    }

    // Use this for initialization
    void Start()
    {
    
    }
    
    // Update is called once per frame
    void Update()
    {
    
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision with " + other.gameObject.name);
        if (other.tag == "Player")
        {
            CameraMovement.moveTo(transform.position, smoothTime, cameraSize);
        }
    }
}
