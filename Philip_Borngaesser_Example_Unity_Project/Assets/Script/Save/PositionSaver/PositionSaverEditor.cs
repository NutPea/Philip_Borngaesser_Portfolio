#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PositionSaver))]
public class PositionSaverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PositionSaver positionSaver = (PositionSaver)target;

        if (!positionSaver.GUIDGotSet)
        {

            positionSaver.SetGUID();
            EditorUtility.SetDirty(positionSaver);
            positionSaver.GUIDGotSet = true;
        }


        if (GUILayout.Button("Generate GUID"))
        {
            positionSaver.GUIDGotSet = false;
            positionSaver.SetGUID();
            EditorUtility.SetDirty(positionSaver);
            positionSaver.GUIDGotSet = true;
        }

    }
}
#endif
