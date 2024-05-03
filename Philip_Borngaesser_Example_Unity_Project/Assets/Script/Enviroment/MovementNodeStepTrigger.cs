using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementNodeStepTrigger : MonoBehaviour
{

    public MovementNode node;
    public UnityEvent<Transform> onPlayerSteppedTrigger = new UnityEvent<Transform>();

    void Start()
    {
        node = MovementGrid.instance.WorldToMovementNode(transform.position);
        node.onArrivedAtNode.AddListener(OnPlayerMoveEvent);
    }

    private void OnPlayerMoveEvent(bool isPlayer)
    {
        if (isPlayer)
        {
            onPlayerSteppedTrigger.Invoke(node.occupiedObject.transform);
        }
    }

    private void OnDisable()
    {
        if (node == null) return;
        node.onArrivedAtNode.RemoveListener(OnPlayerMoveEvent);
    }
}
