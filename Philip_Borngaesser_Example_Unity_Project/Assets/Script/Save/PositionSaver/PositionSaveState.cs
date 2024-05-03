using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PositionSaveState
{
    public SceneNames.Name scene;
    public string GUID = "Wrong!";
    public Vector2 gridPosition = new Vector2();
}
