using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NonPlayerDialogueManager))]
public class DialogueEventHandler : MonoBehaviour
{
    NonPlayerDialogueManager _nonPlayerDialogueManager;
    public List<DialogueEvent> dialogueEvents;

    private void Start()
    {
        _nonPlayerDialogueManager = GetComponent<NonPlayerDialogueManager>();
        _nonPlayerDialogueManager.onDialogueDataHasChanged.AddListener(OnDataChangeEvent);
    }

    private void OnDataChangeEvent(DialogueNodeData data)
    {
        foreach(DialogueEvent dialogueEvent in dialogueEvents)
        {
            if(data.GUID == dialogueEvent.data.GUID)
            {
                dialogueEvent.onDialogueHasPlayed.Invoke();
            }
        }
    }
}
