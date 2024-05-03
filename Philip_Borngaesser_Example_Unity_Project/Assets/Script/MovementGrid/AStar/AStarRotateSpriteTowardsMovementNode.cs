using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AStarMoveToMovementNode))]
public class AStarRotateSpriteTowardsMovementNode : MonoBehaviour
{

    private AStarMoveToMovementNode _movementNode;
    public Transform spriteRoot;
    private Vector3 startScaling;

    void Start()
    {
        _movementNode = GetComponent<AStarMoveToMovementNode>();
        _movementNode.OnMoveTowardsNode.AddListener(OnRotateSpriteRoot);
        startScaling = spriteRoot.transform.localScale;
    }

    private void OnRotateSpriteRoot(MovementNode node)
    {
        Vector2 dir = node.transform.position - transform.position;
        dir = dir.normalized;
        Debug.Log(dir);
        if(dir.x > 0)
        {
            spriteRoot.transform.localScale = startScaling;
        }
        else
        {
            spriteRoot.transform.localScale = new Vector3(-startScaling.x, startScaling.y, startScaling.z);
        }
    }


}
