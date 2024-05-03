using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarChaseController : MonoBehaviour
{
    public GridMovementController targetGridMovementController;
    private GridMovementController gridMovementController;
    private AStarMoveToMovementNode moveToMovementNode;
    public bool startChasingAtTheBegining;
    private bool _stopChaising;
    private NonPlayerDialogueManager nonPlayerDialogueManager;

    private void Start()
    {
        moveToMovementNode = GetComponent<AStarMoveToMovementNode>();
        gridMovementController = GetComponent<GridMovementController>();
        if (startChasingAtTheBegining)
        {
            StartChasing();
        }
    }
    public void StartChasing()
    {
        targetGridMovementController.OnGridMovementEnd.AddListener(UpdatePath);
        nonPlayerDialogueManager = targetGridMovementController.GetComponent<NonPlayerDialogueManager>();
        if (nonPlayerDialogueManager != null)
        {
            nonPlayerDialogueManager.onShowDialogue.AddListener(OnStopChaising);
            nonPlayerDialogueManager.onCloseDialogue.AddListener(OnContinueChaising);
        }
        UpdatePath();
    }

    private void OnDisable()
    {
        if (nonPlayerDialogueManager != null)
        {
            nonPlayerDialogueManager.onShowDialogue.RemoveListener(OnStopChaising);
            nonPlayerDialogueManager.onCloseDialogue.RemoveListener(OnContinueChaising);
        }
    }

    void UpdatePath()
    {
        if (_stopChaising) return;
        moveToMovementNode.MoveToNode(targetGridMovementController.currentMovementNode);
    }
    private void OnStopChaising(Transform arg0)
    {
        OnStopChaising();
    }

    public void OnStopChaising()
    {
        _stopChaising = true;
        moveToMovementNode.StopMovement();
    }

    public void OnContinueChaising()
    {
        _stopChaising = false;
        moveToMovementNode.ResumeMovement();
    }


}
