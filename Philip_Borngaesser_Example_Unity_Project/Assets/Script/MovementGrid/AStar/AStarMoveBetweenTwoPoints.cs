using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarMoveBetweenTwoPoints : MonoBehaviour
{

    AStarPathfinder _aStarPathfinder;
    public Transform firstWaypoint;
    public Transform secondWaypoint;

    MovementNode _firstMovementNode;
    MovementNode _secondMovementNode;

    GridMovementController _gridMovementController;

    private void Awake()
    {
        _aStarPathfinder = GetComponent<AStarPathfinder>();
        _aStarPathfinder.OnPathHasBeenFound.AddListener(OnPathFound);
        _gridMovementController = GetComponent<GridMovementController>();

    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        _firstMovementNode = MovementGrid.instance.WorldToMovementNode(firstWaypoint.position);
        _secondMovementNode = MovementGrid.instance.WorldToMovementNode(secondWaypoint.position);
        _aStarPathfinder.StartFindPath(_gridMovementController.currentMovementNode,_firstMovementNode);
    }

    private void OnPathFound(List<MovementNode> list)
    {
        
    }



}
