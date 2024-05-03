using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ShowDialogue))]
public class DialogueChangeEvent : MonoBehaviour
{
    public int dialogueIndex = 0;
    private ShowDialogue _showDialogue;
    public UnityEvent onTargetDialogueHasChanged = new UnityEvent();
    public UnityEvent onTargetDialogueHasChangedPreviously = new UnityEvent();

    void Awake()
    {
        _showDialogue = GetComponent<ShowDialogue>();
        _showDialogue.onDialogueHasChanged.AddListener(OnTargetDialogueHasChanged);
    }

    private void OnTargetDialogueHasChanged(int eventIndex)
    {
        if (eventIndex >= dialogueIndex) onTargetDialogueHasChangedPreviously.Invoke();
        if (eventIndex == dialogueIndex) onTargetDialogueHasChanged.Invoke();
    }


}
