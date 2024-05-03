using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private GridMovementController _gridMovementController;
    public bool mirrorInput;

    private bool _moveVerticalHold;
    private bool _moveHorizontalHold;
    [HideInInspector]public bool stopInput;

    public float holdTimer = 0.5f;
    public float currentHoldTimer;

    PlayerInput _inputActions;

    public bool useUIButton;

    public Button upButton;
    public Button downButton;
    public Button rightButton;
    public Button leftButton;

    float verticalValue = 0f;
    float horizontalValue = 0f;


    #region handleInput

    [HideInInspector]public UnityEvent onStep = new UnityEvent();
    public int stepCost = 1;

    private void Awake()
    {

        currentHoldTimer = holdTimer;
        _gridMovementController = GetComponent<GridMovementController>();
    }

    private void Start()
    {
        _inputActions = InputManager.instance.inputActions;
        _inputActions.Keyboard.Horizontal.performed += ctx => MoveHorizontal();
        _inputActions.Keyboard.Vertical.performed += ctx => MoveVertical();

        _inputActions.Keyboard.Horizontal.canceled += ctx => ResetTimerHorizontal();
        _inputActions.Keyboard.Vertical.canceled += ctx => ResetTimerVertical();

    }


    void MoveVertical()
    {
        if (_gridMovementController.isMoving || stopInput || _gridMovementController.stopMovement) return;
        float verticalMovement = _inputActions.Keyboard.Vertical.ReadValue<float>();
        if (mirrorInput) verticalMovement *= -1;
        if (verticalMovement > 0)
        {
            _gridMovementController.MoveToUpNode();
        }
        if (verticalMovement < 0)
        {
            _gridMovementController.MoveToDownNode();
        }
        _moveVerticalHold = true;
        onStep.Invoke();
    }


    void MoveHorizontal()
    {
        if (_gridMovementController.isMoving || stopInput || _gridMovementController.stopMovement) return;
        float horizontalMovement = _inputActions.Keyboard.Horizontal.ReadValue<float>();
        if (mirrorInput) horizontalMovement *= -1;
        if (horizontalMovement > 0)
        {
            _gridMovementController.MoveToRightNode();
        }
        if (horizontalMovement < 0)
        {
            _gridMovementController.MoveToLeftNode();
        }
        _moveHorizontalHold = true;
        onStep.Invoke();
    }

    void ResetTimerVertical()
    {
        _moveVerticalHold = false;
        currentHoldTimer = holdTimer;
    }

    void ResetTimerHorizontal()
    {
        _moveHorizontalHold = false;
        currentHoldTimer = holdTimer;
    }

    private void Update()
    {
        if (_moveHorizontalHold)
        {
            if(currentHoldTimer < 0)
            {
                currentHoldTimer = holdTimer;
                MoveHorizontal();
            }
            else
            {
                currentHoldTimer -= Time.deltaTime;
            }
        }

        if (_moveVerticalHold)
        {
            if (currentHoldTimer < 0)
            {
                currentHoldTimer = holdTimer;
                MoveVertical();
            }
            else
            {
                currentHoldTimer -= Time.deltaTime;
            }
        }

    }

    private void MoveHorizontalButton()
    {
        if (horizontalValue > 0)
        {
            _gridMovementController.MoveToRightNode();
        }
        if (horizontalValue < 0)
        {
            _gridMovementController.MoveToLeftNode();
        }
    }

    private void MoveVerticalButton()
    {
        if (verticalValue > 0)
        {
            _gridMovementController.MoveToUpNode();
        }
        if (verticalValue < 0)
        {
            _gridMovementController.MoveToDownNode();
        }
    }

    public void SetInputIsBlocked(bool value)
    {
        stopInput = value;
    }

    #endregion


}
