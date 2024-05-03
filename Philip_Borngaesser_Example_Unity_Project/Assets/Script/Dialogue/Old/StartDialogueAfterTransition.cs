using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SaveTriggerEvent))]
public class StartDialogueAfterTransition : MonoBehaviour
{
    public DialogueCollection startDialogue;
    public TransitionHandler transitionHandler;
    public NonPlayerDialogueManager playerDialgueManager;
    SaveTriggerEvent saveTriggerEvent;


    private void Awake()
    {
        saveTriggerEvent = GetComponent<SaveTriggerEvent>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        StartDialogue();
        saveTriggerEvent.SaveEvent();
    }

    private void StartDialogue()
    {
        playerDialgueManager.SetDialogue(startDialogue);
        playerDialgueManager.OnStartDialogue(playerDialgueManager.transform);
    }

}
