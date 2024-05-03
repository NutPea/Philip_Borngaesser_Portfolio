using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SaveTriggerEvent))]
public class MovementNodeDialogueTrigger : MonoBehaviour
{
    public List<MovementNodeStepTrigger> stepTriggers;
    public DialogueSystemTrigger dialogueSystemTrigger;
    private SaveTriggerEvent _saveTriggerEvent;

    public Transform dialogueTransform;
    void Start()
    {
        _saveTriggerEvent = GetComponent<SaveTriggerEvent>();
        dialogueSystemTrigger = GetComponent<DialogueSystemTrigger>();
        GetComponentsInChildren<MovementNodeStepTrigger>(stepTriggers);
        foreach (MovementNodeStepTrigger stepTrigger in stepTriggers)
        {
            stepTrigger.onPlayerSteppedTrigger.AddListener(OnStartDialogue);
        }
    }

    private void OnStartDialogue(Transform player)
    {
       dialogueSystemTrigger.TryStart(null);

    }

    public void DialogueHasBeenStarted()
    {
        _saveTriggerEvent.SaveEvent();
        gameObject.SetActive(false);

    }

}
