#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;


[CustomEditor(typeof(SaveTriggerEvent))]
[CanEditMultipleObjects]
public class SaveTriggerEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SaveTriggerEvent saveTriggerEvent = (SaveTriggerEvent)target;
        if (GUILayout.Button("Generate GUID")) {
            saveTriggerEvent.GUIDGotSet = false;
            saveTriggerEvent.SetGUIDEditor();
            EditorUtility.SetDirty(saveTriggerEvent);
            saveTriggerEvent.GUIDGotSet = true;
        }

    }

}
#endif
