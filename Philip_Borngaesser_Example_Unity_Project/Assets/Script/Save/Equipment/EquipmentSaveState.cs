using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentSaveState
{
    public string name;
    public Equipment.Item item;
    public bool hasBeenUnlocked = false;
    public bool hasBeenDropped = false;
    public int dropedSceneIndex = -1;
    public int droppedGridX;
    public int droppedGridY;

    public void SetEquipmentDropSaveState(int currentIndex,MovementNode droppedMovementNode)
    {
        hasBeenDropped = true;
        this.dropedSceneIndex = currentIndex;
        this.droppedGridX = droppedMovementNode.gridX;
        this.droppedGridY = droppedMovementNode.gridY;
    }
}
