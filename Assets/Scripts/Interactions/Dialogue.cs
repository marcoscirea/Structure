using UnityEngine;
using System.Collections;

public class Dialogue : Interaction
{

    //public int dialogueNum = 3;

    protected override void doStart()
    {

    }

    public override void Update()
    {

    }

    public override void action()
    {
        //dm.startDialogue(dialogueNum);
        Debug.Log("Dialogue");
        //gameObject.SendMessage("OnConversation", this.transform, SendMessageOptions.DontRequireReceiver);
        gameObject.SendMessage("OnUse", GameObject.FindGameObjectWithTag("Player").transform, SendMessageOptions.DontRequireReceiver);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PointClick>().activate();
    }

    public override void secondary()
    {
        //description code if necessary
    }
    
    public void startDialogue()
    {
        
        
    }
   
}
