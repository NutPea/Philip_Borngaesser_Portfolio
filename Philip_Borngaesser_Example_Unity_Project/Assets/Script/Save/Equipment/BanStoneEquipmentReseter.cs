using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanStoneEquipmentReseter : MonoBehaviour
{
    Equipment_Controller equipment_Controller;
    GridMovementController gridMovement;
    private MovementNode startMovementNode;

    public GameObject aktivateOnArrival;

    void Start()
    {
        startMovementNode = MovementGrid.instance.WorldToMovementNode(transform.position);
        gridMovement = GetComponent<GridMovementController>();
        gridMovement.OnGridMovementEnd.AddListener(AktivateOnArrival);
        equipment_Controller = GetComponent<Equipment_Controller>();
    }

    public void ResetBanStonePosition()
    {
        Debug.Log(SaveStateManager.instance.gameObject);
        equipment_Controller = GetComponent<Equipment_Controller>();
        if (!SaveStateManager.instance.EquipmentHasBeenUnlocked(equipment_Controller.equipment))
        {
            Debug.Log("NotUnlocked!");
            gameObject.SetActive(false);
            return;

        }
        else
        {
            Debug.Log("Unlocked!");
        }


        if (gridMovement == null)
        {
            gridMovement = GetComponent<GridMovementController>();
        }
        gridMovement.OnGridMovementEnd.AddListener(AktivateOnArrival);

        if (equipment_Controller.IsEquiped)
        {
            equipment_Controller.DropEquipmentAtNodeDirectly(OverSceneReceiver.instance.player.GetComponent<PlayerEquipmentController>(), startMovementNode);
        }
        else
        {
            gridMovement.MoveToTargetNodeImmediately(startMovementNode);
        }
        Debug.Log("HasBeenPlaced");
        gridMovement.PlaceOnNearestGridNode();
    }

    void AktivateOnArrival()
    {
        aktivateOnArrival.SetActive(true);
        gridMovement.OnGridMovementEnd.RemoveListener(AktivateOnArrival);
    }

    bool hasBeenNulled = false;
    bool hasBeenSet = false;
}
