using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{

    public Animator animator;
    private GridMovementController _gridMovementController;
    private PlayerEquipmentController _playerEquipment;
    private PlayerStepHandler _playerStepHandler;
    private HealthManager _healthManager;
    private PlayerBootsHandler _playerBootsHandler;
    private PlayerPickUpHandler _playerPickUpHandler;

    public EquipmentAnimnationMovementSettings noEquipmentSettings;
    public EquipmentAnimnationMovementSettings swordEquipmentSettings;
    public EquipmentAnimnationMovementSettings gloveEquipmentSettings;

    void Start()
    {
        _gridMovementController = GetComponent<GridMovementController>();
        _gridMovementController.OnGridMovementStart.AddListener(OnStartMove);
        _gridMovementController.OnGridInteractStart.AddListener(OnStartInteract);
        _gridMovementController.OnMovementNotAllowed.AddListener(OnMoveNotAllowed);

        noEquipmentSettings.InitCurves(_gridMovementController);

        _playerEquipment = GetComponent<PlayerEquipmentController>();
        _playerEquipment.onEquipItem.AddListener(OnChangeInteractingCurve);

        _playerStepHandler = GetComponent<PlayerStepHandler>();
        _playerStepHandler.onStepDead.AddListener(OnStepDead);

        _healthManager = GetComponent<HealthManager>();
        _healthManager.OnDeath.AddListener(OnStepDead);

        _playerBootsHandler = GetComponent<PlayerBootsHandler>();
        _playerBootsHandler.onJumpChargeStart.AddListener(OnJumpChargeStart);
        _playerBootsHandler.onJumpChargeEnd.AddListener(OnJumpChargeStop);
        _playerBootsHandler.onJumpFullCharge.AddListener(OnJumpFullCharge);

        _playerPickUpHandler = GetComponent<PlayerPickUpHandler>();
        _playerPickUpHandler.onPickUp.AddListener(OnPickUp);
        _playerPickUpHandler.onDrop.AddListener(OnDrop);

    }

    private void OnPickUp()
    {
        animator.SetTrigger("OnPickUp");
    }

    private void OnDrop()
    {
        animator.SetTrigger("OnDrop");
    }

    private void OnJumpChargeStart()
    {
        animator.SetBool("JumpHold", true);
    }

    private void OnJumpChargeStop()
    {
        animator.SetBool("JumpHold", false);
    }

    private void OnJumpFullCharge()
    {
        animator.SetTrigger("OnJumpFullCharge");
    }

    private void OnStepDead()
    {
        animator.SetTrigger("OnDead");
    }

    private void OnMoveNotAllowed()
    {
        animator.SetTrigger("OnMoveNotAllowed");
    }

    private void OnChangeInteractingCurve(Equipment.Item item)
    {
        switch (item)
        {
                case Equipment.Item.None:
                {
                    noEquipmentSettings.SetInteractionCurves(_gridMovementController);
                    break;
                }
                case Equipment.Item.Sword:
                {
                    swordEquipmentSettings.SetInteractionCurves(_gridMovementController);
                    break;
                }
                case Equipment.Item.StrengthGloves:
                {
                    gloveEquipmentSettings.SetInteractionCurves(_gridMovementController);
                    break;
                }
        }
    }

    private void OnStartMove()
   {
        animator.SetTrigger("OnMove");
   }

    private void OnStartInteract(MovementNode arg0)
    {

        switch (_playerEquipment.currentItemEnum)
        {
            case Equipment.Item.None:
            {
                    animator.SetTrigger("OnInteract");
                    break;
            }
            case Equipment.Item.Sword:
            {
                    
                    float randomeValue = UnityEngine.Random.Range(0.0f, 1.0f);
                    if (randomeValue < 0.5f)
                    {
                        animator.SetTrigger("OnSwordAttackRight");
                    }
                    else
                    {
                        animator.SetTrigger("OnSwordAttackLeft");
                    }
                    break;
            }
            case Equipment.Item.StrengthGloves:
            {
                    animator.SetTrigger("OnPush");
                    break;
            }
        }
    }
}

[System.Serializable]
public class EquipmentAnimnationMovementSettings
{
    public AnimationCurve interactingCurve = AnimationCurve.Linear(0, 1, 1, 1);
    public Vector3 startScale_Interacting = new Vector3(1.5f, 0.5f, 1f);
    public Vector3 topScale_Interacting = new Vector3(0.5f, 1.5f, 1f);

    public void SetInteractionCurves(GridMovementController gridMovementController)
    {
        gridMovementController.interactingCurve = interactingCurve;
        gridMovementController.startScale_Interacting = startScale_Interacting;
        gridMovementController.topScale_Interacting = topScale_Interacting;
    }

    public void InitCurves(GridMovementController gridMovementController)
    {
        interactingCurve = gridMovementController.interactingCurve;
        startScale_Interacting = gridMovementController.startScale_Interacting;
        topScale_Interacting = gridMovementController.topScale_Interacting;
    }

}
