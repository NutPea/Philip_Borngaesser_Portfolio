using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SaveTriggerEvent))]
public class EquipmentUnlockDialogueTrigger : MonoBehaviour
{
    public DialogueCollection dialogueCollection;
    public Equipment_Controller equipment;
    private SaveTriggerEvent _saveTriggerEvent;

    private void Start()
    {
        equipment.onEquip.AddListener(PlayerUnlockedEquipment);
        _saveTriggerEvent = GetComponent<SaveTriggerEvent>();
    }

    public void PlayerUnlockedEquipment()
    {
        NonPlayerDialogueManager nonPlayerDialogueManager = GameObject.FindGameObjectWithTag("Player").GetComponent<NonPlayerDialogueManager>();
        nonPlayerDialogueManager.SetDialogue(dialogueCollection);
        nonPlayerDialogueManager.OnStartDialogue(nonPlayerDialogueManager.transform);
        _saveTriggerEvent.SaveEvent();
    }
}
