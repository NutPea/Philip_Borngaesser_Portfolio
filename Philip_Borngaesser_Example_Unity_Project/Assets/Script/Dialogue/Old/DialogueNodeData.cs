using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueNodeData : ScriptableObject
{
    public string dialogueText;
    public string GUID;
    [HideInInspector]public DialogueCollection dialogueCollection;
    [HideInInspector] public Rect dialogueViewPosition;
    public DialogueNodeData autoConnectedNode;
    public List<DialogueNodeData> connectedNodes = new List<DialogueNodeData>();
    public List<string> connectedNodesText = new List<string>();
    public Color nodeColor;


    public void RenameScriptableObject(string value)
    {
#if UNITY_EDITOR
        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(this), value);
#endif
    }

    public bool HasConnectedNodes()
    {
        if (connectedNodes.Count == 0) return false;
        bool oneNodeIsConnected = false;
        foreach(DialogueNodeData data in connectedNodes)
        {
            if(data != null)
            {
                oneNodeIsConnected = true;
                break;
            }
        }
        return oneNodeIsConnected;
        
    }

    public bool HasAutoConnection()
    {
        return autoConnectedNode != null;
    }
}
