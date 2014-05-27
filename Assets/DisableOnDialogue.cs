using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class DisableOnDialogue :  ActOnDialogueEvent {

    //public MonoBehaviour[] scripts;

    public override void TryStartActions(Transform actor){
        Debug.Log("starting dialogue");
    }

    public override void TryEndActions(Transform actor){

    }
}
