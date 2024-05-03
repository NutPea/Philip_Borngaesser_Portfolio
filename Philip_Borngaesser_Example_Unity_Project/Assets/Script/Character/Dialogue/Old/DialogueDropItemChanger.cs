using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShowDialogue))]
[RequireComponent(typeof(DropItemHandler))]
public class DialogueDropItemChanger : MonoBehaviour
{
    private ShowDialogue _showDialogue;
    private DropItemHandler _dropItemHandler;
    [Header("DialogueDrop")]
    public int canDropDialogueIndex = 0;
    public int changeDialogueIndex = 0;

    [Header("FailToDrop")]
    public bool extraDialogeWhenSpawnItemFails;
    public int extraDialogeIndex;


    [Header("ItemPickUp")]
    public bool canChangeOnItemPickUp;
    public int itemPickUpIndex;

    void Start()
    {
        _showDialogue = GetComponent<ShowDialogue>();
        _dropItemHandler = GetComponent<DropItemHandler>();

        _showDialogue.onDialogueHasPrinted.AddListener(OnDialogueChange);
        _dropItemHandler.onCantDropItem.AddListener(OnShowExtraDialogueEvent);
        _dropItemHandler.onDropItem.AddListener(OnDropItemEvent);
        _dropItemHandler.onItemGotPickedUp.AddListener(OnItemPickedUpEvent);

    }

    private void OnItemPickedUpEvent()
    {
        if (!canChangeOnItemPickUp) return;
        _showDialogue.JumpToDialogue(itemPickUpIndex);
        _showDialogue.PrintNextDialogue();
    }

    private void OnShowExtraDialogueEvent()
    {
        if (!extraDialogeWhenSpawnItemFails) return;
        _showDialogue.ChangeDialogue(extraDialogeIndex);
    }

    private void OnDialogueChange(int dialogueIndex)
    {
        if(dialogueIndex == canDropDialogueIndex)
        {
            //_dropItemHandler.ActivateDropOnInteract();
        }
    }
    private void OnDropItemEvent()
    {
        _showDialogue.JumpToDialogue(changeDialogueIndex);
        _showDialogue.PrintNextDialogue();
        //_dropItemHandler.dropOnInteracting = false;
    }




}
