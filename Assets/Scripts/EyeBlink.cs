using UnityEngine;
using System.Collections;

public class EyeBlink : MonoBehaviour
{

    //humans blink once every 2-10 seconds so every 2*(1-5) s.
    //a blink lasts 300 to 400 ms so 0.3-0.4 s so 0.1* (3-4) s

    bool blinking;         //is the character currently blinking?
    private float currTime;//current timer
    public GameObject eyelids; //object to activate/deactivate


    // Use this for initialization
    void Start()
    {
        currTime = openTime(); //establish current timer as open eyes
        blinking = false;                       //we are not blinking
    }
    
    // Update is called once per frame
    void Update()
    {
        if(currTime > 0.0f) //if the timer is greater than 0
        {
            currTime -= Time.deltaTime; //subtract time
        }
        else //if the timer is up
        {
            if(blinking) //if the character is blinking
            {
                currTime = openTime();  //reset the timer to the open time + a random value
                eyelids.SetActive(false);
                blinking = false;  //we are no longer blinking
            }
            else  //if the character is not blinking
            {
                currTime = closeTime();  //reset the timer to blinking time + a random value
                eyelids.SetActive(true); //set the texture to blinky eyes
                blinking = true;  //we are now blinking
            }
        }
    }

    float openTime()
    {
        return 2 * (Random.Range(1f, 5f));
    }

    float closeTime()
    {
        return 0.1f * Random.Range(3f,4f);
    }
}
