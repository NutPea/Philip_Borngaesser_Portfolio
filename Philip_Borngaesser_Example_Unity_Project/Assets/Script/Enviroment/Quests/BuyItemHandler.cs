using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(GridMovementController))]
[RequireComponent(typeof(SaveTriggerEvent))]
public class BuyItemHandler : MonoBehaviour
{
    [SerializeField]
    [VariablePopup]
    private string variableString;
    GridMovementController movementController;
    SaveTriggerEvent saveTriggerEvent;
    public int price = 1;
    public TextMeshProUGUI priceText;

    public enum VariableSetter { None, Number, Bool }
    public VariableSetter variableSetter;

    [SerializeField] private bool toSetBool;
    [SerializeField] private int value;

    void Start()
    {

        movementController = GetComponent<GridMovementController>();
        movementController.OnOccupiedGridMovementRequest.AddListener(SetQuestItem);
        saveTriggerEvent = GetComponent<SaveTriggerEvent>();
        priceText?.GetComponentInChildren<TextMeshProUGUI>();
        if(price >= 0){
            priceText.text = "Free";
        }
        else{
            priceText.text = price.ToString();
        }
    }

    private void SetQuestItem(Transform player)
    {
        GridMovementController playerMovementController = player.GetComponent<GridMovementController>();
        if (!playerMovementController.isPlayer) return;

        PlayerMoneyHandler playerMoneyHandler = player.GetComponent<PlayerMoneyHandler>();
        if (playerMoneyHandler.currentMoneyAmount < price) return;

        playerMoneyHandler.currentMoneyAmount -= price;

        switch (variableSetter)
        {
            case VariableSetter.None: break;
            case VariableSetter.Number:
                int variableValue = DialogueLua.GetVariable(variableString).asInt;
                variableValue += value;
                DialogueLua.SetVariable(variableString, variableValue);
                break;
            case VariableSetter.Bool:
                bool boolVariableValue = DialogueLua.GetVariable(variableString).asBool;
                boolVariableValue = toSetBool;
                DialogueLua.SetVariable(variableString, boolVariableValue);
                break;
        }
        saveTriggerEvent.SaveEvent();

        gameObject.SetActive(false);
    }

}