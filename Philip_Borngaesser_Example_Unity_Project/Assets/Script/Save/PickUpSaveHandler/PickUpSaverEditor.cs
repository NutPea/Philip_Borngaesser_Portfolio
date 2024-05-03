using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(PickUpSaver))]
public class PickUpSaverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PickUpSaver pickUpSaver = (PickUpSaver)target;

        if (!pickUpSaver.GUIDGotSet)
        {

            pickUpSaver.SetGUID();
            EditorUtility.SetDirty(pickUpSaver);
            pickUpSaver.GUIDGotSet = true;
        }


        if (GUILayout.Button("Generate GUID"))
        {
            pickUpSaver.GUIDGotSet = false;
            pickUpSaver.SetGUID();
            EditorUtility.SetDirty(pickUpSaver);
            pickUpSaver.GUIDGotSet = true;
        }

    }
}
