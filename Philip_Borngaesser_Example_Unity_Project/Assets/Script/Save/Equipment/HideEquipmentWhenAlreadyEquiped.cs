using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Equipment_Controller))]
public class HideEquipmentWhenAlreadyEquiped : MonoBehaviour
{

    Equipment_Controller _equipmentController;

    private void Awake()
    {
        
    }

    void Start()
    {
        _equipmentController = GetComponent<Equipment_Controller>();
        OverSceneReceiver.instance.overSceneEquipmentHasEquiped.AddListener(OverSceneEquipmentChange);

    }

    private void OverSceneEquipmentChange(Equipment.Item item)
    {
        if(_equipmentController.equipment == item)
        {
            gameObject.SetActive(false);
        }
    }
}
