using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovementController))]
public class AttackPlayerOnInteraction : MonoBehaviour
{
    public int damageAmount = 1;
    private GridMovementController _gridMovementController;
    private HealthManager playerHealthManager;

    private void Start()
    {
        _gridMovementController = GetComponent<GridMovementController>();
        _gridMovementController.OnGridInteractEnd.AddListener(OnAttackPlayer);
    }

    private void OnAttackPlayer(MovementNode node)
    {
        if (node.occupiedObject == null) return;
        GameObject nodeGameobject = node.occupiedObject.gameObject;
        if (nodeGameobject.CompareTag("Player"))
        {
            if (playerHealthManager == null) playerHealthManager = nodeGameobject.GetComponent<HealthManager>();
            playerHealthManager.OnDamageEvent.Invoke(damageAmount, TeamFlag.Enemy, transform);
        }
        
    }

}
