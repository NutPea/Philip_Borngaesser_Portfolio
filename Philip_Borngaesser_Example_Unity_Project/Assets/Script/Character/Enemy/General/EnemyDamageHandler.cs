using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHandler : MonoBehaviour
{
    GridMovementController _gridMovementController;
    HealthManager _healthManager;
    void Start()
    {
        _gridMovementController = GetComponent<GridMovementController>();
        _gridMovementController.OnOccupiedGridMovementRequest.AddListener(DamageTaken);
        _healthManager = GetComponent<HealthManager>();
        _healthManager.OnCalculateDamage.AddListener(OnGotDamage);
    }

    private void OnGotDamage(bool dead, int arg1, Transform arg2)
    {
        if (dead)
        {

        }
    }

    void DamageTaken(Transform value)
    {
        value.gameObject.GetComponent<GridMovementController>().OnOccupiedGridMovementBackMessage.Invoke(transform);
    }

}
