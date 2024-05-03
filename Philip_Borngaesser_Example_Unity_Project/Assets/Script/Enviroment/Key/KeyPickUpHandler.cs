using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GridMovementController))]
public class KeyPickUpHandler : MonoBehaviour
{
    private GridMovementController _gridMovementController;
    public UnityEvent onKeyPickUp = new UnityEvent();


    private void Awake()
    {
        _gridMovementController = GetComponent<GridMovementController>();
        _gridMovementController.OnOccupiedGridMovementRequest.AddListener(OnInteractEvent);
        
    }

    private void OnInteractEvent(Transform player)
    {
        PlayerKeyHandler playerKeyHandler = player.GetComponent<PlayerKeyHandler>();
        playerKeyHandler.AddKey(1);
        onKeyPickUp.Invoke();
        gameObject.SetActive(false);
    }

   
}
