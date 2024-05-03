using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(KeyPickUpHandler))]
public class KeyPickUpSaveHandler : MonoBehaviour
{
    private KeyPickUpHandler _keyPickUpHandler;
    public int pickUpIndex = 0;
    void Start()
    {
        _keyPickUpHandler = GetComponent<KeyPickUpHandler>();
        _keyPickUpHandler.onKeyPickUp.AddListener(KeyPickUpEvent);
        if(SaveStateManager.instance.CheckKeyPickUpSaveState(SceneManager.GetActiveScene().buildIndex, pickUpIndex))
        {
            gameObject.SetActive(false);
        }
    }

    private void KeyPickUpEvent()
    {
        SaveStateManager.instance.SaveKeyPickUpSaveState(SceneManager.GetActiveScene().buildIndex, pickUpIndex);
    }

}
