using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PickUpSaveState 
{
    public int sceneIndex;
    public Pickupable.Item pickUpable = Pickupable.Item.None;
    public string GUID = "Wrong!";
    public float xPosition = 0;
    public float yPosition = 0;
    public float zPosition = 0;
    public string gameObjectName = "None";
}
