using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour
{
    GridMovementController _gridMovementController;
    HealthManager _healthManager;

    private void Awake()
    {
        _gridMovementController = GetComponent<GridMovementController>();
        _healthManager = GetComponent<HealthManager>();
        
    }

    void Start()
    {
        _gridMovementController.OnOccupiedGridMovementBackMessage.AddListener(OnGotDamage);
        _gridMovementController.OnOccupiedGridMovementRequest.AddListener(OnGotDamage);
    }


    public void OnGotDamage(Transform value)
    {
        AttackPlayerOnInteraction attackPlayerOnInteraction = value.GetComponent<AttackPlayerOnInteraction>();
        if (attackPlayerOnInteraction == null) return;
       // _healthManager.OnDamageEvent.Invoke(attackPlayerOnInteraction.damageAmount, TeamFlag.Enemy, value);
    }


  
}
