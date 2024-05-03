#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    private readonly Vector2 defaultNodeSize = new Vector2(150,400);
    private GUIStyle dialogeStyle;
    private List<DialogueNode> currentDialogueNodes = new List<DialogueNode>();
    public DialogueCollection dialogueCollection;

    public AllDialogueCollectionHolder _allDialogueCollectionHolder;
    public Blackboard blackboard;
    public DialogueGraph dialogueGraph;

    public DialogueGraphView()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0, grid);

        grid.StretchToParentSize();
        dialogeStyle = new GUIStyle();
        dialogeStyle.fixedHeight = 200;
        dialogeStyle.fixedWidth = 150;
        //AddElement(GenerateEntryPointNode());
    }

    private DialogueNode GenerateEntryPointNode()
    {
        var node = new DialogueNode
        {
            title = "Start",
            GUID = Guid.NewGuid().ToString(),
            dialogueText = "Entry Point",
            entryPoint = true
        };

        var generatePort = GeneratePort(node, Direction.Output);
        generatePort.portName = "Next";
        node.outputContainer.Add(generatePort);

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(100, 200, 100, 150));
        return node;
    }


    public void AddPropertyToBlackBoard(ExposedProperty exposedProperty)
    {
        var localPropertyName = exposedProperty.PropertyName;
        var localPropertyValue = exposedProperty.PropertyValue;

        while(dialogueCollection.exposedProperties.Any(x=> x.PropertyName == localPropertyName))
        {
            localPropertyName = $"{localPropertyName}(1)"; //USERNAME(1) || USERNAME(1)(1)
        }

        var property = new ExposedProperty();
        property.PropertyName = localPropertyName;
        property.PropertyValue = localPropertyValue;

        var container = new VisualElement();
        var blackboardField = new BlackboardField { text = property.PropertyName, typeText = "string property" };
        container.Add(blackboardField);

        var propertyValueTextField = new TextField("Value:")
        {
            value = localPropertyName
        };
        propertyValueTextField.RegisterValueChangedCallback(evt =>
        {
            var changingPropertyIndex = dialogueCollection.exposedProperties.FindIndex(x => x.PropertyName == property.PropertyName);
            dialogueCollection.exposedProperties[changingPropertyIndex].PropertyValue = evt.newValue;
        });

        var blackBoardValueRow = new BlackboardRow(propertyValueTextField, propertyValueTextField);
        container.Add(blackBoardValueRow);

        dialogueCollection.exposedProperties.Add(property);
        blackboard.Add(container);
    }

    public void DrawPropertyToBlackBoard(ExposedProperty exposedProperty)
    {
        var localPropertyName = exposedProperty.PropertyName;
        var localPropertyValue = exposedProperty.PropertyValue;

        var property = new ExposedProperty();
        property.PropertyName = localPropertyName;
        property.PropertyValue = localPropertyValue;

        var container = new VisualElement();
        var blackboardField = new BlackboardField { text = property.PropertyName, typeText = "string property" };
        container.Add(blackboardField);

        var propertyValueTextField = new TextField("Value:")
        {
            value = localPropertyName
        };
        propertyValueTextField.RegisterValueChangedCallback(evt =>
        {
            var changingPropertyIndex = dialogueCollection.exposedProperties.FindIndex(x => x.PropertyName == property.PropertyName);
            dialogueCollection.exposedProperties[changingPropertyIndex].PropertyValue = evt.newValue;
        });

        var blackBoardValueRow = new BlackboardRow(propertyValueTextField, propertyValueTextField);
        container.Add(blackBoardValueRow);

        blackboard.Add(container);
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {

        if (graphViewChange.elementsToRemove != null )
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                {
                    DialogueNode dialogueNode = elem as DialogueNode;
                    if (dialogueNode != null)
                    {
                        dialogueCollection.dialogueNodeDatas.Remove(dialogueNode.nodeData);
                        dialogueCollection.DeleteDialogueData(dialogueNode.nodeData);
                        EditorUtility.SetDirty(dialogueCollection);
                        AssetDatabase.SaveAssets();
                    }

                    Edge edge = elem as Edge;
                    if (edge != null)
                    {
                        DialogueNode dialogueNodeOutputPort = edge.output.node as DialogueNode;
                        DialogueNode dialogueNodeInput = edge.input.node as DialogueNode;
                        if (edge.output == dialogueNodeOutputPort.autoPort)
                        {
                            dialogueNodeOutputPort.nodeData.autoConnectedNode = null;
                        }
                        else
                        {
                            int index = dialogueNodeOutputPort.nodeData.connectedNodes.IndexOf(dialogueNodeInput.nodeData);
                            dialogueNodeOutputPort.nodeData.connectedNodes[index] = null; 
                        }

                        EditorUtility.SetDirty(dialogueNodeOutputPort.nodeData);
                        AssetDatabase.SaveAssets();
                    }
                }
            });
        }

        if (graphViewChange.movedElements != null)
        {
            graphViewChange.movedElements.ForEach(elem =>
            {
                DialogueNode dialogueNode = elem as DialogueNode;
                if (dialogueNode != null)
                {
                    EditorUtility.SetDirty(dialogueNode.nodeData);
                    AssetDatabase.SaveAssets();
                }
            });
        }


        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                DialogueNode outputDialoguePort = edge.output.node as DialogueNode;
                DialogueNode inputDialoguePort = edge.input.node as DialogueNode;
                if(edge.output == outputDialoguePort.autoPort)
                {
                    outputDialoguePort.nodeData.autoConnectedNode = inputDialoguePort.nodeData;
                }
                else
                {
                    int outPutNodeIndex = outputDialoguePort.outputContainer.IndexOf(edge.output);
                    outputDialoguePort.nodeData.connectedNodes[outPutNodeIndex] = inputDialoguePort.nodeData;
                }


                EditorUtility.SetDirty(outputDialoguePort.nodeData);
                AssetDatabase.SaveAssets();
            });
        }

        return graphViewChange;
    }

    public void AddGraphViewChange()
    {
        graphViewChanged += OnGraphViewChanged;
    }

    public void RemoveGraphViewChange()
    {
        graphViewChanged -= OnGraphViewChanged;
    }

    public void CreateNode(string nodeName, DialogueCollection dialogueCollection)
    {
        DialogueNode dialogueNode = CreteDialogeNode(nodeName, dialogueCollection,true);
        AddElement(dialogueNode);
        currentDialogueNodes.Add(dialogueNode);
    }

    public void CreateStartNode(string nodeName, DialogueCollection dialogueCollection)
    {
        DialogueNode dialogueNode = CreteStartDialogeNode(nodeName, dialogueCollection);
        AddElement(dialogueNode);
        currentDialogueNodes.Add(dialogueNode);
    }

    public void LoadNode(string nodeName, DialogueNodeData data,bool hasInputPort = true)
    {
        DialogueNode dialogueNode = LoadDialogueNode(nodeName, data, hasInputPort);
        AddElement(dialogueNode);
        currentDialogueNodes.Add(dialogueNode);
    }



    public void RemoveAllDialogueNodes()
    {
        foreach(DialogueNode node in currentDialogueNodes)
        {
            RemoveElement(node);
        }
        currentDialogueNodes.Clear();
    }

    public void RemoveAllEdges()
    {
        foreach (Edge edge in edges)
        {
            RemoveElement(edge);
        }
    }

    public void RefreshGraphView()
    {
        foreach (DialogueNode node in currentDialogueNodes)
        {
            node.RefreshExpandedState();
            node.RefreshPorts();
        }
    }

    private DialogueNode CreteDialogeNode(string nodeName,DialogueCollection dialogueCollection,bool addShowAutoNode = false)
    {
        DialogueNodeData dialogueNodeData =  dialogueCollection.CreateDialogueNode(typeof(DialogueNodeData));

        var dialogeNode = new DialogueNode
        {
            title = nodeName,
            dialogueText = nodeName,
            GUID = Guid.NewGuid().ToString()
        };
        dialogeNode.nodeData = dialogueNodeData;
        dialogueNodeData.GUID = dialogeNode.GUID;

        if (addShowAutoNode)
        {
            AddAutoChoisePort(dialogeNode);
        }

        var button = new Button(() =>
        {
            AddChoisePort(dialogeNode);
        });

        var inputPort = GeneratePort(dialogeNode, Direction.Input, Port.Capacity.Single);
        inputPort.portName = "Input";
        dialogeNode.inputContainer.Add(inputPort);
        dialogeNode.RefreshExpandedState();
        dialogeNode.RefreshPorts();

        var textField = new TextField("");
        textField.RegisterValueChangedCallback(evt =>
        {
            dialogueNodeData.dialogueText = evt.newValue;
            EditorUtility.SetDirty(dialogueNodeData);
            AssetDatabase.SaveAssets();
        });
        textField.SetValueWithoutNotify(dialogueNodeData.dialogueText);

        dialogeNode.mainContainer.Add(textField);


        Rect position = new Rect(Vector2.zero, defaultNodeSize);
        dialogeNode.SetPosition(position);
        dialogueNodeData.dialogueViewPosition = position;

        button.text = "New Choise";
        dialogeNode.titleContainer.Add(button);



        return dialogeNode;
    }

    private DialogueNode CreteStartDialogeNode(string nodeName, DialogueCollection dialogueCollection)
    {
        DialogueNodeData dialogueNodeData = dialogueCollection.CreateDialogueNode(typeof(DialogueNodeData));

        var dialogeNode = new DialogueNode
        {
            title = nodeName,
            dialogueText = nodeName,
            GUID = Guid.NewGuid().ToString()
        };
        dialogeNode.nodeData = dialogueNodeData;
        dialogueCollection.dialogueNodeDatas.Remove(dialogueNodeData);
        dialogueCollection.startNodeData = dialogueNodeData;
        dialogueNodeData.GUID = dialogeNode.GUID;
        var button = new Button(() =>
        {
            AddChoisePort(dialogeNode);
        });

        var textField = new TextField("");
        textField.RegisterValueChangedCallback(evt =>
        {
            dialogueNodeData.dialogueText = evt.newValue;
            EditorUtility.SetDirty(dialogueNodeData);
            AssetDatabase.SaveAssets();
        });
        textField.SetValueWithoutNotify(dialogueNodeData.dialogueText);
        dialogeNode.mainContainer.Add(textField);

        dialogeNode.SetPosition(new Rect(Vector2.zero, defaultNodeSize));

        button.text = "New Choise";
        dialogeNode.titleContainer.Add(button);

        Rect position = new Rect(100, 200, 100, 150);
        dialogeNode.SetPosition(position);
        dialogueNodeData.dialogueViewPosition = position;


        return dialogeNode;
    }

    public DialogueNode LoadDialogueNode(string nodeName, DialogueNodeData data, bool hasInputPort = true)
    {
        var dialogeNode = new DialogueNode
        {
            title = nodeName,
            dialogueText = data.dialogueText,
            GUID = data.GUID
        };
        dialogeNode.nodeData = data;

        var choiseButton = new Button(() =>
        {
            AddChoisePort(dialogeNode);
        });
        choiseButton.text = "New Choise";

        var decisionButton = new Button(() =>
        {
            AddDecisionPort(dialogeNode);
        });
        decisionButton.text = "New Decision";


        if (hasInputPort)
        {
            var inputPort = GeneratePort(dialogeNode, Direction.Input, Port.Capacity.Single);
            inputPort.portName = "Input";
            dialogeNode.inputContainer.Add(inputPort);
            dialogeNode.RefreshExpandedState();
            dialogeNode.RefreshPorts();
        }

        var textField = new TextField("");
        textField.RegisterValueChangedCallback(evt =>
        {
            data.dialogueText = evt.newValue;
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
        });
        textField.SetValueWithoutNotify(data.dialogueText);
        dialogeNode.mainContainer.Add(textField);

        dialogeNode.SetPosition(data.dialogueViewPosition);
        dialogeNode.GUID = data.GUID;


        dialogeNode.titleContainer.Add(choiseButton);
        dialogeNode.titleContainer.Add(decisionButton);

        return dialogeNode;
    }

    private void AddDecisionPort(DialogueNode dialogeNode)
    {
        GenerateOutputPort(dialogeNode,"",false);
        dialogeNode.nodeData.connectedNodes.Add(null);
        dialogeNode.nodeData.connectedNodesText.Add("");
    }

    public void LoadAllChoisePorts()
    {
        foreach(DialogueNode note in currentDialogueNodes)
        {
            AddAutoChoisePort(note);
            for (int index = 0; index < note.nodeData.connectedNodes.Count; index++)
            {
                GenerateOutputPort(note, note.nodeData.connectedNodesText[index]);
                note.RefreshExpandedState();
                note.RefreshPorts();
            }
        }
        
    }

    public void LoadAllEdges()
    {
        foreach (DialogueNode node in currentDialogueNodes)
        {
            if(node.nodeData.autoConnectedNode != null)
            {
                DialogueNodeData connectedNode = node.nodeData.autoConnectedNode;
                DialogueNode dialogueNode = GetDialogueNodeFromData(connectedNode);

                Port outputPort = node.autoPort as Port;
                Port inputPort = dialogueNode.inputContainer[0] as Port;
                Edge edge = outputPort.ConnectTo(inputPort);
                AddElement(edge);

                node.RefreshExpandedState();
                dialogueNode.RefreshPorts();
            }

            if (node.nodeData.connectedNodes.Count > 0)
            {
                for (int i = 0; i < node.nodeData.connectedNodes.Count; i++)
                {
                    if (node.nodeData.connectedNodes[i] == null) continue;
                    DialogueNodeData connectedNode = node.nodeData.connectedNodes[i];
                    DialogueNode dialogueNode = GetDialogueNodeFromData(connectedNode);

                    Port outputPort = node.outputContainer[i+1] as Port;
                    Port inputPort = dialogueNode.inputContainer[0] as Port;
                    Edge edge = outputPort.ConnectTo(inputPort);
                    AddElement(edge);

                    node.RefreshExpandedState();
                    dialogueNode.RefreshPorts();
                }
            }
        }
    }

    private DialogueNode GetDialogueNodeFromData(DialogueNodeData data)
    {
        DialogueNode dialogueNode = currentDialogueNodes[0];
        foreach(DialogueNode node in currentDialogueNodes)
        {
            if(node.GUID == data.GUID)
            {
                dialogueNode = node;
                break;
            }
        }
        return dialogueNode;
    }

    private void AddChoisePort(DialogueNode dialogeNode)
    {
        GenerateOutputPort(dialogeNode);
        dialogeNode.nodeData.connectedNodes.Add(null);
        dialogeNode.nodeData.connectedNodesText.Add("");
    }

    private void GenerateOutputPort(DialogueNode dialogeNode , string overriddenPortName ="",bool isChoise = true)
    {
        var generatedPort = GeneratePort(dialogeNode, Direction.Output);
        var outputPortCount = dialogeNode.outputContainer.Query("connector").ToList().Count;
        dialogeNode.outputContainer.Add(generatedPort);

        var choisePortName = string.IsNullOrEmpty(overriddenPortName) ? $"Choise{outputPortCount + 1}" : overriddenPortName;
        if (!isChoise)
        {
            choisePortName = string.IsNullOrEmpty(overriddenPortName) ? $"Decision{outputPortCount + 1}" : overriddenPortName;
        }
        generatedPort.portName = choisePortName;

        var textField = new TextField
        {
            name = string.Empty,
            value = choisePortName

        };
        textField.RegisterValueChangedCallback(evt => dialogeNode.nodeData.connectedNodesText[outputPortCount] = evt.newValue);
        generatedPort.contentContainer.Add(new Label("   "));
        generatedPort.contentContainer.Add(textField);

        var deleteButton = new Button(() => RemovePort(dialogeNode, outputPortCount-1))
        {
            text = "X"
        };

        var conditionButton = new Button(() => OpenDecision(dialogeNode))
        {
            text = "Add Condi"
        };

        generatedPort.contentContainer.Add(deleteButton);
        generatedPort.contentContainer.Add(conditionButton);
       

        dialogeNode.RefreshExpandedState();
        dialogeNode.RefreshPorts();
    }

    private void OpenDecision(DialogueNode currentUsedDialogueNode)
    {
        // Add Decisions
        VisualElement newElem = new VisualElement();

        var variableTextField = new TextField
        {
            name = string.Empty
        };

        var label = new Label("Condition :");
        label.AddManipulator(new ContextualMenuManipulator(BuildContextualMenu));
        newElem.Add(label);



        newElem.contentContainer.Add(label);
        currentUsedDialogueNode.outputContainer.Add(label);

    }


    void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction("SomeAction", OnMenuAction, DropdownMenuAction.AlwaysEnabled);
        evt.menu.AppendAction("OtherAction", OnMenuAction, DropdownMenuAction.AlwaysEnabled);
    }

    void OnMenuAction(DropdownMenuAction action)
    {
        Debug.Log(action.name);
    }

    private void AddAutoChoisePort(DialogueNode dialogueNode)
    {
        GenerateAutoOutputPort(dialogueNode);
    }

    private void GenerateAutoOutputPort(DialogueNode dialogeNode, string overriddenPortName = "")
    {
        var generatedPort = GeneratePort(dialogeNode, Direction.Output);
        dialogeNode.outputContainer.Add(generatedPort);

        generatedPort.portName = "AutoPort";
        dialogeNode.autoPort = generatedPort;

        dialogeNode.RefreshExpandedState();
        dialogeNode.RefreshPorts();
    }

    private void RemovePort(DialogueNode dialogeNode,int index)
    {
        DialogueNodeData data = dialogeNode.nodeData;
        data.connectedNodes.RemoveAt(index);
        data.connectedNodesText.RemoveAt(index);
        dialogeNode.outputContainer.Clear();

        dialogueGraph.LoadData();


    }

    private Port GeneratePort(DialogueNode node , Direction portDirection , Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }

    public override List<Port> GetCompatiblePorts(Port startPort,NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();


        ports.ForEach((port => {
            if(startPort != port && startPort.node != port.node)
            {
                compatiblePorts.Add(port);
            }
            
        
        }));

        return compatiblePorts;
    }
}
#endif
