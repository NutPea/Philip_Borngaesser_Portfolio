using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "AllDialogueCollectionHolder", menuName = "Dialogue/AllDialogueCollectionHolder", order = 1)]
public class AllDialogueCollectionHolder : ScriptableObject
{
    public List<DialogueCollection> dialogueCollections = new List<DialogueCollection>();

#if UNITY_EDITOR

    public void SaveCollectionHolder()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

#endif
}
