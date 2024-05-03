using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnableSaveState 
{
    public string GUID;
    public Spawnables.Item item;
    public int sceneBuildIndex;
    public float xPosition;
    public float yPosition;
    public float zPosition;
}
