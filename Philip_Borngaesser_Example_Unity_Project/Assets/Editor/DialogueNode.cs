#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogueNode : Node
{
    public string GUID;
    public string dialogueText;
    public bool entryPoint = false;
    public Port autoPort;
    public DialogueNodeData nodeData;

    public Port inputPort;
    public List<Port> outPutPorts = new List<Port>();




    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        nodeData.dialogueViewPosition = newPos;
    }

    public void ConnectToOtherNoteData()
    {
        
    }
}
#endif
