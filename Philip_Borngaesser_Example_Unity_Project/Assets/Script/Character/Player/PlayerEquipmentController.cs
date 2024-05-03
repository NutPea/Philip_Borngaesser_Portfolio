using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEquipmentController : MonoBehaviour
{

    public Equipment_Controller currentEquipment;
    public Equipment.Item currentItemEnum = Equipment.Item.None;
    public Transform playerSpriteRoot;
    public Transform rightHandBone;
    public Transform leftHandBone;
    public bool isSaveable = true;

    private Transform rootEquipmentTransform;
    private Transform leftHandEquipmentTransform;
    private Transform rightHandEquipmentTransform;
    private bool _stopInput;
    [HideInInspector] public bool stopDropping;
    [HideInInspector] public bool canPickUp = true;

    PlayerInput _inputActions;

    [HideInInspector] public UnityEvent<Equipment_Controller> onEquipmentHasUnlocked = new UnityEvent<Equipment_Controller>();
    [HideInInspector] public UnityEvent onEquipmentHideUnlock = new UnityEvent();
    [HideInInspector] public UnityEvent<Equipment.Item> onEquipItem = new UnityEvent<Equipment.Item>();

    private void Awake()
    {
        canPickUp = true;
    }

    private void Start()
    {
        _inputActions = InputManager.instance.inputActions;
        _inputActions.Keyboard.DropEquipment.performed += ctx => DropCurrentEquipment();
        _inputActions.Keyboard.Horizontal.performed += ctx => { onEquipmentHideUnlock.Invoke(); };
        _inputActions.Keyboard.Vertical.performed += ctx => { onEquipmentHideUnlock.Invoke(); };
    }

    

    public void DropCurrentEquipment()
    {

        if (currentEquipment == null || _stopInput || stopDropping) return;
        ResetSpriteHierachy();

        currentEquipment.DropEquipment(this);

        onEquipItem.Invoke(currentItemEnum);
    }

    public void ResetSpriteHierachy()
    {
        if (leftHandEquipmentTransform != null)
        {
            leftHandEquipmentTransform.transform.parent = rootEquipmentTransform.transform;
        }
        if (rightHandEquipmentTransform != null)
        {
            rightHandEquipmentTransform.transform.parent = rootEquipmentTransform.transform;
        }
    }

    public void EquipEquipment(Equipment_Controller equipment_Controller, Equipment.Item equipment)
    {
        if (SaveStateManager.instance.UnlockEquipment(equipment))
        {
            onEquipmentHasUnlocked.Invoke(equipment_Controller);
        }

        currentEquipment = equipment_Controller;
        currentItemEnum = equipment;

        switch (equipment)
        {
            case Equipment.Item.Sword:
                {
                    equipment_Controller.transform.parent = rightHandBone.transform;
                    rootEquipmentTransform = equipment_Controller.transform;
                    leftHandEquipmentTransform = null;
                    rightHandEquipmentTransform = null;

                    break;
                }
            case Equipment.Item.StrengthGloves:
                {
                    equipment_Controller.transform.parent = transform;
                    Transform leftGlove = equipment_Controller.transform.GetChild(1).GetChild(0);
                    Transform rightGlove = equipment_Controller.transform.GetChild(1).GetChild(1);
                    leftGlove.transform.parent = leftHandBone.transform;
                    rightGlove.transform.parent = rightHandBone.transform;
                    leftGlove.transform.localPosition = Vector3.zero;
                    rightGlove.transform.localPosition = Vector3.zero;

                    rootEquipmentTransform = equipment_Controller.transform.GetChild(1);
                    leftHandEquipmentTransform = leftGlove.transform;
                    rightHandEquipmentTransform = rightGlove.transform;

                    break;
                }
            case Equipment.Item.WateringCan:
                {
                    equipment_Controller.transform.parent = rightHandBone.transform;
                    rootEquipmentTransform = equipment_Controller.transform;
                    leftHandEquipmentTransform = null;
                    rightHandEquipmentTransform = null;

                    break;
                }
            case Equipment.Item.Torch:
                {
                    equipment_Controller.transform.parent = rightHandBone.transform;
                    rootEquipmentTransform = equipment_Controller.transform;
                    leftHandEquipmentTransform = null;
                    rightHandEquipmentTransform = null;

                    break;
                }
        }
        onEquipItem.Invoke(currentItemEnum);
    }

    public void SetInputIsBlocked(bool value)
    {
        _stopInput = value;
    }
}
