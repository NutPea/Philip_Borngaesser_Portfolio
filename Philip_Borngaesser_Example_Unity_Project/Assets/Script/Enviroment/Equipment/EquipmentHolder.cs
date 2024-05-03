using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentHolder : MonoBehaviour
{
    public GameObject sword;
    public GameObject gloves;
    public GameObject wateringCan;
    public GameObject tourch;

    List<EquipmentSaveState> droppedEquipmentsInScene = new List<EquipmentSaveState>();



    IEnumerator Start()
    {

        sword.SetActive(false);
        gloves.SetActive(false);
        droppedEquipmentsInScene = SaveStateManager.instance.GetDroppedItemsInScene();

        if (droppedEquipmentsInScene.Count > 0)
        {
            foreach (EquipmentSaveState saveState in droppedEquipmentsInScene)
            {
                if (saveState.hasBeenDropped)
                {
                    switch (saveState.item)
                    {
                        case Equipment.Item.Sword:
                            sword.SetActive(true);
                            Equipment_Controller swordEquipment = sword.GetComponent<Equipment_Controller>();
                            swordEquipment.stopStartMovement = true;
                            swordEquipment.enabled = true;
                            break;
                        case Equipment.Item.StrengthGloves:
                            gloves.SetActive(true);
                            Equipment_Controller glovesEquipment = gloves.GetComponent<Equipment_Controller>();
                            glovesEquipment.stopStartMovement = true;
                            glovesEquipment.enabled = true;
                            break;
                        case Equipment.Item.WateringCan:
                            wateringCan.SetActive(true);
                            Equipment_Controller wateringCanEquipment = wateringCan.GetComponent<Equipment_Controller>();
                            wateringCanEquipment.stopStartMovement = true;
                            wateringCanEquipment.enabled = true;
                            break;
                        case Equipment.Item.Torch:
                            tourch.SetActive(true);
                            Equipment_Controller tourchCanEquipment = tourch.GetComponent<Equipment_Controller>();
                            tourchCanEquipment.stopStartMovement = true;
                            tourchCanEquipment.enabled = true;
                            break;
                    }
                }
            }
        }

        yield return new WaitForEndOfFrame();

        if (droppedEquipmentsInScene.Count > 0)
        {
            foreach (EquipmentSaveState saveState in droppedEquipmentsInScene)
            {
                if (saveState.hasBeenDropped)
                {
                    switch (saveState.item)
                    {
                        case Equipment.Item.Sword:
                            Equipment_Controller swordEquipment = sword.GetComponent<Equipment_Controller>();
                            swordEquipment.PlaceAtMovementNode(MovementGrid.instance.grid.colum[saveState.droppedGridY].row[saveState.droppedGridX], true);
                            break;
                        case Equipment.Item.StrengthGloves:
                            Equipment_Controller gloveEquipment = gloves.GetComponent<Equipment_Controller>();
                            gloveEquipment.GetComponent<Equipment_Controller>().PlaceAtMovementNode(MovementGrid.instance.grid.colum[saveState.droppedGridY].row[saveState.droppedGridX], true);
                            break;
                    }
                }
            }
        }
    }


    public GameObject GetEquipment(Equipment.Item equipment)
    {
        GameObject currentGameobject = sword;
        switch (equipment)
        {
            case Equipment.Item.Sword:
                currentGameobject = sword; break;
            case Equipment.Item.StrengthGloves:
                currentGameobject = gloves; break;
            case Equipment.Item.WateringCan:
                currentGameobject = wateringCan; break;
            case Equipment.Item.Torch:
                currentGameobject = tourch; break;
        }
        return currentGameobject;
    }

    public void DeaktivateAllEquipments()
    {
        sword.SetActive(false);
        gloves.SetActive(false);
    }

    public void ActivateAllEquipments()
    {
        sword.SetActive(false);
        gloves.SetActive(false);
    }


}
