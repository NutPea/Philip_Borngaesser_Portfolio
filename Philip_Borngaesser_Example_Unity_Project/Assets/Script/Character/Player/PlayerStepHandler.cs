using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerStepHandler : MonoBehaviour
{
    public int maxStepAmount = 100;
    public int currentStepAmount;
    public bool stepDoesNotCounts;

    PlayerMovement _playerMovement;
    PlayerBootsHandler _playerBootsHandler;
    [HideInInspector]public UnityEvent onStepUpdate = new UnityEvent();
    [HideInInspector] public UnityEvent onStepDead = new UnityEvent();
    bool firstStepDontCount = true;

    public Button onResetButton;


    private void Awake()
    {
        currentStepAmount = maxStepAmount;
        _playerMovement = GetComponent<PlayerMovement>();
        _playerMovement.onStep.AddListener(OnUpdatePlayerMovement);

        _playerBootsHandler = GetComponent<PlayerBootsHandler>();
        _playerBootsHandler.onJump.AddListener(OnUpdatePlayerJump);

        onResetButton.onClick.AddListener(OnInstandDead);
    }

 

    private void OnInstandDead()
    {
        currentStepAmount = 0;
        onStepDead.Invoke();
        onStepUpdate.Invoke();
    }

    private void Start()
    {
        onStepUpdate.Invoke();
        InputManager.instance.inputActions.Keyboard.Reset.performed += ctx => { onStepDead.Invoke(); };
    }

    private void OnDisable()
    {
        _playerMovement.onStep.RemoveListener(OnUpdatePlayerMovement);
        _playerBootsHandler.onJump.RemoveListener(OnUpdatePlayerJump);
    }

    void OnUpdatePlayerMovement()
    {
        UpdateStepAmount(_playerMovement.stepCost);
    }

    void OnUpdatePlayerJump()
    {
        UpdateStepAmount(_playerBootsHandler.jumpLength);
    }

    void UpdateStepAmount(int cost)
    {
        if (stepDoesNotCounts) return;

        currentStepAmount -= cost;
        if(currentStepAmount <= 0)
        {
            onStepDead.Invoke();
        }
        onStepUpdate.Invoke();
    }
}
