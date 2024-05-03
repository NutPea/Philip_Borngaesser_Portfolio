using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUnlockedUIHandler : MonoBehaviour
{
    private PlayerEquipmentController playerEquipmentController;
    public GameObject equipmentUnlockedContainer;
    public Image equipmentImage;
    public TextMeshProUGUI equipmentText;
    bool isOpened;
    void Start()
    {
        playerEquipmentController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEquipmentController>();
        playerEquipmentController.onEquipmentHasUnlocked.AddListener(ShowUnlock);
        playerEquipmentController.onEquipmentHideUnlock.AddListener(HideUnlock);
        equipmentUnlockedContainer.SetActive(false);
    }

    private void ShowUnlock(Equipment_Controller equipController)
    {
        equipmentUnlockedContainer.SetActive(true);
        isOpened = true;
    }

    private void HideUnlock()
    {
        if (!isOpened) return;
        isOpened = false;
        equipmentUnlockedContainer.SetActive(false);
    }
}
