using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SaveTriggerEvent))]
[RequireComponent(typeof(PickupableQuestItem))]
public class PickupableSaveItemSaver : MonoBehaviour
{

    SaveTriggerEvent _saveTriggerEvent;
    PickupableQuestItem _questItem;


    void Awake()
    {
        _saveTriggerEvent = GetComponent<SaveTriggerEvent>();
        _saveTriggerEvent.onApplySaves.AddListener(OnDeactivateItem);

        _questItem = GetComponent<PickupableQuestItem>();
        _questItem.onQuestItemGotPickedUp.AddListener(OnSave);


    }

    private void OnSave()
    {
        _saveTriggerEvent.SaveEvent();
    }

    private void OnDeactivateItem()
    {
        gameObject.SetActive(false);
    }

}
