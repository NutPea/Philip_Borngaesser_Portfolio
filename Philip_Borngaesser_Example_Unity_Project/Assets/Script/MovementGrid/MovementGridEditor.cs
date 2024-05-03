#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovementGrid))]
public class MovementGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Grid"))
        {
            MovementGrid grid = (MovementGrid)target;
            grid.GenerateMovementGrid();
            EditorUtility.SetDirty(grid);
        }

        if (GUILayout.Button("Clear Grid"))
        {
            MovementGrid grid = (MovementGrid)target;
            grid.ClearList();
            grid.hasBeenGenerated = false;
        }
    }
}
#endif
