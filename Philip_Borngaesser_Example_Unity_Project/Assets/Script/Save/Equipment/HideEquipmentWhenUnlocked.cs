using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Equipment_Controller))]
public class HideEquipmentWhenUnlocked : MonoBehaviour
{
    Equipment_Controller equipment_Controller;

    void Start()
    {
        equipment_Controller = GetComponent<Equipment_Controller>();
        bool hasBeenUnlocked = SaveStateManager.instance.EquipmentHasBeenUnlocked(equipment_Controller.equipment);
        gameObject.SetActive(!hasBeenUnlocked);
    }

    
}
