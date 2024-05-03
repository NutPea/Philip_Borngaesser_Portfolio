using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GridMovementController))]
public class ResetEquipmentController : MonoBehaviour
{
    GridMovementController gridMovement;
    public EquipmentHolder equipmentHolder;
    public List<GameObject> banStoneEquipments;
    public bool giveAllEquipments;
    public UnityEvent onResetEverything = new UnityEvent();
    public UnityEvent onAnyEquipmentHasBeenUnlocked = new UnityEvent();

    public GameObject reseterPartikle;

    IEnumerator Start()
    {
        gridMovement = GetComponent<GridMovementController>();
        gridMovement.OnOccupiedGridMovementRequest.AddListener(OnInteracting);
        if (!SaveStateManager.instance.AnyEquipmentHasBeenUnlocked() && !giveAllEquipments)
        {
            reseterPartikle.SetActive(false);
        }
        else
        {
            onAnyEquipmentHasBeenUnlocked.Invoke();

        }
        yield return new WaitForEndOfFrame();
        if (giveAllEquipments)
        {
            foreach(GameObject equipment in banStoneEquipments)
            {
                equipment.SetActive(true);
                equipment.GetComponent<GridMovementController>().PlaceOnNearestGridNode();
            }
        }
    }

    private void OnInteracting(Transform arg0)
    {
        
        SaveStateManager.instance.GatherDroppedEquipment();
        equipmentHolder.DeaktivateAllEquipments();
        foreach(GameObject equipment in banStoneEquipments)
        {
            Equipment_Controller equipment_Controller = equipment.GetComponent<Equipment_Controller>();
            if (!equipment_Controller.IsEquiped)
            {
                equipment_Controller.gameObject.SetActive(true);
                BanStoneEquipmentReseter banStoneEquipmentReseter = equipment.GetComponent<BanStoneEquipmentReseter>();
                banStoneEquipmentReseter.ResetBanStonePosition();
            }
        }
        onResetEverything.Invoke();

    }



}
