using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBootsHandler : MonoBehaviour
{
    GridMovementController gridMovement;
    PlayerMovement playerMovement;

    PlayerInput _inputActions;
    public bool mirrorInputs;
    [Header("Saving")]
    public bool unlocked;
    public GameObject leftBoot;
    public GameObject rightBoot;

    [Header("Charging")]
    public float currentChargeAmount = 0;
    public float maxChargeAmount = 1;

    public float chargeInterval = 0.5f;
    private float currentChargeInterval = 0.0f;
    public float chargeIntervalAddAmount = 0.25f;

    private bool isCharging = false;
    [Header("Jumping")]
    public int jumpLength = 3;
    public enum JumpDirection { None,Up,Down,Right,Left};
    public JumpDirection currentJumpDirection;
    public int amountOfButtonPressed = 0;

    [Header("Cost")]
    [HideInInspector] public UnityEvent onJumpChargeStart = new UnityEvent();
    [HideInInspector] public UnityEvent onJumpChargeEnd = new UnityEvent();
    [HideInInspector] public UnityEvent onJumpFullCharge = new UnityEvent();
    private bool fullChargeTrigger;
    [HideInInspector] public UnityEvent onJump = new UnityEvent();


    private void Awake()
    {
        gridMovement = GetComponent<GridMovementController>();
        playerMovement = GetComponent<PlayerMovement>();
        
    }

    void Start()
    {
         _inputActions = InputManager.instance.inputActions;
        _inputActions.Keyboard.Jump.performed += ctx => OnStartCharge();
        _inputActions.Keyboard.Jump.canceled += ctx => OnStopCharge();
        currentChargeInterval = chargeInterval;

        _inputActions.Keyboard.Horizontal.performed += ctx => HandleHorizontal();
        _inputActions.Keyboard.Horizontal.canceled += ctx => ResetDirection();


        _inputActions.Keyboard.Vertical.performed += ctx => HandleVertical();
        _inputActions.Keyboard.Vertical.canceled += ctx => ResetDirection();


        unlocked = SaveStateManager.instance.itemsUnlockedSaveState.jumpBootsUnlocked;
        if (unlocked)
        {
            Unlock();
        }
        else
        {
            Lock();
        }

    }

    public void Unlock()
    {
        unlocked = true;
        SaveStateManager.instance.itemsUnlockedSaveState.jumpBootsUnlocked = true;
        leftBoot.SetActive(true);
        rightBoot.SetActive(true);
    }

    public void Lock()
    {
        unlocked = false;
        leftBoot.SetActive(false);
        rightBoot.SetActive(false);
    }

    private  void HandleHorizontal()
    {
        if (!isCharging || playerMovement.stopInput) return;
        float verticalMovement = _inputActions.Keyboard.Horizontal.ReadValue<float>();
        if (mirrorInputs) verticalMovement *= -1;
        if (verticalMovement > 0)
        {
            gridMovement.LookToTheRightSide();
            currentJumpDirection = JumpDirection.Right;
            amountOfButtonPressed++;
        }
        if (verticalMovement < 0)
        {
            gridMovement.LookToTheLeftSide();
            currentJumpDirection = JumpDirection.Left;
            amountOfButtonPressed++;
        }
    }

    private void HandleVertical()
    {
        if (!isCharging || playerMovement.stopInput) return;
        float verticalMovement = _inputActions.Keyboard.Vertical.ReadValue<float>();
        if (mirrorInputs) verticalMovement *= -1;
        if (verticalMovement > 0)
        {
            currentJumpDirection = JumpDirection.Up;
            amountOfButtonPressed++;
        }
        if (verticalMovement < 0)
        {
            currentJumpDirection = JumpDirection.Down;
            amountOfButtonPressed++;
        }
    }

    private void ResetDirection()
    {
        amountOfButtonPressed--;
        if (amountOfButtonPressed < 0) amountOfButtonPressed = 0;
        if (amountOfButtonPressed <= 0) currentJumpDirection = JumpDirection.None;

    }

    private void OnStartCharge()
    {
        if (!unlocked) return;
        isCharging = true;
        fullChargeTrigger = false;
        gridMovement.StopMovement();
        onJumpChargeStart.Invoke();

    }

    private void OnStopCharge()
    {
        if (!unlocked) return;
        gridMovement.ResumeMovement();
        if (currentChargeAmount >= maxChargeAmount)
        {
            MovementNode targetNode = null;
            switch (currentJumpDirection)
            {
                case JumpDirection.Up: targetNode = gridMovement.GetFarAwayDirectionUpNode(jumpLength);  break;
                case JumpDirection.Down: targetNode = gridMovement.GetFarAwayDirectionDownNode(jumpLength); break;
                case JumpDirection.Right: targetNode = gridMovement.GetFarAwayDirectionRightNode(jumpLength); break;
                case JumpDirection.Left: targetNode = gridMovement.GetFarAwayDirectionLeftNode(jumpLength); break;
            }
            if(targetNode != null)
            {
                if(!targetNode.isWalkable || !targetNode.isOccupied)
                {
                    gridMovement.MoveToTargetNode(targetNode);
                    onJump.Invoke();
                }
                else
                {
                    //Can Not Move Reation
                }
            }
        }
        isCharging = false;
        currentChargeAmount = 0;
        onJumpChargeEnd.Invoke();
        fullChargeTrigger = false;
    }

    
    void Update()
    {
        if (isCharging && !fullChargeTrigger)
        {
            if(currentChargeInterval < 0)
            {
                currentChargeInterval = chargeInterval;
                currentChargeAmount += chargeIntervalAddAmount;
                if(currentChargeAmount > maxChargeAmount)
                {
                    currentChargeAmount = maxChargeAmount;
                    onJumpFullCharge.Invoke();
                    fullChargeTrigger = true;
                }
            }
            else
            {
                currentChargeInterval -= Time.deltaTime;
            }
        }
    }
}
