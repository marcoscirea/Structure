using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{

    static bool move = false;
    static bool zoom = false;
    static Vector3 target;
    static float smoothTime;
    static float size;
    private static Vector3 velocity = Vector3.zero;
    private static float zoomVelocity = 0;
    public GameObject startingNode;

    void Awake()
    {
        try
        {
            transform.position = startingNode.transform.position;
            Camera.main.orthographicSize = startingNode.GetComponent<CameraNode>().cameraSize;
        
        } catch
        {
            Debug.Log("Camera is missing starting node!");
        }
    }

    // Use this for initialization
    void Start()
    {
    
    }
    
    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
            if (Vector3.Distance(transform.position, target) < 0.01f)
                move = false;
        }

        if (zoom)
        {
            Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, size, ref zoomVelocity, smoothTime);
            if (Mathf.Abs(Camera.main.orthographicSize - size) < 0.01f)
                zoom = false;
        }
    }

    public static void moveTo(Vector3 t, float s, float cam)
    {
        target = t;
        smoothTime = s;
        size = cam;
        move = true;
        zoom = true;
        velocity = Vector3.zero;
        zoomVelocity = 0;
    }
}
