using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerPickUpHandler : MonoBehaviour
{
    private PlayerEquipmentController playerEquipmentController;
    private GridMovementController gridMovementController;
    private GridMovementController pickedUpGridMovementController;
    [HideInInspector] public NPCPickUpHandler pickUpHandler;
    private Transform beforePickedUpTransformParent;

    public Equipment.Item neededPickUpItem = Equipment.Item.StrengthGloves;
    [HideInInspector] public bool hasPickedUpSomething;
    public Transform playerPickUpPosition;
    private PlayerInput _inputActions;

    public Transform leftArm;
    public Transform rightArm;


    [HideInInspector] public UnityEvent onPickUp;
    [HideInInspector] public UnityEvent onDrop;

    void Start()
    {
        playerEquipmentController = GetComponent<PlayerEquipmentController>();
        gridMovementController = GetComponent<GridMovementController>();

        gridMovementController.OnOccupiedGridMovementBackMessage.AddListener(OnTryPickUp);

        _inputActions = InputManager.instance.inputActions;
        _inputActions.Keyboard.DropEquipment.performed += ctx => OnDropPickUppedItem();
    }

    private void OnTryPickUp(Transform occupiedObject)
    {
        if (hasPickedUpSomething) return;
        if (playerEquipmentController.currentEquipment == null) return;
        if (playerEquipmentController.currentEquipment.equipment != neededPickUpItem) return;
        pickUpHandler = occupiedObject.GetComponent<NPCPickUpHandler>();
        if (pickUpHandler == null) return;

        pickedUpGridMovementController = occupiedObject.GetComponent<GridMovementController>();
        if (pickedUpGridMovementController.isMoving || pickedUpGridMovementController.isInteracting) return;



        PickUP(pickUpHandler, pickedUpGridMovementController);

    }

    public void PickUP(NPCPickUpHandler pickUpHandler , GridMovementController pickUppedMovementController)
    {
        this.pickUpHandler = pickUpHandler;
        this.pickedUpGridMovementController = pickUppedMovementController;
        this.pickUpHandler.OnPickUp();
        pickedUpGridMovementController.stopMovement = true;
        playerEquipmentController.stopDropping = true;
        playerEquipmentController.canPickUp = false;
        hasPickedUpSomething = true;

        pickedUpGridMovementController.GiveCurrentNodeFree();

        this.pickUpHandler.transform.localScale = Vector3.one;
        beforePickedUpTransformParent = pickUpHandler.transform.parent;
        this.pickUpHandler.transform.parent = playerPickUpPosition;
        this.pickUpHandler.pickUpPosition.localPosition = Vector3.zero;

        // pickUpHandler.transform.localPosition = pickUpHandler.pickUpPosition.transform.position;
        onPickUp.Invoke();
    }

    private void OnDropPickUppedItem()
    {
        if (!hasPickedUpSomething) return;
        if (gridMovementController.isMoving || gridMovementController.isInteracting) return;

        pickUpHandler.transform.parent = beforePickedUpTransformParent;

        pickUpHandler.transform.localScale = Vector3.one;
        MovementNode toDropNode = gridMovementController.currentMovementNode.FindFreeNodeAround();
        pickedUpGridMovementController.stopMovement = false;
        pickedUpGridMovementController.MoveToTargetNode(toDropNode);

        pickUpHandler.OnDrop();
        hasPickedUpSomething = false;
        Invoke(nameof(RemoveStopDropping), 0.1f);


        onDrop.Invoke();
      

    }

    private void RemoveStopDropping()
    {
        leftArm.transform.localScale = new Vector3(1, 1, 1);
        rightArm.transform.localScale = new Vector3(1, 1, 1);

        playerEquipmentController.stopDropping = false;
        playerEquipmentController.canPickUp = true;

    }
}
