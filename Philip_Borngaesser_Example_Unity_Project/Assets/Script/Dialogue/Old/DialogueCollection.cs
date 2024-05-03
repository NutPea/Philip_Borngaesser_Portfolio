using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueCollection", menuName = "Dialogue/DialogueCollection", order = 1)]
public class DialogueCollection : ScriptableObject
{
    public DialogueNodeData startNodeData;
    public List<DialogueNodeData> dialogueNodeDatas = new List<DialogueNodeData>();
    [Header("Variables")]
    public List<ExposedProperty> exposedProperties = new List<ExposedProperty>();

    public DialogueNodeData CreateDialogueNode(System.Type type)
    {
        DialogueNodeData dialogueNodeData = ScriptableObject.CreateInstance(type) as DialogueNodeData;
        dialogueNodeData.name = type.Name;
        dialogueNodeDatas.Add(dialogueNodeData);
#if UNITY_EDITOR
        AssetDatabase.AddObjectToAsset(dialogueNodeData, this);
        AssetDatabase.SaveAssets();
#endif
        return dialogueNodeData;
    }


    public void DeleteDialogueData(DialogueNodeData dialogueNodeData)
    {
        dialogueNodeDatas.Remove(dialogueNodeData);
#if UNITY_EDITOR
        AssetDatabase.RemoveObjectFromAsset(dialogueNodeData);
        AssetDatabase.SaveAssets();
#endif
    }


    public void RemoveAllUnderObjectsFromAssets()
    {
#if UNITY_EDITOR
        UnityEngine.Object[] assets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(this));
        foreach (UnityEngine.Object obj in assets)
        {
            if (obj == null ||
               (!UnityEditor.AssetDatabase.IsMainAsset(obj) &&
                 !(obj is GameObject) &&
                 !(obj is Component))
               )
            {
                AssetDatabase.RemoveObjectFromAsset(obj);
            }
        }
        AssetDatabase.SaveAssets();
#endif
    }

#if UNITY_EDITOR
    public void RenameCollection(string name)
    {
        string assetPath = AssetDatabase.GetAssetPath(this);
        AssetDatabase.RenameAsset(assetPath, name);
    }

    public void DeleteThisAsset()
    {
        string assetPath = AssetDatabase.GetAssetPath(this);
        AssetDatabase.DeleteAsset(assetPath);
    }

#endif


}
