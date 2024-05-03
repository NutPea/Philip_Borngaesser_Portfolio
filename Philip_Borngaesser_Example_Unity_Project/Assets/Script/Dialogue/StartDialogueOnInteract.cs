using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovementController))]
[RequireComponent(typeof(DialogueSystemTrigger))]
public class StartDialogueOnInteract : MonoBehaviour
{
    private GridMovementController gridMovement;
    private DialogueSystemTrigger dialogueSystem;
    
    void Start()
    {
        gridMovement= GetComponent<GridMovementController>();
        dialogueSystem= GetComponent<DialogueSystemTrigger>();

        gridMovement.OnOccupiedGridMovementRequest.AddListener(OnInteractByPlayer);
    }

    private void OnInteractByPlayer(Transform player)
    {
        GridMovementController playerGridMovement = player.GetComponent<GridMovementController>();
        if (playerGridMovement == null) return;
        if (!playerGridMovement.isPlayer) return;

        dialogueSystem.TryStart(null);
    }
}
