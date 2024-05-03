using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemHandlerBackMessanger : MonoBehaviour
{

    public GridMovementController _gridMovementController;
    [HideInInspector] public DropItemHandler dropItemHandler;


    void Start()
    {
        _gridMovementController = GetComponent<GridMovementController>();
        _gridMovementController.OnOccupiedGridMovementRequest.AddListener(OnPickedUpEvent);

    }

    private void OnPickedUpEvent(Transform arg0)
    {
        dropItemHandler.onItemGotPickedUp.Invoke();
    }

}
