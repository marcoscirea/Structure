using UnityEngine;
using System.Collections;

public class Pickable : Interaction
{

    
    public Vector3 invScale;
    public bool inInventory = false;
    public bool clicked = false;
    OldInventory inventory;
    public bool isActive = true;

    protected override void doStart()
    {
        if (inventory == null)
            inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<OldInventory>();

        //check if object already picked up
        if (inventory.hasBeenPickedUp(gameObject.name))
            Destroy(gameObject);
    }

    public override void Update()
    {

        checkForChangedObjects();

        if (clicked)
        {
            inventory.stopUpdate(true);
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -0.5f);
        }
    }

    public override void action()
    {

        if (!inInventory)
        {
            if (isActive)
            {
                inventory.addItem(gameObject);
                GameObject.FindGameObjectWithTag ("Player").GetComponent<PointClick> ().activate ();
                inInventory = true;

                //Picking up animation
                GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetTrigger("Pickup");

            }

            //dialogue when picking up item
            //dm.startDialogue(dialogue);
        } else
        {
            clicked = true;
            gameObject.collider.enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PointClick>().usingItem(gameObject);
        }

    }

    public override void secondary()
    {
        //unselect if selected
        if (clicked)
        {
            clicked = false;
            gameObject.collider.enabled = true;
            inventory.stopUpdate(false);
            //inventory.updateItems();
        }
    }

    public void useWith(GameObject other)
    {

        bool success = false;
        //interact with object
        //Debug.Log("Interact!");
        /*switch (other.gameObject.name)
        {

        //Combining items template

            case "Hat":
                switch (gameObject.name){
                    case "Eyes":
                        Debug.Log("combining items!");
                        break;
                }
                break; 
        }*/
    
        //last operations
        if (success)
        {
            clicked = false;
            inventory.removeItem(gameObject);
            //inventory.updateItems ();
            gameObject.collider.enabled = true;
            inventory.stopUpdate(false);
        } else
            secondary();
    }

    void checkForChangedObjects()
    {
        //needed when changing scene to have the new dialogue manager etc
        if (inventory == null)
            inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<OldInventory>();
    }
}
