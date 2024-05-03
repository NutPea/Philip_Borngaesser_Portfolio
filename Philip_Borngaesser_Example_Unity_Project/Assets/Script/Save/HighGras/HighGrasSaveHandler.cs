using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SaveTriggerEvent))]
[RequireComponent(typeof(HighGrasController))]
public class HighGrasSaveHandler : MonoBehaviour
{
    private SaveTriggerEvent _saveTriggerEvent;
    private HighGrasController _highGrasController;
    // Start is called before the first frame update
    void Awake()
    {
        _saveTriggerEvent = GetComponent<SaveTriggerEvent>();
        _highGrasController = GetComponent<HighGrasController>();

        _saveTriggerEvent.onApplySaves.AddListener(CutGrasOnEvent);
        _highGrasController.onGrasGotCut.AddListener(SaveOnCutEvent);
    }

    private void SaveOnCutEvent()
    {
        if (_saveTriggerEvent.eventWasSaved) return;
        _saveTriggerEvent.SaveEvent();
    }

    private void CutGrasOnEvent()
    {
        _highGrasController.ForceCutGrass();
    }
}
