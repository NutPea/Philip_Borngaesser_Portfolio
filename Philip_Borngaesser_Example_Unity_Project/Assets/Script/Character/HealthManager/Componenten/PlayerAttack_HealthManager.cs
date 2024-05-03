using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
[RequireComponent(typeof(GridMovementController))]
public class PlayerAttack_HealthManager : MonoBehaviour
{
    GridMovementController _gridMovementController;
    HealthManager _healthManager;
    public Equipment.Item neededEquipment = Equipment.Item.Sword;
    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
        _gridMovementController = GetComponent<GridMovementController>();
        _gridMovementController.OnOccupiedGridMovementRequest.AddListener(DamageTaken);
    }

    void DamageTaken(Transform value)
    {
        PlayerEquipmentController playerEquipmentController = value.GetComponent<PlayerEquipmentController>();
        if (playerEquipmentController == null) return;
        if (playerEquipmentController.currentItemEnum != neededEquipment) return;
        _healthManager.OnDamageEvent.Invoke(1, TeamFlag.Player, value);
    }
}
