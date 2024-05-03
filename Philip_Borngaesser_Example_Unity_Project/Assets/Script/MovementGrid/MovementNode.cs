using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementNode :MonoBehaviour , IHeapItem<MovementNode>
{
    public bool isWalkable;
    public bool isOccupied;
    public GridMovementController occupiedObject;
    public GridMovementController occupiedRequesterObject;
    public MovementNode upNeighbor;
    public MovementNode rightNeighbor;
    public MovementNode leftNeighbor;
    public MovementNode downNeighbor;
    //true is Player , false is NPC
    public UnityEvent<bool> onArrivedAtNode = new UnityEvent<bool>();
    public UnityEvent<bool> onAfterLeftNode = new UnityEvent<bool>();

    public int gridX;
    public int gridY;

    public int g_Cost;
    public int h_Cost;
    public MovementNode parent;
    int heapIndex;

    public int HeapIndex { get => heapIndex;
        set => heapIndex = value;
    }

    public int f_Cost
    {
        get
        {
            return g_Cost + h_Cost;
        }
    }

    public bool IsFree()
    {
        return isWalkable && !isOccupied;
    }

    public MovementNode FindFreeNodeAround()
    {
        if(rightNeighbor != null)
        {
            if (rightNeighbor.IsFree())
            {
                return rightNeighbor;
            }
        }
        if(downNeighbor != null)
        {
            if (downNeighbor.IsFree())
            {
                return downNeighbor;
            }
        }
        if(leftNeighbor != null)
        {
            if (leftNeighbor.IsFree())
            {
                return leftNeighbor;
            }
        }
        if (upNeighbor != null)
        {
            if (upNeighbor.IsFree())
            {
                return upNeighbor;
            }
        }

        return null;
    }


    public List<MovementNode> GetAllNeighbours()
    {
        List<MovementNode> neighbors = new List<MovementNode>();

        if (upNeighbor != null) neighbors.Add(upNeighbor);
        if (rightNeighbor != null) neighbors.Add(rightNeighbor);
        if (downNeighbor != null) neighbors.Add(downNeighbor);
        if (leftNeighbor != null) neighbors.Add(leftNeighbor);

        return neighbors;
    }

    public int CompareTo(MovementNode other)
    {
        int compare = f_Cost.CompareTo(other.f_Cost);
        if (compare == 0)
        {
            compare = h_Cost.CompareTo(other.h_Cost);
        }
        return -compare;
    }
}
