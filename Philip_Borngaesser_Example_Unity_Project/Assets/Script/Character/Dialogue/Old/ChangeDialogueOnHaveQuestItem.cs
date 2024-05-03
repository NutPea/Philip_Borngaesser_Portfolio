using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ShowDialogue))]
public class ChangeDialogueOnHaveQuestItem : MonoBehaviour
{
    public QuestItem.Item item;
    public int newDialogueIndex;
    private ShowDialogue _showDialogue;
    void Start()
    {
        _showDialogue = GetComponent<ShowDialogue>();
        _showDialogue.onShowDialogue.AddListener(OnShowDialogueEvent);
    }

    private void OnShowDialogueEvent()
    {
        PlayerInventoryHandler playerInventoryHandler = _showDialogue._player.GetComponent<PlayerInventoryHandler>();
        if (playerInventoryHandler.HasItemInInventory(item))
        {
            playerInventoryHandler.RemoveItem(item);
            _showDialogue.ChangeDialogue(newDialogueIndex);
        }
    }
}
