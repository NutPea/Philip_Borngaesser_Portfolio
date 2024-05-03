using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovementController))]
public class JumpingBootsUnlockHandler : MonoBehaviour
{
    GridMovementController _gridMovementController;

    private void Awake()
    {
        _gridMovementController = GetComponent<GridMovementController>();
    }
    void Start()
    {
        if (SaveStateManager.instance.itemsUnlockedSaveState.jumpBootsUnlocked)
        {
            gameObject.SetActive(false);
            return;
        }
        _gridMovementController.OnOccupiedGridMovementRequest.AddListener(UnlockBoots);
    }

    private void UnlockBoots(Transform player)
    {
        PlayerBootsHandler playerBootsHandler = player.GetComponent<PlayerBootsHandler>();
        if(playerBootsHandler == null)
        {
            Debug.Log("SomethingWentWrong");
            return;
        }
        playerBootsHandler.Unlock();
        gameObject.SetActive(false);
    }


}
