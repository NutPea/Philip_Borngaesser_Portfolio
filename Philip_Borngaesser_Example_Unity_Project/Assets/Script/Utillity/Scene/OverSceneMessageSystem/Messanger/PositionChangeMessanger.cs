using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChangeMessanger : AMessange
{
    public Vector2 gridPos;
    GridMovementController playerGridMovement;

    public override void Clear(OverSceneMessanger messanger, OverSceneReceiver receiver)
    {
        Destroy(this);
    }


    public override void UseAwake(OverSceneReceiver receiver)
    {
        playerGridMovement = receiver.player.transform.GetComponent <GridMovementController>();
        playerGridMovement.placeOnMovementGridOnStart = false;
    }

    public override void UseStart(OverSceneReceiver receiver)
    {
    }
    public override void UseLateStart(OverSceneReceiver receiver)
    {
        playerGridMovement = receiver.player.transform.GetComponent<GridMovementController>();
        playerGridMovement.GiveCurrentNodeFree();
        MovementGrid grid = MovementGrid.instance;
        MovementNode node = grid.grid.colum[(int)gridPos.y].row[(int)gridPos.x];
        playerGridMovement.SetPositionnToMovementNode(node);

    }
}
