#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueCollectionSelector : EditorWindow
{
    public DialogueGraph dialogueGraph;

    public List<string> allDialogueCollectionNames = new List<string>();
    private int selectedDialogeCollectionIndex = -1;
    private void OnGUI()
    {

        if (dialogueGraph == null )
        {
            return;
        }
        EditorGUI.BeginChangeCheck();
        selectedDialogeCollectionIndex = EditorGUILayout.Popup("DialogueCollections", selectedDialogeCollectionIndex, allDialogueCollectionNames.ToArray());
        if (EditorGUI.EndChangeCheck())
        {
            dialogueGraph.currentDialogueCollection = dialogueGraph._allDialogueCollectionHolder.dialogueCollections[selectedDialogeCollectionIndex];
            dialogueGraph.LoadData();
        }
    }

    public void SetDialogueCollectionNames(List<string> dialogueCollectionName)
    {
        foreach(string s in dialogueCollectionName)
        {
            allDialogueCollectionNames.Add(s);
        }
    }
}
#endif
