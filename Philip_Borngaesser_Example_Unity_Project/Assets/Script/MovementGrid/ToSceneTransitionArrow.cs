using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToSceneTransitionArrow : MonoBehaviour
{
    public Transform arrowSprite;
    public int nodeDistance = 1;
    List<MovementNode> allMovementNodes = new List<MovementNode>();

    void Start()
    {
        arrowSprite.gameObject.SetActive(false);

        MovementNode firstNode = MovementGrid.instance.WorldToMovementNode(transform.position);
        allMovementNodes.Add(firstNode);
        allMovementNodes.Add(firstNode.upNeighbor);
        allMovementNodes.Add(firstNode.downNeighbor);
        allMovementNodes.Add(firstNode.rightNeighbor);
        allMovementNodes.Add(firstNode.leftNeighbor);

        for(int i = 0; i < allMovementNodes.Count; i++)
        {
            allMovementNodes[i].onArrivedAtNode.AddListener(ShowArrow);
            allMovementNodes[i].onAfterLeftNode.AddListener(ShowArrow);
        }

    }

    public void ShowArrow(bool isPlayer)
    {
        if (!isPlayer) return;
        arrowSprite.gameObject.SetActive(true);
    }

    public void RemoveArrow(bool isPlayer)
    {
        if (!isPlayer) return;
        bool hasPlayerOnIt = true;
        foreach(MovementNode node in allMovementNodes)
        {
            if (node.occupiedObject.isPlayer)
            {
                hasPlayerOnIt = true;
                break;
            }
        }

        if (!hasPlayerOnIt)
        {
            arrowSprite.gameObject.SetActive(false);
        }

    }

}
