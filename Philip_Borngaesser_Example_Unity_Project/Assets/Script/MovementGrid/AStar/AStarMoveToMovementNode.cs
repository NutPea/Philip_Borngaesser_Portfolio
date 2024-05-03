using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AStarPathfinder))]
[RequireComponent(typeof(GridMovementController))]
public class AStarMoveToMovementNode : MonoBehaviour
{

    AStarPathfinder _aStarPathfinder;
    MovementNode currentGoalNode;
    public float timeBetweenMoves = 0.1f;
    public float timeBetweenInteractions = 0.1f;

    GridMovementController _gridMovementController;
    public List<MovementNode> movementNodes;
    int currentMovementNodeIndex = 0;
    public GridMovementController playerMovementController;
    public bool showPath;
    bool goalNodeHasChanged;
    [HideInInspector] public UnityEvent OnReachWaypoint = new UnityEvent();
    [HideInInspector] public UnityEvent<MovementNode> OnMoveTowardsNode = new UnityEvent<MovementNode>();
    private bool _hasInteracted;
    public bool _stopMovement;


    void Awake()
    {
        _aStarPathfinder = GetComponent<AStarPathfinder>();
        _gridMovementController = GetComponent<GridMovementController>();
        _aStarPathfinder.OnPathHasBeenFound.AddListener(OnPathFound);
        _gridMovementController.OnGridMovementEnd.AddListener(OnNodeChange);
        _gridMovementController.OnGridInteractEnd.AddListener(InteractAgainAfterTime);
    }

    private void OnDisable()
    {
        StopMovement();
    }

    public void MoveToNode(MovementNode node)
    {
        if (_stopMovement) return;
        if (currentGoalNode == node) return;
        if (currentGoalNode == null)
        {
            _aStarPathfinder.StartFindPath(_gridMovementController.currentMovementNode, node);
        }
        else
        {
            goalNodeHasChanged = true;
        }
        if (_hasInteracted)
        {
            OnNodeChange();
            _hasInteracted = false;
        }
        currentGoalNode = node;
        OnMoveTowardsNode.Invoke(currentGoalNode);
    }

    private void OnPathFound(List<MovementNode> path)
    {
        movementNodes.Clear();
        movementNodes = path;
        movementNodes.Reverse();
        showPath = true;

        currentMovementNodeIndex = 0;
        _gridMovementController.MoveToTargetNode(movementNodes[currentMovementNodeIndex]);
    }
    private void OnNodeChange()
    {
        StartCoroutine(OnNodeChangeCoroutine(timeBetweenMoves));
    }
    public void InteractAgainAfterTime(MovementNode arg0)
    {
        if(_stopMovement) return;
        if (movementNodes.Count <= 0) return;
        if (!movementNodes[currentMovementNodeIndex].isOccupied) return;
        StartCoroutine(InteractAgainAfterTimeCourotine());
    }

    IEnumerator InteractAgainAfterTimeCourotine()
    {
        yield return new WaitForSeconds(timeBetweenInteractions);
        _gridMovementController.MoveToTargetNode(movementNodes[currentMovementNodeIndex]);
        _hasInteracted = true;
    }


    IEnumerator OnNodeChangeCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        currentMovementNodeIndex++;
        if (currentMovementNodeIndex >= movementNodes.Count)
        {
            RechedWaypoint();
        }
        else
        {
            if (movementNodes[currentMovementNodeIndex].isOccupied || goalNodeHasChanged)
            {
                _aStarPathfinder.StartFindPath(_gridMovementController.currentMovementNode, currentGoalNode);
            }
            else
            {
                _gridMovementController.MoveToTargetNode(movementNodes[currentMovementNodeIndex]);
            }
        }
    }

    void RechedWaypoint()
    {
        movementNodes.Clear();
        currentGoalNode = null;
        OnReachWaypoint.Invoke();
    }

    public void ResumeMovement()
    {
        _stopMovement = false;
        _gridMovementController.ResumeMovement();
        OnNodeChange();
    }
    public void StopMovement()
    {
        _stopMovement = true;
        _gridMovementController.StopMovement();
    }
    private void OnDrawGizmos()
    {
        if (showPath)
        {
            if (movementNodes.Count <= 0) return;
            Gizmos.color = Color.cyan;
            for (int index = 0; index < movementNodes.Count - 1; index++)
            {
                Gizmos.DrawLine(movementNodes[index].transform.position, movementNodes[index + 1].transform.position);
            }
        }
    }


}
