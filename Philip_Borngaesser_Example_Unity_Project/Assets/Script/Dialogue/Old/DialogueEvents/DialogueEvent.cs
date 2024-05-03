using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueEvent 
{
    public UnityEvent onDialogueHasPlayed = new UnityEvent();
    public DialogueNodeData data;
}
