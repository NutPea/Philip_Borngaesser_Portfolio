using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GridMovementController))]
public class DropItemHandler : MonoBehaviour
{
    public GameObject dropablePrefab;

    private GridMovementController _gridMovementController;
    [HideInInspector]public UnityEvent onCantDropItem = new UnityEvent();
    [HideInInspector] public UnityEvent onDropItem = new UnityEvent();
    [HideInInspector] public UnityEvent onItemGotPickedUp = new UnityEvent();
    private void Start()
    {
        _gridMovementController = GetComponent<GridMovementController>();
    }


    public void _OnDropItem()
    {
        MovementNode toDropNode = _gridMovementController.currentMovementNode.FindFreeNodeAround();
        if (toDropNode == null) {
            onCantDropItem.Invoke();
            return;
        }

        GameObject item = Instantiate(dropablePrefab, transform.position, Quaternion.identity);
        GridMovementController itemGridMovementController = item.GetComponent<GridMovementController>();
        itemGridMovementController.MoveToTargetNode(toDropNode);
        Debug.Log(toDropNode);
        DropItemHandlerBackMessanger  dropItemHandlerBackMessanger = itemGridMovementController.gameObject.AddComponent<DropItemHandlerBackMessanger>();
        dropItemHandlerBackMessanger.dropItemHandler = this;
        onDropItem.Invoke();

    }
}
