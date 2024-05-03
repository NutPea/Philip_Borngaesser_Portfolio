using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AStarPathfinder : MonoBehaviour
{
    MovementGrid movementGrid;
    public UnityEvent<List<MovementNode>>  OnPathHasBeenFound = new UnityEvent<List<MovementNode>>();

    void Start()
    {
        movementGrid = MovementGrid.instance;
    }

    public void StartFindPath(MovementNode start, MovementNode pathEnd)
    {
        StartCoroutine(FindPath(start, pathEnd));
    }

    IEnumerator FindPath(MovementNode startNode, MovementNode targetNode)
    {
        List<MovementNode> waypoints = new List<MovementNode>();
        bool pathSuccess = false;


        if (startNode.isWalkable && targetNode.isWalkable)
        {
            Heap<MovementNode> openSet = new Heap<MovementNode>(movementGrid.MaxGridSize);

            HashSet<MovementNode> closedSet = new HashSet<MovementNode>();
            openSet.Add(startNode);
            while (openSet.Count > 0)
            {
                MovementNode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    //PathFound;
                    pathSuccess = true;
                    break;
                }

                foreach (MovementNode neighbour in currentNode.GetAllNeighbours())
                {
                    if (!neighbour.isWalkable  || closedSet.Contains(neighbour))
                    {
                        continue;
                    }
                    if (neighbour.isOccupied)
                    {
                        if (targetNode != neighbour) continue;
                    }

                    int newMovementCost = currentNode.g_Cost + GetDistance(currentNode, neighbour);
                    if (newMovementCost < neighbour.g_Cost || !openSet.Contains(neighbour))
                    {
                        neighbour.g_Cost = newMovementCost;
                        neighbour.h_Cost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }

            }
        }

        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Count > 0;
            OnPathHasBeenFound.Invoke(waypoints);
        }
    }

    List<MovementNode> RetracePath(MovementNode startNode, MovementNode endNode)
    {
        List<MovementNode> retracedPath = new List<MovementNode>();
        MovementNode currentNode = endNode;

        while (currentNode != startNode)
        {
            retracedPath.Add(currentNode);
            currentNode = currentNode.parent;
        }

        return retracedPath;
    }


    int GetDistance(MovementNode nodeA, MovementNode nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }

    }
}
