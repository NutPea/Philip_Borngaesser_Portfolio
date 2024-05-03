using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickUpSaver : MonoBehaviour
{
    public string GUID;
    public bool GUIDGotSet;

    private GridMovementController _gridMovementController;
    public Pickupable.Item pickUpAble;
    [HideInInspector] public int savedSceneIndex = 0;
    bool hasSavedPosition;


    private void Awake()
    {
        _gridMovementController = GetComponent<GridMovementController>();
    }

    public void SavePickUpPosition()
    {
        SaveStateManager.instance.SavePickUpPosition(this);
    }


    void Start()
    {
        hasSavedPosition = SaveStateManager.instance.GetSavedPickUPPosition(this);
        Invoke(nameof(DisableIfWrongScene), 0.1f);
    }

    private void DisableIfWrongScene()
    {
        if (!hasSavedPosition) return;
        if (savedSceneIndex == SceneManager.GetActiveScene().buildIndex) return;

        gameObject.SetActive(false);
    }

    public void SetGUID()
    {
        if (Application.isPlaying) return;
        GUID = System.Guid.NewGuid().ToString();
    }
}
