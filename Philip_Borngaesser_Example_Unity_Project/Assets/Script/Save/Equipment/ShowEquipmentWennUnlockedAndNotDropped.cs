using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Equipment_Controller))]
public class ShowEquipmentWennUnlockedAndNotDropped : MonoBehaviour
{
    Equipment_Controller _equipmentController;
    private SaveStateManager saveStateManager;

    void Start()
    {
        _equipmentController = GetComponent<Equipment_Controller>();
        saveStateManager = SaveStateManager.instance;
        bool canBeShown = saveStateManager.EquipmentHasBeenUnlocked(_equipmentController.equipment) && !saveStateManager.EquipmentHasBeenDropped(_equipmentController.equipment);
        gameObject.SetActive(canBeShown);

    }


}
