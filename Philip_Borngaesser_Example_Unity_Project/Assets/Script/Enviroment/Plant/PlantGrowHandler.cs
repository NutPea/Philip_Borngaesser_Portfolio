using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMovementController))]
public class PlantGrowHandler : MonoBehaviour
{
    private GridMovementController _gridMovementController;
    public Equipment.Item growItem;
    public Equipment.Item destoryItem;
    public GameObject seed;
    public GameObject plant;

    public bool hasGrown;
    public bool isDestroyed;

    void Start()
    {
        _gridMovementController = GetComponent<GridMovementController>();
        _gridMovementController.OnOccupiedGridMovementRequest.AddListener(OnHandlePlantInteraction);
    }

    private void OnHandlePlantInteraction(Transform player)
    {
        PlayerEquipmentController playerEquipmentController = player.GetComponent<PlayerEquipmentController>();
        if (playerEquipmentController == null) return;
        if (!hasGrown)
        {
            GrowPlant(playerEquipmentController.currentItemEnum);
        }
        else
        {
            if (isDestroyed)
            {
                DestroyPlant(playerEquipmentController.currentItemEnum);
            }
            else
            {
                GrowPlant(playerEquipmentController.currentItemEnum);
            }
        }
    }

    public void GrowPlant(Equipment.Item item)
    {
        if (item != growItem) return;
        //Play grow plant animation.

    }
    private void DestroyPlant(Equipment.Item item)
    {
        if (item != destoryItem) return;
        //Play destroy PLant Animation.
    }
}
