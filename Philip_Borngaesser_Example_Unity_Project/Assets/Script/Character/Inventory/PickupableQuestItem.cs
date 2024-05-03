using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GridMovementController))]
public class PickupableQuestItem : MonoBehaviour
{
    public QuestItem.Item questItem;
    private GridMovementController _gridMovementController;
    public UnityEvent onQuestItemGotPickedUp = new UnityEvent();

    private void Awake()
    {
        _gridMovementController = GetComponent<GridMovementController>();
        _gridMovementController.OnOccupiedGridMovementRequest.AddListener(OnPickUpQuestItemEvent);
    }

    private void Start()
    {
        if (SaveStateManager.instance.CheckQuestItemInInventory(questItem))
        {
            _gridMovementController.OnOccupiedGridMovementRequest.RemoveListener(OnPickUpQuestItemEvent);
            _gridMovementController.GiveCurrentNodeFree();
            gameObject.SetActive(false);
        }
    }

    private void OnPickUpQuestItemEvent(Transform requester)
    {
        if (requester.gameObject.CompareTag("Player"))
        {
            PlayerInventoryHandler playerInventoryHandler = requester.GetComponent<PlayerInventoryHandler>();
            playerInventoryHandler.PickUpItem(questItem);
            _gridMovementController.GiveCurrentNodeFree();
            onQuestItemGotPickedUp.Invoke();
            gameObject.SetActive(false);
        }
    }
}
