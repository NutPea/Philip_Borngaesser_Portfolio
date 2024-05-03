using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverSceneEquipmentMessanger : AMessange
{
    public Equipment.Item overSceneEquipment;
    Equipment_Controller equipmentController;

    public override void Clear(OverSceneMessanger messanger, OverSceneReceiver receiver)
    {
        Destroy(this);
    }

    public override void UseAwake(OverSceneReceiver receiver)
    {
        equipmentController = receiver.equipmentHolder.GetEquipment(overSceneEquipment).GetComponent<Equipment_Controller>();
        equipmentController.gameObject.SetActive(true);
        equipmentController.enabled = true;
        equipmentController.GetComponent<GridMovementController>().enabled = true;
        equipmentController.EquipToPlayer(receiver.player.transform);

    }

    public override void UseStart(OverSceneReceiver receiver)
    {
        receiver.overSceneEquipmentHasEquiped.Invoke(overSceneEquipment);
    }

    public override void UseLateStart(OverSceneReceiver receiver)
    {
    }
}
