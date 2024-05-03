using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GridMovementController))]
public class BoxController : MonoBehaviour
{
    private GridMovementController _gridMovementController;
    public Equipment.Item neededEquipment = Equipment.Item.StrengthGloves;
    public UnityEvent onPlayerUses = new UnityEvent();

    void Start()
    {
        _gridMovementController = GetComponent<GridMovementController>();
        _gridMovementController.OnOccupiedGridMovementRequest.AddListener(MoveBox);
    }

    void MoveBox(Transform user)
    {
        PlayerEquipmentController playerEquipmentController = user.GetComponent<PlayerEquipmentController>();
        if (playerEquipmentController == null) return;
        if (playerEquipmentController.currentItemEnum != neededEquipment) return;
        onPlayerUses.Invoke();
        Vector3 direction = user.position - transform.position;
        _gridMovementController.MoveDependingOnDirVector(direction.normalized);
    }

}
